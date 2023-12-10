using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargadorCallback : MonoBehaviour {

    private bool esPrimerUpdateFrame = true; // Para que el cargador no se ejecute en el primer frame de la escena dado que se necesita un frame para poder mostrar la escena

    private void Update() {
        if (esPrimerUpdateFrame) {
            esPrimerUpdateFrame = false;

            Cargador.CargadorCallback();
        }
    }

}
