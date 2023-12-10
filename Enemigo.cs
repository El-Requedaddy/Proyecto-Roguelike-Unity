using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Enemigo : MonoBehaviour, IRecibeDaño, ILootObjetoPadre, IPersonajeJuego, IObtieneObjetoDePool {

    private SistemaVida sistemaVida;

    public event EventHandler<OnAtaqueEventArgs> OnAtaque; // Se usa para notificar cuando se ataca
    public class OnAtaqueEventArgs : EventArgs {
        public Objeto objeto;
    }

    [SerializeField] private int vida;
    [SerializeField] private float velocidad = 1f;
    [SerializeField] private EnemigoIA enemigoIA;
    [SerializeField] private Transform puntoObjeto; // Punto de spawn del objeto
    [SerializeField] private Objeto objeto; // Objeto que se puede lootear

    [SerializeField]private PoolObjetos poolObjetos; // Pool de objetos del arma  

    private bool estaMuerto = false;
    private bool estaAtacando = false;
    private bool estaChaseando = false;
    private bool estaEnCoolDown = false;

    EnemigoAnimator animator;
    [SerializeField] private float distanciaMinimaAtaque = 3f;
    [SerializeField] private float tiempoDeCoolDown = 4f;

    private void Start() {
        sistemaVida = new SistemaVida(vida);

        poolObjetos = PoolObjetos.Instance;
        animator = GetComponent<EnemigoAnimator>();
        StartCoroutine(EsperarInicializacionPool()); // Esperamos a que se inicialice el pool de objetos para sacar un objeto sin "Nurupo gyaah"
        animator.OnUsoObjetoTermina += Animator_OnUsoObjetoTermina;                                                      
    }

    private void Animator_OnUsoObjetoTermina(object sender, EventArgs e) {
        estaAtacando = false; // Cuando el ataque termina, se cambia el valor de la variable estaAtacando a false
    }

    private void Update() {
        GestionarMovimiento();
    }

    public IEnumerator EsperarInicializacionPool() {
        while (!poolObjetos.EstaDisponible()) {
            yield return null;
        }

        SacarObjetoDePool();
    }

    private void GestionarMovimiento() {
        if (!estaAtacando && !estaMuerto) {
            float distancia = Vector2.Distance(transform.position, Player.Instance.transform.position); // Distancia actual entre la IA y el jugador

            Vector3 direccionFinal = enemigoIA.ObtenerDireccionMovimiento(); // Obtenemos la dirección final

            //Rotación para seguir al player apuntando 
            float rotateSpeed = 10f;

            if (distancia >= enemigoIA.GetDistanciaMinima()) { // Si la distancia es mayor que la distancia mínima, rotamos hacia la dirección final
                transform.forward = Vector3.Slerp(transform.forward, direccionFinal, Time.deltaTime * rotateSpeed);
            } else { // Si la distancia es menor que la distancia mínima, rotamos hacia el jugador
                transform.forward = Vector3.Slerp(transform.forward, Player.Instance.transform.position, Time.deltaTime * rotateSpeed);
            }

            transform.position += direccionFinal * Time.deltaTime * velocidad;

            estaChaseando = direccionFinal != Vector3.zero; // Si la dirección final es distinta de 0 es que se está moviendo

            GestionarAtaque(direccionFinal * Time.deltaTime * velocidad);

        }
    }

    private void GestionarAtaque(Vector3 direccion) {
        if (!estaEnCoolDown && !estaAtacando) {
            //Debug.Log("Atacando porque se ma acercao pecha el chaval");
            float distancia = Vector3.Distance(transform.position, Player.Instance.transform.position); // Distancia actual entre la IA y el jugador
            if (distancia <= distanciaMinimaAtaque) { // Si la distancia es menor que la distancia mínima, atacamos
                estaAtacando = true;
                estaEnCoolDown = true;
                Arma arma = objeto as Arma;
                string tipoAtaqueArma = arma.GetTipoAtaque();
                OnAtaque?.Invoke(this, new OnAtaqueEventArgs { objeto = this.objeto });
                StartCoroutine(CoolDownAtaque());
            }
        }
    }

    private IEnumerator CoolDownAtaque() {
        Debug.Log("CoolDown");
        yield return new WaitForSeconds(tiempoDeCoolDown);
        estaEnCoolDown = false;
    }

    public void Morir() {
        if (!estaMuerto) {
            estaMuerto = true;
            EliminarFisicas();
            DevolverObjetoAPool();
            CleanObjeto();
            StartCoroutine(DestruirDespuesDeEsperar(2f));
        }
    }

    private IEnumerator DestruirDespuesDeEsperar(float tiempo) {
        yield return new WaitForSeconds(tiempo);
        Destroy(gameObject);
    }

    private void EliminarFisicas() {
        if (estaMuerto) { 
            Rigidbody cuerpo;
            cuerpo = GetComponent<Rigidbody>();
            cuerpo.isKinematic = true;
            cuerpo.velocity = Vector3.zero;
            cuerpo.freezeRotation = true;
            cuerpo.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    // Interfaz IPersonajeJuego
    public void SetEstaAtacando(bool a) {
        estaAtacando = a;
    }

    public bool EstaAtacando() {
        return estaAtacando;
    }

    public bool EstaMuerto() {
        return estaMuerto;
    }

    public bool EstaChaseando() {
        return estaChaseando;
    }

    // Interfaz IRecibeDaño
    public void RecibirDaño(float daño) {
        Debug.Log("Recibiendo daño");
        sistemaVida.RecibirDanio((int)daño);
        if (sistemaVida.GetPorcentajeVida() <= 0) {
            Morir();
        }
    }

    // Interfaz ILootObjetoPadre
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

    // Interfaz IObtieneObjetoDePool
    public void SacarObjetoDePool() { // Sacamos el objeto del pool
        Arma arma = poolObjetos.GetArmaDePool();
        arma.SetPadreObjeto(this);
        arma.SuscripcionAnimator();
    }

    public void DevolverObjetoAPool() { // Devolvemos el objeto al pool
        poolObjetos.DevolverObjetoAPool(objeto as Arma);
    }

    // Interfaz IPersonajeJuego
    public void ImponerCoolDown(float cooldown) {

    }
}