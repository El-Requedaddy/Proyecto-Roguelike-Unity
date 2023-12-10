using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PersonajeSO;

// TODO: Cuando entregue el proyecto, antes de desarrollar mas el Player urge refactorizar y separar en clases!!!

public class Player : MonoBehaviour, ILootObjetoPadre, IRecibeDa�o, IPersonajeJuego {

    [SerializeField] private PersonajeSO personaje;

    private List<Llave> llaves; // Lista de llaves que tiene el jugador

    public static Player Instance { get; private set; } // Creamos una variable est�tica de tipo Player para poder acceder a ella desde cualquier script sin necesidad de asignarla en el inspector de Unity

    public event EventHandler<OnObjetoSeleccionadoCambiaEventArgs> OnObjetoSeleccionadoCambia; // Se usa para notificar cuando se cambia el objeto seleccionado
    public class OnObjetoSeleccionadoCambiaEventArgs : EventArgs {
        public IElementoInteraccionable objetoSeleccionado;
    }

    public event EventHandler<OnAtaqueEventArgs> OnAtaque; // Se usa para notificar cuando se ataca
    public class OnAtaqueEventArgs : EventArgs {
        public Objeto objeto;
    }

    [SerializeField] private Transform puntoObjeto; // Punto de spawn del objeto

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;  // Referencia al script GameInput para poder acceder a sus variables. Se asigna el script en el inspector de Unity arrastr�ndolo de modo que no sea necesario asignarlo en el c�digo
    [SerializeField] private LayerMask objetosLayerMask;
    [SerializeField] private LayerMask layerElementosAbribles;
    [SerializeField] private ControladorAtaquesObjetos controladorAtaques; // Referencia al script ControladorAtaquesObjetos para poder acceder a sus variables. Se asigna el script en el inspector de Unity arrastr�ndolo de modo que no sea necesario asignarlo en el c�digo
    [SerializeField] private GestorBuffsJugador gestorBuffs;

    private Vector3 ultimaDireccionInteraccion; // Guardamos la �ltima direcci�n en la que se interactu� para que el player mire hacia esa direcci�n en el HandleInteraction
    private IElementoInteraccionable objetoSeleccionado;  // Guardamos el objeto seleccionado para poder interactuar con �l

    private Objeto objeto; // Instancia del objeto

    // Configura la m�scara de capa para excluir la capa "RadioInteraction"
    int radioInteractionLayer = 0;

    private LayerMask capasInteractuables;

    private bool isWalking; // Variable que indica si el player est� caminando
    private bool estaAtacando = false; // Variable que indica si el player est� atacando
    private bool estaMuerto = false; // Variable que indica si el player est� muerto

    private string tipoAtaque = null; // Variable que indica el tipo de ataque que se est� realizando

    private void Awake() {
        capasInteractuables = objetosLayerMask | layerElementosAbribles; // Se combinan las dos capas para poder interactuar con los objetos y las puertas
        llaves = new List<Llave>();

        radioInteractionLayer = LayerMask.NameToLayer("Objetos");
        if (Instance != null) { // Si ya hay una instancia de Player, no se crea una nueva y se muestra un mensaje de error en la consola
            Debug.Log("Hay mas de un Player, ESPABILA NOTAS!!!");
        }
        Instance = this; // Asignamos la instancia de Player a la variable est�tica Instance
    }

    private void Update() {
        if (estaMuerto) {
            return;
        }
        GestionarMovimiento();
        GestionarInteracciones();
    }

    private void Start() {
        personaje.InicializarPersonaje();

        gameInput = FindObjectOfType<GameInput>();
        if (gameInput == null) {
            Debug.LogError("No se encontr� el GameInput");
        }

        gameInput.OnAccionInteraccion += GameInput_OnAccionInteraccion;
        gameInput.OnUsoObjeto += GameInput_OnAtaque;
        PlayerAnimator.Instance.OnUsoObjetoTermina += PlayerAnimator_OnAtaqueTermina;
        AsignarCamaraAJugador();
    }

    private void GameInput_OnAtaque(object sender, EventArgs e) {
        if (TieneObjeto() && objeto is Arma) {
            estaAtacando = true; // Cuando se pulsa el bot�n de atacar, se cambia el valor de la variable estaAtacando a true
            OnAtaque?.Invoke(this, new OnAtaqueEventArgs { objeto = this.objeto });
            //Arma arma = objeto as Arma;
            //string tipoAtaqueArma = arma.GetTipoAtaque();



            //if (objeto is Arma) {
            //    Atacar();
            //} else if (objeto is Consumible) {
            //    Consumir();
            //}
        }
    }

    private void PlayerAnimator_OnAtaqueTermina(object sender, System.EventArgs e) {
        Debug.Log("PlayerAnimator_OnAtaqueTermina");
        estaAtacando = false; // Cuando el ataque termina, se cambia el valor de la variable estaAtacando a false
    }

    private void GameInput_OnAccionInteraccion(object sender, System.EventArgs e) { // Evento que se ejecuta cuando se pulsa el bot�n de interactuar con el que la interacci�n con el objeto se gestiona en el script del objeto
        if (objetoSeleccionado != null) { // Si hay un objeto seleccionado, se ejecuta el c�digo dentro del if
            objetoSeleccionado.Interactuar(this);
        }
    }

