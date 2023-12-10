using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameOverUI : MonoBehaviour {

    private void Start() {
        if (PartidaManager.Instance == null) {
            Debug.LogError("No hay ninguna instancia de PartidaManager en la escena");
            return;
        }
        PartidaManager.Instance.OnEstadoCambia += PartidaManager_OnEstadoCambia;
        Ocultar();
    }

    private void PartidaManager_OnEstadoCambia(object sender, EventArgs e) {
        if (PartidaManager.Instance.EstaEnGameOver()) {
            Mostrar();
        } else {
            Ocultar();
        }
    }

    private void Mostrar() {
        gameObject.SetActive(true);
    }

    private void Ocultar() {
        gameObject.SetActive(false);
    }

}