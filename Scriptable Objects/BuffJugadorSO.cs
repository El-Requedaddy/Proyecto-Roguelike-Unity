using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public abstract class BuffJugadorSO : ScriptableObject {

    [SerializeField] private float duracionBuff;
    [SerializeField] private ParticleSystem buffParticulas;

    public enum TipoBuff {
        Danio,
        Velocidad,
        Vida,
    }

    private float duracionActual;

    public abstract void ActivarBuff();

    public abstract void DesactivarBuff();

    [SerializeField] private TipoBuff tipo;

    public TipoBuff GetTipoBuff() {
        return tipo;
    }

    public void SetTipoBuff(TipoBuff tipo) {
        this.tipo = tipo;
    }

    public virtual bool estaActivo() {
        if (duracionActual > duracionBuff) {
            return false;
        } else {
            return true;
        }
    }

    public void ActualizarBuff() {
        duracionActual -= Time.deltaTime;
        if (duracionActual <= 0f) {
            DesactivarBuff();
        }
    }

    private void Awake() {
        duracionActual = duracionBuff;
    }

    public void IniciarBuff() {
        duracionActual = duracionBuff;
        ActivarBuff();
    }

    public ParticleSystem GetParticulas() {
        return buffParticulas;
    }

    public float GetDuracionBuff() {
        return duracionBuff;
    }

}