    private void GestionarInteracciones() {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();  // Obtenemos el vector normalizado de la direcci�n de movimiento del player
        Vector3 moveDir = new Vector3(InputVector.x, 0f, InputVector.y);  // Creamos un vector3 para poder modificar la posici�n del player

        if (moveDir != Vector3.zero) { // Si no se mueve el player la ultima direccion de interaccion no cambia
            ultimaDireccionInteraccion = moveDir;
        }

        float distanciaInteraccion = 2f;  // Distancia a la que se puede interactuar con un objeto

        if (Physics.Raycast(transform.position, ultimaDireccionInteraccion, out RaycastHit raycastHit, distanciaInteraccion, capasInteractuables)) { // Si el rayo colisiona con algo, se ejecuta el c�digo dentro del if
            if (raycastHit.transform.TryGetComponent(out IElementoInteraccionable objeto)) {
                if (objetoSeleccionado != objeto) {
                    SetObjetoSeleccionado(objeto);
                }
            } else {
                SetObjetoSeleccionado(null);
            }
        } else {
            SetObjetoSeleccionado(null);
        }
    }

    private void GestionarMovimiento() { // Funci�n que gestiona el movimiento del player 
        if (!estaAtacando) { // Si el player no est� atacando, se ejecuta el c�digo dentro del if

            int layerMask = ~(1 << radioInteractionLayer); // Configura la m�scara de capa para excluir la capa "RadioInteraction"

            Vector2 inputVector = gameInput.GetMovementVectorNormalized();  // Obtenemos el vector normalizado de la direcci�n de movimiento del player
            Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);  // Creamos un vector3 para poder modificar la posici�n del player

            float distanciaMovimiento = moveSpeed * Time.deltaTime; // Calculamos la distancia que se ha movido el player en el frame actual
            float radioPlayer = .7f;  // Radio del player
            float alturaPlayer = 2f;  // Altura del player
            bool sePuedeMover = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * alturaPlayer, radioPlayer, moveDir, distanciaMovimiento, layerMask);

            if (!sePuedeMover) { // Si no se puede mover en esa direcci�n, comprobamos si se puede mover en la direcci�n X o Z
                Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
                sePuedeMover = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * alturaPlayer, radioPlayer, moveDirX, distanciaMovimiento, layerMask);

                if (sePuedeMover) { // Si se puede mover en la direcci�n X, cambiamos el moveDir a esa direcci�n
                    moveDir = moveDirX;
                } else { // Si no se puede mover en la direcci�n X, comprobamos si se puede mover en la direcci�n Z
                    Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                    sePuedeMover = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * alturaPlayer, radioPlayer, moveDirZ, distanciaMovimiento, layerMask);

                    if (sePuedeMover) { // Si se puede mover en la direcci�n Z, cambiamos el moveDir a esa direcci�n
                        moveDir = moveDirZ;
                    } else {
                        
                    }
                }
            }

            if (sePuedeMover) {
                transform.position += moveDir * distanciaMovimiento;
            }

            isWalking = moveDir != Vector3.zero;  // Si el moveDir es distinto de 0 es que se est� moviendo

            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);  // Hacemos que el player mire hacia donde se mueve gracias a Slerp que es una interpolaci�n entre dos vectores y Time.deltaTime para que sea independiente de la velocidad del procesador. Movedir es el vector que queremos que mire el player

            //Debug.Log(inputVector);
        }
    }

    private void SetObjetoSeleccionado(IElementoInteraccionable objeto) {
        this.objetoSeleccionado = objeto;
        OnObjetoSeleccionadoCambia?.Invoke(this, new OnObjetoSeleccionadoCambiaEventArgs { objetoSeleccionado = objetoSeleccionado }); // Se ejecuta el evento OnObjetoSeleccionadoCambia
    } 

    public bool IsWalking() { // Funci�n que devuelve si el player est� caminando o no para que el animator pueda cambiar la animaci�n
        return isWalking;
    }

    // Interfaz IPersonajeJuego
    public void SetEstaAtacando(bool a) {
        estaAtacando = a;
    }

    public bool EstaAtacando() {
        return estaAtacando;
    }

    public bool EstaMuerto() {
        return false;
    }

    public void CancelarAtaque() {
        estaAtacando = false;
    }

    public Transform GetLootObjetoSeguirTransformada() {
        return puntoObjeto;
    }

    public void SetObjeto(Objeto objeto) {
        this.objeto = objeto;
    }

    public Objeto GetObjeto() {
        return objeto;
    }

    public void CleanObjeto() {
        objeto = null;
    }

    public bool TieneObjeto() {
        return objeto != null;
    }

    public void SetearComoPadre(Objeto hijo) {
        hijo.transform.parent = this.GetLootObjetoSeguirTransformada();
    }

    // Interfaz IPersonajeJuego
    public void ImponerCoolDown(float cooldown) {

    }

    // Interfaz IRecibeDa�o
    public void RecibirDa�o(float da�o) {
        personaje.sistemaVida.RecibirDanio((int)da�o);
        if (personaje.sistemaVida.GetPorcentajeVida() <= 0) {
            Morir();
        }
        
    }

    public void Morir() {
        if (!estaMuerto) {
            estaMuerto = true;
            Time.timeScale = 0;
        }
    }

    private void AsignarCamaraAJugador() {
        var virtualCameraComponent = FindObjectOfType<CinemachineVirtualCamera>();

        if (virtualCameraComponent != null) {
            virtualCameraComponent.Follow = transform;
        } else {
            Debug.LogError("No se encontr� una virtual camera en la escena.");
        }
    }

    public void AnadirLlave(Llave llave) {
        llaves.Add(llave);
    }

    public bool UsarLlave(TipoLlave tipoLlave) {
        for (int i = 0; i < llaves.Count; i++) {
            if (llaves[i].tipoLlave == tipoLlave) {
                llaves.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public bool TieneLlave() {
        return llaves.Count > 0;
    }

}
