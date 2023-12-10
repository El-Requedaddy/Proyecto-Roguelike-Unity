using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SistemaVida;

[CreateAssetMenu()]
public class PersonajeSO : ScriptableObject {

    public static event EventHandler<OnVidaBajaArgs> OnVidaBaja;
    public class OnVidaBajaArgs : EventArgs {
        public float porcentajeVida;
    }
    public static event EventHandler<OnVidaSubeArgs> OnVidaSube;
    public class OnVidaSubeArgs : EventArgs {
        public float porcentajeVida;
    }

    [SerializeField] Personaje personaje;
    [SerializeField] int vidaPersonaje;

    [SerializeField] ParticleSystem particulasHabilidad;

    public SistemaVida sistemaVida;

    public void InicializarPersonaje() {
        sistemaVida = new SistemaVida(vidaPersonaje);
        sistemaVida.OnVidaBaja += SistemaVida_OnVidaBaja;
        sistemaVida.OnVidaSube += SistemaVida_OnVidaSube;
    }

    private void SistemaVida_OnVidaSube(object sender, SistemaVida.OnVidaSubeArgs e) {
        OnVidaSube?.Invoke(this, new OnVidaSubeArgs { porcentajeVida = e.porcentajeVida });
        Debug.Log("Vida sube : " + sistemaVida.GetVida());
    }

    private void SistemaVida_OnVidaBaja(object sender, SistemaVida.OnVidaBajaArgs e) {
        OnVidaBaja?.Invoke(this, new OnVidaBajaArgs { porcentajeVida = e.porcentajeVida });
        Debug.Log("Vida baja : " + sistemaVida.GetPorcentajeVida());
    }

    public enum Personaje {
        Mago,
        Caballero,
        Rogue,
        Barbaro
    }

}