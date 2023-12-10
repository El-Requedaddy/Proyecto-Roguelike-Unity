using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootObjetoPadre { 

    public Transform GetLootObjetoSeguirTransformada(); // Devuelve la transformada padre de un objeto que se puede lootear

    public void SetObjeto(Objeto objeto); // Setea un objeto que se puede lootear

    public Objeto GetObjeto(); // Devuelve el objeto que se puede lootear

    public void CleanObjeto(); // Limpia el objeto que se puede lootear

    public bool TieneObjeto(); // Devuelve si tiene un objeto que se puede lootear

}
