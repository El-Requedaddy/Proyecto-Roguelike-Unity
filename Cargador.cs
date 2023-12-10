using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Cargador { // Clase que no está attacheada a ninguna instancia de objeto

    public enum Escena { // Enumerador de escenas
        MenuPrincipalScene,
        GameScene,
        PantallaCargaScene
    }

    private static Escena escenaObjetivo;

    public static void CargarEscena(Escena escenaObjetivo) { // Método para cargar escenas con una pantalla de carga
        Cargador.escenaObjetivo = escenaObjetivo;

        SceneManager.LoadScene(Escena.PantallaCargaScene.ToString());
    }

    public static void CargadorCallback() {
        SceneManager.LoadScene(escenaObjetivo.ToString()); // Cargamos la escena objetivo
    }

}
