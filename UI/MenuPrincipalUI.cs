using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalUI : MonoBehaviour {

    public static MenuPrincipalUI Instance { get; private set; }

    [SerializeField] private Button botonJugar;
    [SerializeField] private Button botonSalir;

    private void Awake() {
        Instance = this;

        botonJugar.Select();

        botonJugar.onClick.AddListener( () => {
            //Cargador.CargarEscena(Cargador.Escena.GameScene);
            SeleccionPersonajeUI.Instance.Mostrar();
            SeleccionPersonaje.Instance.CentrarPrimerPersonaje();
            Ocultar();
        });

        botonSalir.onClick.AddListener(() => {
            Application.Quit();
        });

        Time.timeScale = 1.0f; // Reactivamos el tiempo para que no se quede pausado el juego
    }

    public void Mostrar() {
        gameObject.SetActive(true);
    }

    public void Ocultar() {
        gameObject.SetActive(false);
    }


}
