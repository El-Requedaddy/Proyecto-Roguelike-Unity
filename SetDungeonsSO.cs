using DungeonArchitect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SetDungeonsSO : ScriptableObject {

    // Cada array contiene 4 dungeons distintas en la que varia el Personaje con el que se empieza 
    public Dungeon[] dungeonsDificultadFacil;
    public Dungeon[] dungeonsDificultadNormal;
    public Dungeon[] dungeonsDificultadDificil;
    
}