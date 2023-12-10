using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SistemaVida {
    //Esta clase no hace falta ni explicarla

    public event EventHandler<OnVidaBajaArgs> OnVidaBaja; // Evento que se dispara cuando se cambia la vida
    public class OnVidaBajaArgs : EventArgs {
        public float porcentajeVida;
    }

    public event EventHandler<OnVidaSubeArgs> OnVidaSube; // Evento que se dispara cuando se cambia la vida
    public class OnVidaSubeArgs : EventArgs {
        public float porcentajeVida;
    }

    private int vida;
    private int vidaMaxima;

    public SistemaVida(int vidaMaxima) {
        this.vidaMaxima = vidaMaxima;
        vida = vidaMaxima;
    }

    public int GetVida() { 
        return vida;
    }

    public void RecibirDanio(int vida) {
        this.vida -= vida;
        if (this.vida < 0) {
            this.vida = 0;
        }
        OnVidaBaja?.Invoke(this, new OnVidaBajaArgs { porcentajeVida = GetPorcentajeVida() });
    }

    public void RecibirVida(int vida) {
        this.vida += vida;
        if (this.vida > vidaMaxima) {
            this.vida = vidaMaxima;
        }
        OnVidaSube?.Invoke(this, new OnVidaSubeArgs { porcentajeVida = GetPorcentajeVida() });
    }

    public float GetPorcentajeVida() {
        return (float)vida / vidaMaxima;
    }

}
