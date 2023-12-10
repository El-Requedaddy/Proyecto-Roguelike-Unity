using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirarACamara : MonoBehaviour {

    private enum Modo {
        MirarACamara,
        Invertido,
        CamaraDelante,
        CamaraDelanteInvertido
    }

    [SerializeField] private Modo modo;

    private void LateUpdate() {
        switch (modo) {
            case Modo.MirarACamara:
                transform.LookAt(Camera.main.transform);
                break;
            case Modo.Invertido:
                Vector3 distanciaACamara = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + distanciaACamara);
                break;
            case Modo.CamaraDelante:
                transform.forward = Camera.main.transform.forward;
                break;
            case Modo.CamaraDelanteInvertido:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }

}