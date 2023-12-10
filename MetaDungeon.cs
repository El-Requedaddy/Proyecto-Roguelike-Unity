using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaDungeon : MonoBehaviour {

    public static event EventHandler OnJugadorEntraEnMeta; // Evento que se lanza cuando el jugador entra en la meta

    [SerializeField] ParticleSystem particulasPortalSalida;

    private void Start() {
        ParticleSystem instancia;
        instancia = Instantiate(particulasPortalSalida, transform);
        instancia.Play();
    }

    private void OnTriggerEnter(Collider other) { // Cuando el jugador entra en la meta
        Debug.Log("El jugador ha entrado en la meta");
        if (other.CompareTag("Player")) {
            Player.Instance.gameObject.SetActive(false);
        }
    }

}
