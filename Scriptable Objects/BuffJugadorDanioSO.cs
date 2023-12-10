using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuffJugadorDanioSO : BuffJugadorSO {

    [SerializeField] private float danioBuff;
    private ParticleSystem instancia;

    public override void ActivarBuff() {
        instancia = Instantiate(GetParticulas(), Player.Instance.transform);
        instancia.Play();
    }

    public override void DesactivarBuff() {
        instancia.Stop();
    }

}