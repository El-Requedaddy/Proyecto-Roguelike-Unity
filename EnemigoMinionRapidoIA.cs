using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoMinionRapidoIA : EnemigoIA {

    public override Vector2 CalcularDireccionDeseable(Vector2 direccionDeseada) {

        //if (!JugadorEncontrado()) { // Si no hay jugador no hacemos nada
        //    return Vector2.zero;
        //}

        Vector2 direccionDeseable = Vector2.zero;
        float mayorPeso = -1f;
        float distancia = Vector2.Distance(transform.position, Player.Instance.transform.position); // Distancia actual entre la IA y el jugador
        float umbralDistancia = GetDistanciaMinima() + 6f;

        for (int i = 0; i < arrayPesos.Count; i++) {
            Vector2 direccionMovimiento = arrayDirecciones[i];
            float peso = Vector2.Dot(direccionDeseada.normalized, direccionMovimiento.normalized); // Calculamos el peso de la dirección con el producto escalar

            if (peso > mayorPeso) {
                mayorPeso = peso;
                direccionDeseable = direccionMovimiento;
            }
        }

        if (distancia > 15f) {
            return Vector2.zero;
        } else {
            //if (direccionDeseable == Vector2.zero) {
            //    Vector3 direccionOpuesta3D = (transform.position - Player.Instance.transform.position).normalized;
            //    Vector3 arriba = Vector3.up;

            //    // Calculamos el producto cruzado entre la dirección opuesta y un vector hacia arriba sacando la direccion perpendicular
            //    Vector3 productoCruzado = Vector3.Cross(direccionOpuesta3D, arriba);
            //    Vector2 productoCruzado2D = new Vector2(productoCruzado.x, productoCruzado.z);

            //    // Asignamos el producto cruzado como la dirección deseable
            //    direccionDeseable = productoCruzado2D;
            //    return direccionDeseable;
            //}
            //return direccionDeseable;
        }


        return direccionDeseable;

    }

}