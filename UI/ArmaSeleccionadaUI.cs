using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArmaSeleccionadaUI : MonoBehaviour {

    [SerializeField] private Arma armaNueva; // Este arma será la nueva arma
    private Arma armaActual; // Este arma será la actual arma del jugador

    [SerializeField] private TextMeshProUGUI textoArmaOriginal;
    [SerializeField] private TextMeshProUGUI textoArmaNueva;
    [SerializeField] private Transform icono;

    private void Start () {
        Player.Instance.OnObjetoSeleccionadoCambia += Player_OnObjetoSeleccionadoCambia;
    }

    private void Player_OnObjetoSeleccionadoCambia(object sender, Player.OnObjetoSeleccionadoCambiaEventArgs e) {
        
        if (Player.Instance.TieneObjeto()) {
            Arma armaComprobacion = Player.Instance.GetObjeto() as Arma; // Cogemos el arma actual del jugador y la guardamos en la variable
            if (armaComprobacion == null) { // Aseguramos que el objeto sea un arma
                return;
            }
        }

        armaActual = Player.Instance.GetObjeto() as Arma; // Cogemos el arma actual del jugador y la guardamos en la variable
        if (e.objetoSeleccionado == (IElementoInteraccionable)armaNueva) { // Si el objeto seleccionado es el arma nueva, es decir, esta, se activa el icono
            icono.gameObject.SetActive(true); // Activamos el objeto visual
            ActualizarDatos();
        } else {
            if (armaNueva != null) { // Aseguramos que no sea null
                icono.gameObject.SetActive(false); // Activamos el objeto visual
                armaActual = null;
            } else {

            }
        }
    }

    private void Awake() {
        icono.gameObject.SetActive(false);
    }

    private void ActualizarDatos() {

        if (Player.Instance.GetObjeto() == null) {
            textoArmaOriginal.text = 0.ToString(); // Cogemos el daño del arma actual
            textoArmaNueva.color = Color.green;
        } else {
            textoArmaOriginal.text = armaActual.GetArmaSO().danio.ToString(); // Cogemos el daño del arma actual
            if (armaNueva.GetArmaSO().danio > armaActual.GetArmaSO().danio) {
                textoArmaNueva.color = Color.green;
            } else {
                textoArmaNueva.color = Color.red;
            }
        }

        textoArmaNueva.text = armaNueva.GetArmaSO().danio.ToString(); // Cogemos el daño del arma nueva

    }

}