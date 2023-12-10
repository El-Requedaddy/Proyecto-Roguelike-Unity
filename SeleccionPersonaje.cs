using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionPersonaje : MonoBehaviour {

    public static SeleccionPersonaje Instance { get; private set; }

    [SerializeField] private Transform[] personajes;

    private int personajeActual = 0;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private float zoom = 25f;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
    }
    
    public void CentrarPrimerPersonaje() { // Centra la camara en el primer personaje
        virtualCamera.LookAt = personajes[0];
        virtualCamera.m_Lens.FieldOfView = zoom;
    }

    private void MirarAPersonaje(Transform personaje) { // Centra la camara en el personaje que le pasemos
        virtualCamera.LookAt = personaje;
    }

    public int SiguientePersonaje() { // Cambia al siguiente personaje
        personajeActual++;
        if (personajeActual >= personajes.Length) {
            personajeActual = 0;
        }
        MirarAPersonaje(personajes[personajeActual]);

        return personajeActual;
    }

    public int AnteriorPersonaje() { // Cambia al anterior personaje
        personajeActual--;
        if (personajeActual < 0) {
            personajeActual = personajes.Length - 1;
        }
        MirarAPersonaje(personajes[personajeActual]);

        return personajeActual;
    }

    public void Ocultar() { // Oculta el menu de seleccion de personaje
        float zoomDefault = 60f;

        gameObject.SetActive(false);
        virtualCamera.m_Lens.FieldOfView = zoomDefault; // Devolvemos el zoom a su valor por defecto
    }

    public void Mostrar() {
        gameObject.SetActive(true);
    }

    public int GetPersonajeElegido() { // Devuelve el personaje elegido
        return personajeActual;
    }

}
