using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    private const string PLAYER_PREFS_BINDINGS = "PlayerBindings";

    public static GameInput Instance { get; private set; }

    public event EventHandler OnAccionInteraccion;
    public event EventHandler OnUsoObjeto;
    public event EventHandler OnPausa;

    private bool isWalking;

    private InputJugadorActions inputActions;  // Referencia al script InputJugadorActions que contiene las acciones de los inputs

    public enum Binding {
        Mover_Up,
        Mover_Down,
        Mover_Left,
        Mover_Right,
        Interaccionar,
        Ataque,
        Pausa,
        Gamepad_Interaccionar,
        Gamepad_Ataque,
        Gamepad_Pausa
    }

    private void Awake() {
        if (Instance != null) { // Si ya hay una instancia de Player, no se crea una nueva y se muestra un mensaje de error en la consola
            Debug.Log("Hay mas de un GameInput, ESPABILA NOTAS!!!");
        }
        Instance = this; // Asignamos la instancia de Player a la variable estática Instance

        inputActions = new InputJugadorActions();  // Instanciamos el script InputJugadorActions

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        inputActions.Player.Enable();  // Activamos el input del player
        inputActions.Player.Interaccionar.performed += Interaccion_Realizada;  // Añadimos el evento de interactuar al input de interactuar
        inputActions.Player.Ataque.performed += UsoObjeto_performed;
        inputActions.Player.Pausa.performed += Pausa_performed;
    }

    private void OnDestroy() {
        inputActions.Player.Interaccionar.performed -= Interaccion_Realizada;  // Eliminamos el evento de interactuar al input de interactuar
        inputActions.Player.Ataque.performed -= UsoObjeto_performed;
        inputActions.Player.Pausa.performed -= Pausa_performed;

        inputActions.Dispose();  // Eliminamos el input del player
    }

    private void Pausa_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPausa?.Invoke(this, EventArgs.Empty);
    }

    private void UsoObjeto_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!Player.Instance.EstaAtacando()) {
            OnUsoObjeto?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Interaccion_Realizada(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnAccionInteraccion?.Invoke(this, EventArgs.Empty);  // Llamamos al evento de interactuar
    }

    public Vector2 GetMovementVectorNormalized() {  // Función que devuelve un vector normalizado con la dirección de movimiento del player
        Vector2 inputVector = inputActions.Player.Mover.ReadValue<Vector2>();  // No usamos un vector3 dado que el input solo tiene dos ejes, importante para mantener el código limpio

        inputVector = inputVector.normalized;  // Importante normalizar el vector en caso de que se mueva diagonalmente se ajuste a la velocidad deseada

        return inputVector;
    }
     
    public bool IsWalking() {
        return isWalking;
    } 

    public string GetTextoBinding(Binding bindeo) {
        switch (bindeo) {
            default:
            case Binding.Mover_Up:
                return inputActions.Player.Mover.bindings[1].ToDisplayString();
            case Binding.Mover_Down:
                return inputActions.Player.Mover.bindings[2].ToDisplayString();
            case Binding.Mover_Left:
                return inputActions.Player.Mover.bindings[3].ToDisplayString();
            case Binding.Mover_Right:
                return inputActions.Player.Mover.bindings[4].ToDisplayString();
            case Binding.Interaccionar:
                return inputActions.Player.Interaccionar.bindings[0].ToDisplayString();
            case Binding.Ataque:
                return inputActions.Player.Ataque.bindings[0].ToDisplayString();
            case Binding.Pausa:
                return inputActions.Player.Pausa.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interaccionar:
                return inputActions.Player.Interaccionar.bindings[1].ToDisplayString();
            case Binding.Gamepad_Ataque:
                return inputActions.Player.Ataque.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pausa:
                return inputActions.Player.Pausa.bindings[1].ToDisplayString();
        }
    }

    public void CambiarBinding(Binding bindeo, Action onAccion) {
        inputActions.Player.Disable(); // Desactivamos el input del player

        InputAction inputAction;
        int indiceBindeo;

        switch (bindeo) {
            default:
            case Binding.Mover_Up:
                inputAction = inputActions.Player.Mover;
                indiceBindeo = 1;
                break;
            case Binding.Mover_Down:
                inputAction = inputActions.Player.Mover;
                indiceBindeo = 2;
                break;
            case Binding.Mover_Left:
                inputAction = inputActions.Player.Mover;
                indiceBindeo = 3;
                break;
            case Binding.Mover_Right:
                inputAction = inputActions.Player.Mover;
                indiceBindeo = 4;
                break;

            case Binding.Interaccionar:
                inputAction = inputActions.Player.Interaccionar;
                indiceBindeo = 0;
                break;
            case Binding.Ataque:
                inputAction = inputActions.Player.Ataque;
                indiceBindeo = 0;
                break;
            case Binding.Pausa:
                inputAction = inputActions.Player.Pausa;
                indiceBindeo = 0;
                break;
            case Binding.Gamepad_Interaccionar:
                inputAction = inputActions.Player.Interaccionar;
                indiceBindeo = 1;
                break;
            case Binding.Gamepad_Ataque:
                inputAction = inputActions.Player.Ataque;
                indiceBindeo = 1;
                break;
            case Binding.Gamepad_Pausa:
                inputAction = inputActions.Player.Pausa;
                indiceBindeo = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(indiceBindeo).OnComplete (callback => {
            Debug.Log("Binding cambiado");
            callback.Dispose(); // Liberamos el callback para que no se quede en memoria
            inputActions.Player.Enable(); // Activamos el input del player
            onAccion(); // Llamamos a la acción que se ejecutará al terminar el binding

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, inputAction.SaveBindingOverridesAsJson()); // Guardamos los cambios en el binding como string en el playerprefs
            PlayerPrefs.Save(); // Guardamos los cambios en el playerprefs
        }).Start();
    }

}
