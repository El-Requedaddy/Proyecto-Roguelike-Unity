using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionPersonajeUI : MonoBehaviour {

    public static SeleccionPersonajeUI Instance { get; private set; }

    [SerializeField] private Button botonDerecha;
    [SerializeField] private Button botonIzquierda;
    [SerializeField] private Button botonSeleccionar;

    [SerializeField] private TextMeshProUGUI textoNombrePersonaje;
    [SerializeField] private TextMeshProUGUI textoDescripcionPersonaje;

    [SerializeField] private Button botonDificultad;
    [SerializeField] private Text textoDificultad;

    private string dificultad = "Normal";

    private void Start() {
        Ocultar();
    }

    private void Awake() {
        botonSeleccionar.Select();

        Instance = this;

        botonDerecha.onClick.AddListener(() => {
            int indice = SeleccionPersonaje.Instance.SiguientePersonaje();
            CambiarTextoPersonaje(indice);
        });
        botonIzquierda.onClick.AddListener(() => {
            int indice = SeleccionPersonaje.Instance.AnteriorPersonaje();
            CambiarTextoPersonaje(indice);
        });

        botonSeleccionar.onClick.AddListener(() => {
            PlayerPrefs.SetInt("PersonajeElegido", SeleccionPersonaje.Instance.GetPersonajeElegido());
            PlayerPrefs.SetString("Dificultad", dificultad);
            Cargador.CargarEscena(Cargador.Escena.GameScene);
            //SeleccionPersonaje.Instance.SeleccionarPersonaje();
        });

        botonDificultad.onClick.AddListener(() => {
            CambiarTextoDificultad();
        });
    }

    private void CambiarTextoDificultad() {
        if (dificultad == "Normal") {
            dificultad = "Dificil";
        } else {
            dificultad = "Normal";
        }
        textoDificultad.text = dificultad;
    }

    private void CambiarTextoPersonaje(int indice) {
        switch (indice) {
            case 0:
                textoNombrePersonaje.text = "MAGO";
                textoDescripcionPersonaje.text = "El mago es un personaje que se especializa en el uso de magia. Es un personaje que se puede usar tanto para atacar como para devastar con magia.";
                break;
            case 1:
                textoNombrePersonaje.text = "CABALLERO";
                textoDescripcionPersonaje.text = "El caballero es un personaje que se especializa en el uso de armas. Es un personaje que se puede usar tanto para atacar como para defender.";
                break;
            case 2:
                textoNombrePersonaje.text = "ROGUE";
                textoDescripcionPersonaje.text = "El togue es un personaje que se especializa en el uso de invisibilidad. Es un personaje que se puede usar tanto para atacar como para defender.";
                break;
            case 3:
                textoNombrePersonaje.text = "BARBARO";
                textoDescripcionPersonaje.text = "El barbaro es un personaje que se especializa en el uso de fuerza. Es un personaje que se puede usar tanto para atacar como para defender.";
                break;
            default:
                break;
        }

    }

    public void Mostrar() {
        gameObject.SetActive(true);
    }

    public void Ocultar() {
        gameObject.SetActive(false);
    }

}