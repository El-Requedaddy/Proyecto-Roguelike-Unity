using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjetos : MonoBehaviour {

    public static PoolObjetos Instance { get; private set; } // Creamos una variable estática de tipo Player para poder acceder a ella desde cualquier script sin necesidad de asignarla en el inspector de Unity

    public GameObject prefab;
    public int cantidadObjetosInicial = 10;
    private List<GameObject> objetos;

    private bool inicializado = false;

    private void Awake() {
        if (Instance != null) { // Si ya hay una instancia de Player, no se crea una nueva y se muestra un mensaje de error en la consola
            Debug.Log("Hay mas de un PoolObjetos, ESPABILA NOTAS!!!");
        }
        Instance = this; // Asignamos la instancia de Player a la variable estática Instance
    }

    private void Start() {
        objetos = new List<GameObject>();
        for (int i = 0; i < cantidadObjetosInicial; i++) {
            GenerarObjeto();
        }
        inicializado = true;
    }

    private void GenerarObjeto() {
        GameObject objetoGenerado = Instantiate(prefab);
        objetoGenerado.SetActive(false);

        Objeto objeto = objetoGenerado.GetComponent<Objeto>();
        objeto.SetPoolObjetos(this);
        Debug.Log("Generando objeto " + objetoGenerado.name);
        Debug.Log("Objeto generado: " + objeto);
        objetos.Add(objetoGenerado);
    }

    public Arma GetArmaDePool() {
        foreach (GameObject objeto in objetos) {
            if (objeto != null && !objeto.activeSelf && objeto.GetComponent<Arma>()) {
                objeto.SetActive(true);
                Arma arma = objeto.GetComponent<Arma>(); // Si el objeto es un arma, lo devolvemos
                if (arma != null) { // Si el objeto es null significa que no es un arma
                    return arma;
                }
            }
        }
        GenerarObjeto();
        return GetArmaDePool();
    }

    public void DevolverObjetoAPool(Objeto objeto) {
        objeto.gameObject.SetActive(false);
    }

    public bool EstaDisponible() {
        return inicializado;
    }

}