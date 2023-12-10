using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObtieneObjetoDePool {

    public void SacarObjetoDePool();

    public void DevolverObjetoAPool();

    public IEnumerator EsperarInicializacionPool();

}
