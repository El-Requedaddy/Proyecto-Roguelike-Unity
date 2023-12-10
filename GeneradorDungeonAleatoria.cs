using DungeonArchitect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDungeonAleatoria : MonoBehaviour {

    [SerializeField] private SetDungeonsSO setDungeons;
    [SerializeField] private string padre;

    [SerializeField] private Dungeon dungeonDificilMaga;
    [SerializeField] private Dungeon dungeonNormalMaga;
    [SerializeField] private Dungeon dungeonDificilBarbaro;
    [SerializeField] private Dungeon dungeonNormalBarbaro;
    [SerializeField] private Dungeon dungeonDificilRogue;
    [SerializeField] private Dungeon dungeonNormalRogue;
    [SerializeField] private Dungeon dungeonDificilCaballero;
    [SerializeField] private Dungeon dungeonNormalCaballero;

    private void Start() {

        int indicePersonaje = PlayerPrefs.GetInt("PersonajeElegido");
        string dificultad = PlayerPrefs.GetString("Dificultad");

        Dungeon dungeon; 

        switch (indicePersonaje) {
            case 0:
                if (dificultad == "Normal") {
                    dungeon = dungeonNormalMaga;
                } else {
                    dungeon = dungeonDificilMaga;
                }
                break;
            case 1:
                if (dificultad == "Normal") {
                    dungeon = dungeonNormalCaballero;
                } else {
                    dungeon = dungeonDificilCaballero;
                }
                break;
            case 2: 
                if (dificultad == "Normal") {
                    dungeon = dungeonNormalRogue;
                } else {
                    dungeon = dungeonDificilRogue;
                }
                break;
            case 3: 
                if (dificultad == "Normal") {
                    dungeon = dungeonNormalBarbaro;
                } else {
                    dungeon = dungeonDificilBarbaro;
                }
                break;
            default:
                dungeon = null;
                break;
        }

        dungeon.Config.Seed = (uint)(Random.value * int.MaxValue);
        Debug.Log("Dungeon.Config: " + dungeon.Config);
        dungeon.Build();

    }

    //private void Start() {
    //    int indicePersonaje = PlayerPrefs.GetInt("PersonajeElegido");
    //    string dificultad = PlayerPrefs.GetString("Dificultad");

    //    Dungeon dungeon = null;

    //    if (setDungeons != null) {
    //        for (int i = 0; i < setDungeons.dungeonsDificultadNormal.Length; i++) {
    //            if (i == indicePersonaje) {
    //                if (dificultad == "Normal") {
    //                    dungeon = setDungeons.dungeonsDificultadNormal[i];
    //                } else {
    //                    dungeon = setDungeons.dungeonsDificultadDificil[i];
    //                }
    //            }
    //        }

    //        Debug.Log("Dungeon: " + dungeon);
    //        PooledDungeonSceneProvider sceneProvider = dungeon.GetComponentInChildren<PooledDungeonSceneProvider>();
    //        Debug.Log("SceneProvider: " + sceneProvider);
    //        if (sceneProvider) {
    //            GameObject parentObject = GameObject.Find(padre);
    //            if (parentObject) {
    //                sceneProvider.itemParent = parentObject;
    //            } else {
    //                Debug.LogError("No se ha encontrado el objeto padre con el nombre: " + padre);
    //            }
    //        } else {
    //            Debug.LogError("No se ha encontrado el componente DungeonSceneProvider en el objeto Dungeon");
    //        }

    //        if (dungeon != null) {
    //            dungeon.Config.Seed = (uint)(Random.value * int.MaxValue);
    //            Debug.Log("Dungeon.Config: " + dungeon.Config);
    //            dungeon.Build();
    //        } else {
    //            Debug.LogError("No se ha encontrado el dungeon");
    //        }

    //    }
    //}

}
