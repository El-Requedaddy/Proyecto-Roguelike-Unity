using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartidaManager : MonoBehaviour {

    public static PartidaManager Instance { get; private set; }

    public event EventHandler OnEstadoCambia;
    public event EventHandler OnJuegoPausado;
    public event EventHandler OnJuegoReanudado;
    public event EventHandler OnJuegoPerdido;
    public event EventHandler OnJuegoGanado;

    private enum State {
        Jugando,
        Pausa,
        GameOver,
        Victoria
    }

    private State state;

    private void Start() {
        GameInput.Instance.OnPausa += GameInput_OnPausa;
    }

    private void GameInput_OnPausa(object sender, EventArgs e) {
        GestionPausa();
    }

    private void Awake() {
        if (Instance != null) { // Si ya hay una instancia de Player, no se crea una nueva y se muestra un mensaje de error en la consola
            Debug.Log("Hay mas de un Manager, ESPABILA NOTAS!!!");
        }
        Instance = this; // Asignamos la instancia de Player a la variable estática Instance
        state = State.Jugando;
    }

    private void Update() {
        switch (state) {
            case State.Jugando:
                OnEstadoCambia?.Invoke(this, EventArgs.Empty);
                break;
            case State.Pausa:
                OnEstadoCambia?.Invoke(this, EventArgs.Empty);
                break;
            case State.GameOver:
                OnEstadoCambia?.Invoke(this, EventArgs.Empty);
                break;
            case State.Victoria:
                OnEstadoCambia?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    public bool EstaJugando() {
        return state == State.Jugando;
    }

    public bool EstaEnPausa() {
        return state == State.Pausa;
    }

    public bool EstaEnGameOver() {
        return state == State.GameOver;
    }

    public void GestionPausa() { // Función que gestiona la pausa del juego
        if (state == State.Jugando) {
            state = State.Pausa;
            Time.timeScale = 0f;
            OnJuegoPausado?.Invoke(this, EventArgs.Empty);
        } else if (state == State.Pausa) {
            state = State.Jugando;
            Time.timeScale = 1f;
            OnJuegoReanudado?.Invoke(this, EventArgs.Empty);
        }
    }

}