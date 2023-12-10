using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoMeleeIA : EnemigoIA {

    public override Vector2 CalcularDireccionDeseable(Vector2 direccionDeseada) {

        //if (!JugadorEncontrado()) { // Si no hay jugador no hacemos nada
        //    return Vector2.zero;
        //}

        //RellenarMapaPeligros(); // Rellenamos la matriz de peligros y obtenemos el indice de la dirección despues del menor peligro para enmascarar

        Vector2 direccionDeseable = Vector2.zero;
        float mayorPeso = -1f;
        float distancia = Vector2.Distance(transform.position, Player.Instance.transform.position); // Distancia actual entre la IA y el jugador
        float umbralDistancia = GetDistanciaMinima() + 6f;

        if (distancia < GetDistanciaMinima() + 1f) {
            // Realizamos el cálculo para favorecer movimientos laterales alrededor del jugador
            Vector3 direccionOpuesta3D = (transform.position - Player.Instance.transform.position).normalized;
            Vector3 arriba = Vector3.up;

            // Calculamos el producto cruzado entre la dirección opuesta y un vector hacia arriba sacando la direccion perpendicular
            Vector3 productoCruzado = Vector3.Cross(direccionOpuesta3D, arriba);
            Vector2 productoCruzado2D = new Vector2(productoCruzado.x, productoCruzado.z);

            // Asignamos el producto cruzado como la dirección deseable
            direccionDeseable = productoCruzado2D;

        } else {

            // Valores para tratar si solo hay un minimo peligro siendo el unico 

            // Realizamos el cálculo normal
            // Calculamos la dirección deseable
            for (int i = 0; i < arrayPesos.Count; i++) {
                Vector2 direccionMovimiento = arrayDirecciones[i];
                float peso = Vector2.Dot(direccionDeseada.normalized, direccionMovimiento.normalized) + arrayPesos[i]; // Calculamos el peso de la dirección con el producto escalar
                float angulo = Vector2.Angle(direccionDeseada, direccionMovimiento); // Calculamos el angulo entre la dirección deseada y la dirección de movimiento

                Vector3 direccion = transform.position + new Vector3(arrayDirecciones[i].x, 0f, arrayDirecciones[i].y);
                float distanciaAljugador = Vector3.Distance(direccion, Player.Instance.transform.position);

                // Calculamos el castigo en función de la distancia al jugador
                float castigoAjustado = GetCastigo() * funcionSigmoide(umbralDistancia - distanciaAljugador);
                peso *= castigoAjustado;

                float pesoNormalizado = (peso + 1f) / 2f; // Normalizamos el peso para que esté entre 0 y 1
                pesoNormalizado = Mathf.Clamp(pesoNormalizado, 0f, 1f); // Aseguramos que el peso esté entre 0 y 1   

                arrayPesos[i] = pesoNormalizado; // Actualizamos el peso de la matriz de pesos

                if (pesoNormalizado > mayorPeso) {
                    mayorPeso = pesoNormalizado;
                    direccionDeseable = direccionMovimiento;
                }

            }
        }

        if (distancia > 15f) {
            return Vector2.zero;
        } else {
            return direccionDeseable;
        }

    }

}