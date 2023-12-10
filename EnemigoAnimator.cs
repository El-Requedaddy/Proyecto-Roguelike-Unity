using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoAnimator : MonoBehaviour, IAnimatorUsaObjeto {

    private const string ESTA_CHASEANDO = "EstaChaseando";
    private const string TIENE_OBJETO = "TieneObjeto";
    private const string ESTA_MUERTO = "EstaMuerto";

    public event EventHandler OnUsoObjetoInicia;  // Evento que se dispara cuando el ataque inicia y se manda al arma
    public event EventHandler OnUsoObjetoTermina;  // Evento que se dispara cuando el ataque termina

    [SerializeField] Enemigo enemigo;
    private Animator animator;

    private string tipoAtaque; // Guarda el tipo de ataque que se está ejecutando
    private string estadoAnteriorEnemigo; // Guarda el estado anterior del enemigo para poder volver a ese estado cuando el ataque termine

    private void Awake() {
        animator = GetComponent<Animator>();
        enemigo.OnAtaque += Enemigo_OnAtaque;
    }

    private void Enemigo_OnAtaque(object sender, Enemigo.OnAtaqueEventArgs e) { // Cuando el enemigo ataca, se inicia la animación de ataque
        if (e.objeto as Arma != null) { // Si el objeto que se está usando es un arma, se inicia la animación de ataque usando su tipo de ataque
            Arma arma = e.objeto as Arma;
            IniciarAnimacionAtaque(arma.GetTipoAtaque());
        }
    }

    private void Update() {
        animator.SetBool(ESTA_CHASEANDO, enemigo.EstaChaseando());
        animator.SetBool(ESTA_MUERTO, enemigo.EstaMuerto());
        //animator.SetBool(TIENE_OBJETO, enemigo.TieneObjeto());
        ComprobarEstadoAtaqueYTerminar();
    }

    public void ComprobarEstadoAtaqueYTerminar() {
        if (tipoAtaque != null) {
            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName(tipoAtaque)); // Obtenemos el estado actual del animator
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(tipoAtaque) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f) { // Comprobamos si el ataque ha terminados
                OnAtaqueTerminado(); // Disparamos el evento OnAtaqueTermina dado que el ataque ha terminado
            }
        }
    }

    public void OnAtaqueTerminado() {

        //if (estadoAnteriorEnemigo == "Idle") { // Si el estado anterior del player era Idle, volvemos a poner el estado Idle
        //    animator.Play("Idle");
        //    Debug.Log("Volviendo a estado Idle");
        //} else if (estadoAnteriorEnemigo == "Walking") {
        //    Debug.Log("Volviendo a estado Walking");
        //    animator.Play("Chaseando"); // Si el estado anterior del player era Walking, volvemos a poner el estado Walking
        //}

        OnUsoObjetoTermina?.Invoke(this, EventArgs.Empty); // Disparamos el evento OnAtaqueTermina dado que el ataque ha terminado
    }

    public void IniciarAnimacionAtaque(string tipoAtaque) {
        if (enemigo.EstaChaseando()) {
            estadoAnteriorEnemigo = "Walking";
        } else {
            estadoAnteriorEnemigo = "Idle";
        }

        OnUsoObjetoInicia?.Invoke(this, EventArgs.Empty);

        string triggerAtaque = tipoAtaque; // Obtenemos el nombre del trigger que queremos activar
        animator.SetTrigger(triggerAtaque);
        this.tipoAtaque = tipoAtaque; // Guardamos el tipo de ataque para poder usarlo en el método Atacar()
        Debug.Log("Ataque iniciado, tipo de ataque: " + this.tipoAtaque);
    }

}
