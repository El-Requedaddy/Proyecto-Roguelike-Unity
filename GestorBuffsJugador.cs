using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorBuffsJugador : MonoBehaviour {

    public static event EventHandler onBuffComienza;

    public static event EventHandler onBuffTermina;

    private List<BuffJugadorSO> buffsActivos;

    private void Awake() {
        buffsActivos = new List<BuffJugadorSO>();
    }

    public void AnadirBuff(BuffJugadorSO buff) { // Añadimos un buff a la lista de buffs activos y lo activamos
        buffsActivos.Add(buff);
        buff.ActivarBuff();
        Debug.Log("Buff añadido");
        Debug.Log("Buffs activos: " + buffsActivos.Count);
    }

    private void Update() {
        if (buffsActivos.Count == 0) return; // Si no hay buffs activos no hacemos nada
        foreach (BuffJugadorSO buff in buffsActivos) { // Recorremos la lista de buffs activos y actualizamos cada uno y eliminamos en caso de estar inactivo
            buff.ActualizarBuff();
        }
    }
    
    public void EliminarBuff(BuffJugadorSO buff) {
        buffsActivos.Remove(buff);
        buff.DesactivarBuff();
    }



}