using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorSonido : MonoBehaviour {

    private const string PLAYER_PREFS_VOLUMEN_EFECTOS_SONIDO = "VolumenEfectosSonido";

    public static GestorSonido Instance { get; private set; }

    [SerializeField] private ReferenciasClipsAudioSO referenciasClipsAudioSO;

    private float volumenEfectosSonido = 1f;

    private void Awake() {
        Instance = this;

        volumenEfectosSonido = PlayerPrefs.GetFloat(PLAYER_PREFS_VOLUMEN_EFECTOS_SONIDO, 1f);
    }

    private void Animator_OnUsoObjetoInicia(object sender, EventArgs e) {
        Debug.Log("GestorSonido.Animator_OnUsoObjetoInicia()");
    }

    private void ReproducirSonido(AudioClip audioClip, Vector3 posicion, float volumen = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, posicion, volumen);
    }

    private void ReproducirSonido(AudioClip[] audioClips, Vector3 posicion, float multiplicadorVolumen = 1f) {
        ReproducirSonido(audioClips[UnityEngine.Random.Range(0, audioClips.Length)], posicion, multiplicadorVolumen * volumenEfectosSonido);
    }

    public void CambiarVolumen() {
        volumenEfectosSonido += 0.1f;
        if (volumenEfectosSonido > 1.1f) {
            volumenEfectosSonido = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_VOLUMEN_EFECTOS_SONIDO, volumenEfectosSonido); // Guardar en PlayerPrefs el volumen de los efectos de sonido para que se mantenga entre partidas
        PlayerPrefs.Save();
    }

    public float GetVolumenEfectosSonido() {
        return volumenEfectosSonido;
    }

}
