using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarObjeto : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 20, 0); // Ajusta los valores para controlar la velocidad de rotación en cada eje

    void Update() {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
