using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VIdaPersonajeUI : MonoBehaviour {

    [SerializeField] private Image corazon1;
    [SerializeField] private Image corazon2;
    [SerializeField] private Image corazon3;
    [SerializeField] private Image corazon4;
    [SerializeField] private Image corazon5;

    [SerializeField] private Image corazonNegro1;
    [SerializeField] private Image corazonNegro2;
    [SerializeField] private Image corazonNegro3;
    [SerializeField] private Image corazonNegro4;
    [SerializeField] private Image corazonNegro5;


    private void Start() { 
        Debug.Log("Suscribiendo a eventos de PersonajeSO");
        PersonajeSO.OnVidaBaja += PersonajeSO_OnVidaBaja;
        PersonajeSO.OnVidaSube += PersonajeSO_OnVidaSube;
        Debug.Log("Eventos de PersonajeSO suscritos correctamente");
        PartidaManager.Instance.OnJuegoPausado += Instance_OnJuegoPausado;
        PartidaManager.Instance.OnJuegoReanudado += Instance_OnJuegoReanudado;
    }

    private void Instance_OnJuegoReanudado(object sender, EventArgs e) {
        Mostrar();
    }

    private void Instance_OnJuegoPausado(object sender, EventArgs e) {
        Ocultar();
    }

    private void PersonajeSO_OnVidaSube(object sender, PersonajeSO.OnVidaSubeArgs e) {
        ActualizarCorazones(e.porcentajeVida);
    }

    private void PersonajeSO_OnVidaBaja(object sender, PersonajeSO.OnVidaBajaArgs e) {
        ActualizarCorazones(e.porcentajeVida);
        Debug.Log("Vida baja : " + e.porcentajeVida);
    }

    // No es efectivo tener dos eventos para subir y bajar la vida, ya que se puede hacer con un solo evento. Sin embargo, lo hago para poder separar el sistema de vida del personaje del sistema de vida de los enemigos


    private void ActualizarCorazones(float porcentajeVida) {
        // Muestra el primer corazón si el porcentaje de vida es mayor o igual al 20% y menor al 40%
        corazon1.enabled = porcentajeVida > 0f;
        // Muestra el segundo corazón si el porcentaje de vida es mayor o igual al 40% y menor al 60%
        corazon2.enabled = porcentajeVida > 0.2f;
        // Muestra el tercer corazón si el porcentaje de vida es mayor o igual al 60% y menor al 80%
        corazon3.enabled = porcentajeVida > 0.4f;
        // Muestra el cuarto corazón si el porcentaje de vida es mayor o igual al 80% y menor al 100%
        corazon4.enabled = porcentajeVida > 0.6f;
        // Muestra el quinto corazón si el porcentaje de vida es igual al 100%
        corazon5.enabled = porcentajeVida > 0.8f;
    }

    private void Mostrar() {
        gameObject.SetActive(true);
    }

    private void Ocultar() {
        gameObject.SetActive(false);
    }
}
