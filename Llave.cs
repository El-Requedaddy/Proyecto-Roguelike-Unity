using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoLlave {
    LlaveNormal,
    LlaveSalida,
}

public class Llave : Objeto {

    public override void Interactuar(IPersonajeJuego personaje) {
        Player player = personaje as Player;
        if (player != null) {
            player.AnadirLlave(this);
            gameObject.SetActive(false);
        }
    }

    [SerializeField] public TipoLlave tipoLlave;

    public TipoLlave GetTipoLlave() {
        return tipoLlave;
    }

}