using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameBuildSettings", menuName = "Game Build Settings", order = 1)]
public class GameBuildSettings : ScriptableObject
{
    public GameBuildType GameType;
    public bool isVRSupported;
    public bool isDebugBuild;
    public string GameRegion;
    public string ConnectionIP;
    public string ServerAccessKey;

}
public enum GameBuildType
{
    GameLobby,
    GameServer,
    AdventureServer,
    GameClient
}
