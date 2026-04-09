using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState", order = 1)]
public class GameState : ScriptableObject
{
    public Vector3 currentLevel;
}
