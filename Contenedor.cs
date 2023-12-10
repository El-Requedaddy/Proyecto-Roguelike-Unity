using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contenedor : MonoBehaviour, ILootObjetoPadre, IElementoInteraccionable, IObtieneObjetoDePool {

    public event EventHandler OnJugadorAbreCofre;

    private bool abierto = false;

    [SerializeField] private PoolObjetos poolObjetos; // Pool de objetos del arma
    private LootObjetoSO objetoPrefab;
    [SerializeField] private Transform spawnObjeto; // Punto de spawn del objeto

    private Objeto objeto; // Instancia del objeto que posee el cofre

    private void Start () {
        poolObjetos = PoolObjetos.Instance;
    }

    public IEnumerator EsperarInicializacionPool() {
        while (poolObjetos == null) {
            yield return null;
        }

        SacarObjetoDePool();
    }

    public void Interactuar(IPersonajeJuego player) {
        if (!abierto) {
            Debug.Log("Contenedor interactuado, soy un contenedor");
            SacarObjetoDePool();

            OnJugadorAbreCofre?.Invoke(this, EventArgs.Empty); // Invocamos el evento

            abierto = true;
        } else {

            if (player as Player) { // Si el jugador es el que interactua con el contenedor y el contenedor tiene un objeto, el jugador coge el objeto

                ILootObjetoPadre personaje = player as ILootObjetoPadre; // Convertimos el jugador en un ILootObjetoPadre para poder acceder a sus metodos

                if (personaje.TieneObjeto() && !TieneObjeto()) { // Si el jugador tiene un objeto y el contenedor no tiene un objeto, el jugador le da el objeto al contenedor
                    personaje.GetObjeto().SetPadreObjeto(this);
                } else if (personaje.TieneObjeto() && TieneObjeto()) { // Si el jugador tiene un objeto y el contenedor tiene un objeto, el jugador intercambia el objeto con el contenedor
                    personaje.GetObjeto().SetPadreObjeto(this);
                    GetObjeto().SetPadreObjeto(personaje);
                }
            }

            return;
        }
        
    }

    // Interfaz IObtieneObjetoDePool
    public void SacarObjetoDePool() { // Sacamos el objeto del pool
        objeto = poolObjetos.GetArmaDePool();
        objeto.SetPadreObjeto(this);
        objetoPrefab = objeto.GetLootObjetoSO();
    }

    public void DevolverObjetoAPool() { // Devolvemos el objeto al pool
        poolObjetos.DevolverObjetoAPool(objeto as Arma);
        objeto = null;
    }

    // Interfaz ILootObjetoPadre
    public Transform GetLootObjetoSeguirTransformada() {
        return spawnObjeto;
    }

    public void SetObjeto(Objeto objeto) {
        this.objeto = objeto;
    }

    public Objeto GetObjeto() {
        return objeto;
    }

    public void CleanObjeto() {
        objeto = null;
    }

    public bool TieneObjeto() {
        return objeto != null;
    }



}
