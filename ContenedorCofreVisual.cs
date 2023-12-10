using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContenedorCofreVisual : MonoBehaviour {

    private const string ABRIR = "Abrir";
    
    [SerializeField] private Contenedor contenedor; 

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        contenedor.OnJugadorAbreCofre += Contenedor_OnJugadorAbreCofre;
    }

    private void Contenedor_OnJugadorAbreCofre(object sender, System.EventArgs e) {
        animator.SetBool(ABRIR, true);
    }

}
