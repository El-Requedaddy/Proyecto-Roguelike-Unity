using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaVisual : MonoBehaviour {

    private const string ABRIR = "Abrir";

    [SerializeField] private Puerta puerta;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        puerta.OnJugadorAbrePuerta += Puerta_OnJugadorAbrePuerta;
    }

    private void Puerta_OnJugadorAbrePuerta(object sender, EventArgs e) {
        animator.SetBool(ABRIR, true);
    }

}
