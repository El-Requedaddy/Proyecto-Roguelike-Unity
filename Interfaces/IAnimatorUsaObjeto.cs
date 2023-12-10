using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IAnimatorUsaObjeto {

    public event EventHandler OnUsoObjetoInicia;  // Evento que se dispara cuando el ataque inicia
    public event EventHandler OnUsoObjetoTermina;  // Evento que se dispara cuando el ataque termina

    public void OnAtaqueTerminado();

    public void ComprobarEstadoAtaqueYTerminar();

    public void IniciarAnimacionAtaque(string tipoAtaque);

}
