using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : Objeto {

    private GestorBuffsJugador buffsJugador;
    private BuffJugadorSO buff;

    public override void Interactuar(IPersonajeJuego personaje) {

        if (personaje as Player) { // Aseguramos que el personaje que interactua es el jugador
            Player p = personaje as Player;
            if (!p.GetObjeto()) {
                SetPadreObjeto(p);
                Debug.Log("Objeto interactuado, soy una pocion");
            } else {
                SetPadreObjeto(p);
                p.GetObjeto().GetPadreObjeto().SetObjeto(this);
            }
            buffsJugador = GetComponentInParent<GestorBuffsJugador>(); // Obtenemos el componente GestorBuffsJugador del padre

            if (GetLootObjetoSO() as LootObjetoPocionSO) {
                // Si el objeto que se puede lootear es una pocion, obtenemos el buff de la pocion
                buff = (GetLootObjetoSO() as LootObjetoPocionSO).buff;
            }
        }

    }

    private void OnDestroy() {
        GameInput.Instance.OnUsoObjeto -= GameInput_OnUsoObjeto;
    }

    private void Start() {
        GameInput.Instance.OnUsoObjeto += GameInput_OnUsoObjeto;
    }

    private void GameInput_OnUsoObjeto(object sender, EventArgs e) {
        if (GetPadreObjeto() != null && buffsJugador != null) {
            buffsJugador.AnadirBuff(buff); // Añadimos el buff al jugador
            GetPadreObjeto().CleanObjeto(); // Quitamos el padre del objeto
            Destroy(gameObject);
        }
    }
}
