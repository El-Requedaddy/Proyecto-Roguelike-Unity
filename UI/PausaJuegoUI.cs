using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausaJuegoUI : MonoBehaviour {

    [SerializeField] private Button reanudarButton;
    [SerializeField] private Button menuPrincipalButton;
    [SerializeField] private Button opcionesButton;

    private void Awake() {
        reanudarButton.onClick.AddListener(() => {
            PartidaManager.Instance.GestionPausa();
        });
        menuPrincipalButton.onClick.AddListener(() => {
            Cargador.CargarEscena(Cargador.Escena.MenuPrincipalScene);
        });
        opcionesButton.onClick.AddListener(() => {
            Ocultar();
            OpcionesUI.Instance.Mostrar(Mostrar); // Mostrar es el callback que se ejecuta cuando se cierra la ventana de opciones de modo que podemos volver a mostrar la ventana de pausa
        });
    }

    private void Start () {
        PartidaManager.Instance.OnJuegoPausado += PartidaManager_OnJuegoPausado;
        PartidaManager.Instance.OnJuegoReanudado += PartidaManager_OnJuegoReanudado;
        Ocultar();
    }

    private void PartidaManager_OnJuegoReanudado(object sender, EventArgs e) {
        Ocultar();
    }

    private void PartidaManager_OnJuegoPausado(object sender, EventArgs e) {
        Mostrar(); 
    }

    private void Mostrar() {
        gameObject.SetActive(true);
        reanudarButton.Select();
    }

    private void Ocultar() {
        gameObject.SetActive(false);
    }

}
