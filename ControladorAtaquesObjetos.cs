using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorAtaquesObjetos : MonoBehaviour {

    [SerializeField] private GameInput gameInput;

    private const string ATAQUE_ESPADA = "AtaqueEspada";
    private const string ATAQUE_ARCO = "AtaqueArco";
    private const string ATAQUE_HACHA = "AtaqueHacha";

    private bool triggerAtaque = false;

    private void Start() {
        gameInput.OnUsoObjeto += GameInput_OnAtaque;
    }

    public string DeterminarTipoAtaque(Arma arma) {

        if (arma.GetTipoAtaque() == "Espada") {
            return ATAQUE_ESPADA;
        }

        if (arma.GetTipoAtaque() == "Hacha") {
            return ATAQUE_HACHA;
        }

        return null;

    }

    private void GameInput_OnAtaque(object sender, System.EventArgs e) {
        if (Player.Instance.TieneObjeto()) {
            Arma espada = Player.Instance.GetObjeto() as Arma;
            if (espada != null) {
                string tipoAtaque = DeterminarTipoAtaque(espada);
                ComenzarAtaque(tipoAtaque);
            }
        }
    }

    public void ComenzarAtaque(string tipoAtaque) {
        if (tipoAtaque != null) {
            PlayerAnimator.Instance.IniciarAnimacionAtaque(tipoAtaque);
        }
    }


}
