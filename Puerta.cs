using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour, IElementoInteraccionable {

    public event EventHandler OnJugadorAbrePuerta;
    [SerializeField] private TipoLlave tipoLlavePuerta;
    [SerializeField] private BoxCollider puertaSobrante;

    public void Interactuar(IPersonajeJuego personaje) {
        Player player = personaje as Player;
        if (player != null) {
            if (player.UsarLlave(tipoLlavePuerta)) {
                puertaSobrante.enabled = false;
                OnJugadorAbrePuerta?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
