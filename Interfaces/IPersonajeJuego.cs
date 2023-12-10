using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersonajeJuego {

    public void SetearComoPadre(Objeto objeto);

    public void ImponerCoolDown(float cooldown);

    public void SetEstaAtacando(bool a);

    public bool EstaAtacando();

    public bool EstaMuerto();

}
