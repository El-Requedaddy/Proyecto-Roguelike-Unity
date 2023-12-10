using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoSeleccionadoVisual : MonoBehaviour
{

    [SerializeField] private Objeto objetoLoot; // objeto a cambiar de visual
    [SerializeField] private GameObject visualGameObject;

    // Start is called before the first frame update
    private void Start() {
        Player.Instance.OnObjetoSeleccionadoCambia += Instance_OnObjetoSeleccionadoCambia;
    }

    private void Instance_OnObjetoSeleccionadoCambia(object sender, Player.OnObjetoSeleccionadoCambiaEventArgs e) {
        if (objetoLoot == null) {
            return;
        }
        if (e.objetoSeleccionado == (IElementoInteraccionable)objetoLoot) {
            visualGameObject.SetActive(true); // Activamos el objeto visual
        } else {
            visualGameObject.SetActive(false); // Desactivamos el objeto visual
        }
    }

}
