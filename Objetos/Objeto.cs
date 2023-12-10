using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objeto : MonoBehaviour, IElementoInteraccionable {

    private ILootObjetoPadre padreObjeto;

    private const string layerBase = "Objetos";
    private const string layerObjetoEnPosesion = "ObjetoNoInteractuable";

    [SerializeField] protected LootObjetoSO objetoPrefab;

    protected PoolObjetos poolObjetos; // Pool de objetos 

    public virtual void Interactuar(IPersonajeJuego personaje) {
        Debug.Log("Aquí no deberías interactuar");
    }

    public LootObjetoSO GetLootObjetoSO() {
        return objetoPrefab;
    }

    public void SetPadreObjeto(ILootObjetoPadre padreObjeto) { // Seteamos el padre del objeto y lo colocamos en el punto de spawn del padre del objeto (el contenedor)
        if (this.padreObjeto != null && padreObjeto.TieneObjeto()) { // En resumidas cuentas intercambiamos objetos del jugador y el contenedor
            Objeto aux = this.padreObjeto.GetObjeto(); 
            this.padreObjeto.CleanObjeto();
            padreObjeto.GetObjeto().SetPadreObjeto(this.padreObjeto); // Si el padre del objeto tiene un objeto, lo cambiamos por el objeto que se le va a hacer padre
        }

        this.padreObjeto = padreObjeto; // Seteamos el padre del objeto

        padreObjeto.SetObjeto(this); // Seteamos el objeto en el padre del objeto

        if (!AsignarPadre(padreObjeto as IPersonajeJuego)) { // Si el padre del objeto no es el jugador o enemigo, cambiamos la layer del objeto a la layer base
            CambiarLayer(layerBase); // Cambiamos la layer del objeto a la layer base
            transform.parent = padreObjeto.GetLootObjetoSeguirTransformada(); // Colocamos el objeto en el punto de spawn del padre del objeto
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Colocamos el objeto en el punto de spawn del padre del objeto
        } else { // El jugador es al que se le hace padre por lo que tenemos que asegurarnos de rotar el arma para que se vea bien
            CambiarLayer(layerObjetoEnPosesion); // Cambiamos la layer del objeto a la layer de objeto en posesion
            transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f)); // Colocamos el objeto en el punto de spawn del padre del objeto
        }
    }

    public ILootObjetoPadre GetPadreObjeto() {
        return padreObjeto;
    }

    private void CambiarLayer(string layer) {
        gameObject.layer = LayerMask.NameToLayer(layer);
    }

    public bool AsignarPadre(IPersonajeJuego personaje) {
        if (personaje as Player || personaje as Enemigo) {
            personaje.SetearComoPadre(this);
            return true;
        }
        return false;
    }

    public void SetPoolObjetos(PoolObjetos pool) {
        poolObjetos = pool;
    }

}
