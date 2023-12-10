using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IAnimatorUsaObjeto {

    private GameInput gameInput;

    public static PlayerAnimator Instance { get; private set; }

    public event EventHandler OnUsoObjetoInicia;  // Evento que se dispara cuando el ataque inicia y se manda al arma
    public event EventHandler OnUsoObjetoTermina;  // Evento que se dispara cuando el ataque termina

    private const string IS_WALKING = "IsWalking";
    private const string TIENE_OBJETO = "TieneObjeto";

    [SerializeField] Player player; // Referencia al script Player para poder acceder a sus variables.
    private Animator animator;

    private string tipoAtaque; // Variable que guarda el tipo de ataque que se está ejecutando
    private string estadoAnteriorPlayer; // Variable que guarda el estado anterior del player para poder volver a ese estado cuando el ataque termine

    private void Awake() {
        if (Instance != null) { // Si ya hay una instancia de PlayerAnimator, no se crea una nueva y se muestra un mensaje de error en la consola
            Debug.Log("Hay mas de un PlayerAnimator, ESPABILA NOTAS!!!");
        }
        Instance = this; // Asignamos la instancia de PlayerAnimator a la variable estática Instance
    }

    private void Start() {
        gameInput = FindObjectOfType<GameInput>();
        animator = GetComponent<Animator>();
        player.OnAtaque += Player_OnAtaque;
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking()); // Le pasamos al animator la variable isWalking para que pueda cambiar la animación
        animator.SetBool(TIENE_OBJETO, player.TieneObjeto()); // Le pasamos al animator la variable tieneObjeto para que pueda cambiar la animación

        ComprobarEstadoAtaqueYTerminar(); // Comprobamos si el ataque ha terminado para avisar a los scripts que estén suscritos al evento OnAtaqueTermina
    }

    private void Player_OnAtaque(object sender, Player.OnAtaqueEventArgs e) {
        if (e.objeto as Arma != null) { // Si el objeto que se está usando es un arma, se inicia la animación de ataque usando su tipo de ataque
            Arma arma = e.objeto as Arma;
            IniciarAnimacionAtaque(arma.GetTipoAtaque());
        }
    }

    public void ComprobarEstadoAtaqueYTerminar() {
        if (tipoAtaque != null) {
            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName(tipoAtaque)); // Obtenemos el estado actual del animator
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(tipoAtaque) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f) { // Comprobamos si el ataque ha terminados
                OnAtaqueTerminado(); // Disparamos el evento OnAtaqueTermina dado que el ataque ha terminado
            }
        }
    }

    public void OnAtaqueTerminado() {
        Debug.Log("Ataque Terminado");

        if (estadoAnteriorPlayer == "Idle") { // Si el estado anterior del player era Idle, volvemos a poner el estado Idle
            animator.Play("Idle");
        } else if (estadoAnteriorPlayer == "Walking") {
            animator.Play("Andando"); // Si el estado anterior del player era Walking, volvemos a poner el estado Walking
        }

        OnUsoObjetoTermina?.Invoke(this, EventArgs.Empty); // Disparamos el evento OnAtaqueTermina dado que el ataque ha terminado
    }

    public void IniciarAnimacionAtaque(string tipoAtaque) {
        Debug.Log("Iniciando animación de ataque");

        if (player.IsWalking()) {
            estadoAnteriorPlayer = "Walking";
        } else {
            estadoAnteriorPlayer = "Idle";
        }

        OnUsoObjetoInicia?.Invoke(this, EventArgs.Empty);

        string triggerAtaque = tipoAtaque; // Obtenemos el nombre del trigger que queremos activar
        animator.SetTrigger(triggerAtaque);
        this.tipoAtaque = tipoAtaque; // Guardamos el tipo de ataque para poder usarlo en el método Atacar()
    }
}
