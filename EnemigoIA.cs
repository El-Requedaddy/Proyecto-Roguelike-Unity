using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class EnemigoIA : MonoBehaviour {

    public abstract Vector2 CalcularDireccionDeseable(Vector2 direccionDeseada);

    protected List<float> arrayPesos; // Matriz de pesos que se usará para calcular la dirección deseable
    protected List<Vector2> arrayDirecciones; // Matriz de direcciones que se usará para calcular la dirección deseable
    protected List<float> listaPeligros; // Lista de peligros que se usará para calcular la dirección deseable
    int divisiones = 20;

    [SerializeField] private LayerMask entornoLayerMask;
    [SerializeField] private LayerMask layerJugador;

    [SerializeField] private float castigo;
    [SerializeField] private float incentivo;

    private float menorPeligroValor;

    [SerializeField] private float distanciaMinima = 5f;

    private bool modoChase = false;

    int layerMaskCast;

    private void Awake() {
        InicializarMatrizPesosYMovimientos();
        layerMaskCast = (1 << entornoLayerMask) | (1 << layerJugador); // Creamos una máscara con las capas que queremos mirar para el detectarJugador
        //Debug.Log("La máscara es: " + entornoLayerMask.ToString());
        layerJugador.ToString();
    }

    private void Start() {
        
    }

    public Vector3 ObtenerDireccionMovimiento() {

        if (Player.Instance != null) {
            Vector3 direccionDeseada = Player.Instance.transform.position - transform.position;
            //Debug.Log("La direccion deseada es: " + direccionDeseada);
            Vector2 direccionDeseadaV = new Vector2(direccionDeseada.x, direccionDeseada.z);
            Vector2 direccionDeseable = CalcularDireccionDeseable(direccionDeseadaV);

            // Cambiamos el forward del enemigo para que mire hacia la dirección deseada ademas de convertir a Vector3 para que no se mueva en el eje Y
            Vector3 direccionFinal = new Vector3(direccionDeseable.x, 0f, direccionDeseable.y);
            direccionFinal.z = direccionDeseable.y;

            return direccionFinal;
        }  

        return Vector3.zero;

    }

    protected void InicializarMatrizPesosYMovimientos() {
        float angulo = 0f;
        float incrementoAngulo = (2 * Mathf.PI) / divisiones; // 18 grados en radianes
        float valorPesoBase = 0.5f; // Valor base del peso que se le asignará a cada dirección

        arrayPesos = new List<float>(divisiones); // Inicializar la lista de pesos con el tamaño deseado
        arrayDirecciones = new List<Vector2>(divisiones); // Inicializar la lista de direcciones con el tamaño deseado

        for (int i = 0; i < divisiones; i++) {
            angulo += incrementoAngulo; // Incrementamos el ángulo en 18 grados
            arrayDirecciones.Add(new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo))); // Agregamos la dirección a la lista de direcciones
            arrayPesos.Add(valorPesoBase); // Agregamos el peso a la lista de pesos
        }
    }

    protected bool JugadorEncontrado() {
        Vector3 direccion = Player.Instance.transform.position - transform.position;
        float distancia = direccion.magnitude;

        RaycastHit hit;
        if (Physics.Linecast(transform.position, Player.Instance.transform.position, out hit, layerMaskCast)) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                Debug.Log("Encontrado al jugador");
                return true;
            } else {
                Debug.Log("Colisión con objeto: " + hit.collider.name);
            }
        } else {
            Debug.Log("No se ha detectado al jugador");
        }

        return false;
    }

    protected int RellenarMapaPeligros() {
        listaPeligros = new List<float>(arrayPesos.Count); // Inicializar la lista de peligros con el tamaño deseado
        float distancia; // Distancia actual entre la IA y el peligro
        float distanciaMinimaLocal = 3f; // Distancia mínima a la que se considera que la IA está en peligro
        float minimoValorPeligro = Mathf.Infinity; // Valor mínimo de peligro
        bool sePuedeMover; // Variable que indica si la IA se puede mover en una dirección
        float radioPlayer = .7f;  // Radio del enemigo
        float alturaPlayer = 2f;  // Altura del enemigo
        int indiceDevolver = 0; // Indice del valor de peligro a devolver
        bool haSacadoPuntoDeRecorrido = false; // Variable que indica si se ha sacado un punto de recorrido desde el que se verificara direccion a tomar

        for (int i = 0; i < arrayPesos.Count; i++) { // Recorrer la matriz de pesos
            Vector3 direccion = transform.position + new Vector3(arrayDirecciones[i].x, 0f, arrayDirecciones[i].y);
            sePuedeMover = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * alturaPlayer, radioPlayer, direccion, distanciaMinimaLocal, entornoLayerMask);
            distancia = Vector3.Distance(transform.position, Player.Instance.transform.position);

            if (!sePuedeMover) {
                listaPeligros.Add(distancia / distanciaMinimaLocal);
                if (listaPeligros[i] < minimoValorPeligro) {
                    minimoValorPeligro = listaPeligros[i];
                } else {
                    if (!haSacadoPuntoDeRecorrido) {
                        haSacadoPuntoDeRecorrido = true;
                        indiceDevolver = i - 1;
                    }      
                }
            } else {
                listaPeligros.Add(0f);
                minimoValorPeligro = 0f;
            }
        }

        return indiceDevolver;
    }
    
    private void OnDrawGizmos() { // Dibujar las líneas de la matriz de pesos parar saber como se distribuye el interés de la IA

        float intensidad = 1.5f; // Intensidad de la línea

        if (arrayPesos != null && arrayDirecciones != null) {
            for (int i = 0; i < arrayPesos.Count; i++) {
                // Interpolar el color entre rojo y verde según el peso
                float peso = arrayPesos[i];
                Color color = Color.Lerp(Color.red, Color.green, peso) * intensidad; // Interpolar el color entre rojo y verde según el peso

                // Calcular el punto final de la línea según la dirección y el peso
                Vector3 inicio = transform.position; // El punto de inicio es la posición del objeto
                Vector3 direccion = new Vector3(arrayDirecciones[i].x, 0f, arrayDirecciones[i].y); // Convertir a Vector3 para que no se mueva en el eje Y
                Vector3 fin = inicio + direccion * (1 - peso); // El punto final es la posición del objeto + la dirección * el peso

                // Dibujar la línea con el color interpolado
                Gizmos.color = color;
                Gizmos.DrawLine(inicio, fin);
            }
        }
            
    }

    protected float funcionSigmoide(float x) {
        return 1 / (1 + Mathf.Exp(-x)); // Función sigmoide para calcular el peso de la dirección deseada
    }

    public float GetValorMenorPeligro() {
        return menorPeligroValor;
    }

    public float GetIncentivo() {
        return incentivo;
    }

    public float GetCastigo() {
        return castigo;
    }

    public float GetDistanciaMinima() {
        return distanciaMinima;
    }

}