using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcionesUI : MonoBehaviour {

    public static OpcionesUI Instance { get; private set; }

    [SerializeField] private Button efectosSonidoButton;
    [SerializeField] private Button musicaButton;
    [SerializeField] private Button volverButton;

    [SerializeField] private Text textoEfectosSonido;
    [SerializeField] private Text textoMusica;

    [SerializeField] private Button moverArribaButton;
    [SerializeField] private Button moverAbajoButton;
    [SerializeField] private Button moverIzquierdaButton;
    [SerializeField] private Button moverDerechaButton;
    [SerializeField] private Button interactuarButton;
    [SerializeField] private Button atacarButton;
    [SerializeField] private Button pausaButton;

    // Gamepad
    [SerializeField] private Button gamepadInteractuarButton;
    [SerializeField] private Button gamepadAtacarButton;
    [SerializeField] private Button gamepadPausaButton;

    [SerializeField] private Text gamepadTextoInteractuar;
    [SerializeField] private Text gamepadTextoAtacar;
    [SerializeField] private Text gamepadTextoPausa;


    [SerializeField] private Text textoMoverArriba;
    [SerializeField] private Text textoMoverAbajo;
    [SerializeField] private Text textoMoverIzquierda;
    [SerializeField] private Text textoMoverDerecha;
    [SerializeField] private Text textoInteractuar;
    [SerializeField] private Text textoAtacar;
    [SerializeField] private Text textoPausa;

    [SerializeField] private Transform pulsarParaBindear;

    private Action onCerrarBotonPulsado;

    private void Awake() {
        Instance = this;

        efectosSonidoButton.onClick.AddListener(() => {
            GestorSonido.Instance.CambiarVolumen();
            ActualizarNumeroEfectoSonido();
        });
        musicaButton.onClick.AddListener(() => {
            GestorMusica.Instance.CambiarVolumen();
            ActualizarNumeroMusica();
        });
        volverButton.onClick.AddListener(() => {
            Ocultar();
            onCerrarBotonPulsado();
        });

        moverArribaButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Mover_Up);
        });
        moverAbajoButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Mover_Down);
        });
        moverIzquierdaButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Mover_Left);
        });
        moverDerechaButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Mover_Right);
        });
        interactuarButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Interaccionar);
        });
        atacarButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Ataque);
        });
        pausaButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Pausa);
        });

        // Gamepad
        gamepadInteractuarButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Gamepad_Interaccionar);
        });
        gamepadAtacarButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Gamepad_Ataque);
        });
        gamepadPausaButton.onClick.AddListener(() => {
            BindearTeclas(GameInput.Binding.Gamepad_Pausa);
        });

        //Ocultar();
    }

    private void Start() {
        PartidaManager.Instance.OnJuegoReanudado += PartidaManager_OnJuegoReanudado;
        ActualizarNumeroEfectoSonido();
        ActualizarNumeroMusica();
        ActualizarBindings();
        OcultarPulsarParaBindear();
        Ocultar();
    }

    private void PartidaManager_OnJuegoReanudado(object sender, EventArgs e) {
        Ocultar();
    }

    public void Mostrar(Action onCerrarBotonPulsado) {
        this.onCerrarBotonPulsado = onCerrarBotonPulsado;

        gameObject.SetActive(true);
        efectosSonidoButton.Select();
    }

    private void Ocultar() {
        gameObject.SetActive(false);
    }

    private void ActualizarNumeroEfectoSonido() {
        textoEfectosSonido.text = "Efectos de sonido: " + Mathf.Round(GestorSonido.Instance.GetVolumenEfectosSonido() * 10f);
    }

    private void ActualizarNumeroMusica() {
        textoMusica.text = "Musica: " + Mathf.Round(GestorMusica.Instance.GetVolumenMusica() * 10f);
    }

    private void ActualizarBindings() {
        textoMoverArriba.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Mover_Up);
        textoMoverAbajo.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Mover_Down);
        textoMoverIzquierda.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Mover_Left);
        textoMoverDerecha.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Mover_Right);
        textoInteractuar.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Interaccionar);
        textoAtacar.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Ataque);
        textoPausa.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Pausa);

        // Gamepad
        gamepadTextoInteractuar.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Gamepad_Interaccionar);
        gamepadTextoAtacar.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Gamepad_Ataque);
        gamepadTextoPausa.text = GameInput.Instance.GetTextoBinding(GameInput.Binding.Gamepad_Pausa);
    }

    private void MostrarPulsarParaBindear() {
        pulsarParaBindear.gameObject.SetActive(true);
    }

    private void OcultarPulsarParaBindear() {
        pulsarParaBindear.gameObject.SetActive(false);
    }

    private void BindearTeclas(GameInput.Binding bindeo) { // Bindeamos teclas para cada accion
        MostrarPulsarParaBindear();

        GameInput.Instance.CambiarBinding(bindeo, () => { // Función que se ejecuta cuando se pulsa una tecla
            OcultarPulsarParaBindear();
            ActualizarBindings();
        });
    }

}