using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadreObjetoSuelo : MonoBehaviour, ILootObjetoPadre, IElementoInteraccionable {

    [SerializeField] private Objeto objeto;

    private void Awake() {
        objeto.SetPadreObjeto(this);
    }

    public Transform GetLootObjetoSeguirTransformada() {
        return transform;
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

    public virtual void Interactuar(IPersonajeJuego player) {
        if (player as Player) { // Si el jugador es el que interactua con el contenedor y el contenedor tiene un objeto, el jugador coge el objeto
            ILootObjetoPadre personaje = player as ILootObjetoPadre; // Convertimos el jugador en un ILootObjetoPadre para poder acceder a sus metodos

            if (personaje.TieneObjeto() && !TieneObjeto()) { // Si el jugador tiene un objeto y el contenedor no tiene un objeto, el jugador le da el objeto al contenedor
                personaje.GetObjeto().SetPadreObjeto(this);
            } else if (personaje.TieneObjeto() && TieneObjeto()) { // Si el jugador tiene un objeto y el contenedor tiene un objeto, el jugador intercambia el objeto con el contenedor
                personaje.GetObjeto().SetPadreObjeto(this);
                GetObjeto().SetPadreObjeto(personaje);
            }
        }
    }

}