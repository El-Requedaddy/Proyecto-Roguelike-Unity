using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Arma : Objeto {

    private string tipoAtaque;

    private List<GameObject> objetosColisionados = new List<GameObject>();
    Collider hitbox;

    [SerializeField] private LayerMask recibeDañoLayerMask;

    private LootObjetoArmaSO scriptableArma;

    private float cooldownHitbox = 1f;

    public abstract void Atacar();

    private bool estaEnCooldown = false;

    private void Start() {
        scriptableArma = GetLootObjetoSO() as LootObjetoArmaSO; // Obtenemos el scriptable object del arma en base al de objeto
        IAnimatorUsaObjeto animator = GetComponentInParent<IAnimatorUsaObjeto>(); // Cogemos el animator del padre
        hitbox = GetComponent<BoxCollider>(); // Obtenemos el componente de la hitbox
        hitbox.enabled = false;

    }

    private void OnDisable() { // Nos desuscribimos a los eventos de ataque del animator cuando se desactiva el objeto ahorrando memoria
        IAnimatorUsaObjeto animator = GetComponentInParent<IAnimatorUsaObjeto>();
        if (animator != null) {
            animator.OnUsoObjetoInicia -= Animator_OnAtaqueComienza;
            animator.OnUsoObjetoTermina -= Animator_OnAtaqueTermina;
        }
    }

    public override void Interactuar(IPersonajeJuego p) {

        ILootObjetoPadre personaje = p as ILootObjetoPadre;

        if (personaje != null && personaje.GetObjeto() == this) { // Si el objeto interactuado es el mismo que el objeto que tiene el jugador, no se interactua con el objeto
            return;
        } else {
            if (!personaje.GetObjeto()) {
                SetPadreObjeto(personaje);
                SuscripcionAnimator();
                Debug.Log("Objeto interactuado, soy un objeto");
            } else {
                SetPadreObjeto(personaje);
                personaje.GetObjeto().GetPadreObjeto().SetObjeto(this);
            }
        }
    }

    private void OnTriggerEnter(Collider colision) {
        if (!estaEnCooldown) {
            if (colision.gameObject.GetComponent<IRecibeDaño>() != null) {

                Debug.Log("Colisiono con " + colision.gameObject.name);

                IRecibeDaño recibidorDanio = colision.gameObject.GetComponent<IRecibeDaño>();
                recibidorDanio.RecibirDaño(GetDanio());
                Atacar();
                CoolDownAtaque();
            }
        }
        
    }

    private void OnCollisionEnter(Collision colision) {
        if (!estaEnCooldown) {
            if (colision.gameObject.GetComponent<IRecibeDaño>() != null) {

                Debug.Log("Colisiono con " + colision.gameObject.name);

                // Físicas de impacto
                Rigidbody rigidbodyRecibidorDanio = colision.gameObject.GetComponent<Rigidbody>();
                Vector3 direccionImpacto = colision.contacts[0].point - transform.position;
                direccionImpacto = direccionImpacto.normalized;
                rigidbodyRecibidorDanio.AddForce(-direccionImpacto * 500f);

                IRecibeDaño recibidorDanio = colision.gameObject.GetComponent<IRecibeDaño>();
                recibidorDanio.RecibirDaño(GetDanio());
                Atacar();
                CoolDownAtaque();
            }
        }   
    }

    public void SuscripcionAnimator() {
        IAnimatorUsaObjeto animator = GetComponentInParent<IAnimatorUsaObjeto>();
        if (animator != null) {
            animator.OnUsoObjetoInicia += Animator_OnAtaqueComienza;
            animator.OnUsoObjetoTermina += Animator_OnAtaqueTermina;
        }
    }

    private IEnumerator CoolDownAtaque() {
        Debug.Log("CoolDown");
        yield return new WaitForSeconds(cooldownHitbox);
        estaEnCooldown = false;
    }

    private void Animator_OnAtaqueComienza(object sender, System.EventArgs e) {
        hitbox.enabled = true; // Activamos la hitbox
    }

    private void Animator_OnAtaqueTermina(object sender, System.EventArgs e) {
        hitbox.enabled = false; // Desactivamos la hitbox
    }

    public string GetTipoAtaque() {
        return tipoAtaque;
    }

    public void SetTipoArma(string tipoArma) {
        this.tipoAtaque = tipoArma;
    }

    public float GetDanio() {
        return scriptableArma.danio;
    }

    public LootObjetoArmaSO GetArmaSO() {
        return scriptableArma;
    }

}
