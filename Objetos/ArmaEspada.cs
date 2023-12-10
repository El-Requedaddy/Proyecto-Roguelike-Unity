using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaEspada : Arma {

    private const string TIPO_ARMA = "AtaqueEspada"; // TODO: sacar a un enum

    private void Awake() {
        SetTipoArma(TIPO_ARMA); // Inicializamos el tipo de arma
    }

    public override void Atacar() { // Funcion que se ejecuta cuando se ataca con la espada
        Debug.Log("Atacando con espada");
    }

}