using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GestorMusica : MonoBehaviour {

    private const string PLAYER_PREFS_VOLUMEN_MUSICA = "VolumenMusica";

    public static GestorMusica Instance { get; private set; }

    private float volumenMusica = .3f;
    private AudioSource audioSource;

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volumenMusica = PlayerPrefs.GetFloat(PLAYER_PREFS_VOLUMEN_MUSICA, .3f);
    }

    public void CambiarVolumen() {
        volumenMusica += 0.1f;
        if (volumenMusica > 1.1f) {
            volumenMusica = 0f;
        }
        audioSource.volume = volumenMusica;

        PlayerPrefs.SetFloat(PLAYER_PREFS_VOLUMEN_MUSICA, volumenMusica); // Guardar en PlayerPrefs el volumen de la musica para que se mantenga entre partidas
        PlayerPrefs.Save();
    }

    public float GetVolumenMusica() {
        return volumenMusica;
    }

}
