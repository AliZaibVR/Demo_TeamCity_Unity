using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameBuildSettings", menuName = "Game Build Settings", order = 1)]
public class GameBuildSettings : ScriptableObject
{
    public GameBuildType gameRegion;

}
public enum GameBuildType
{
    GameLobby,
    GameServer,
    AdventureServer,
    GameClient
}
