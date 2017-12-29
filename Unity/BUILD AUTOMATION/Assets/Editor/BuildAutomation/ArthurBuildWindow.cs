using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArthurBuildWindow : EditorWindow
{
    bool GameLobby,GameServer,AdventureServer,GameClient;
    bool GuestVR, GuestViewer;

    [MenuItem("Build/Build Panel")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ArthurBuildWindow));
    }

    void OnGUI()
    {
        // The actual window code goes here
        GUILayout.Label("Game Build Panel", EditorStyles.boldLabel);

        GameLobby = EditorGUILayout.Toggle("GameLobby", GameLobby);

        GameServer = EditorGUILayout.Toggle("GameServer", GameServer);

        AdventureServer = EditorGUILayout.Toggle("AdventureServer", AdventureServer);

        GameClient = EditorGUILayout.Toggle("GameClient", GameClient);

        GUILayout.Space(30);
        if (GUILayout.Button("Build"))
        {
            BuildAutomation.BuildAllWindowsStandaloneUsingEditor(GameLobby, GameServer, AdventureServer,GameClient);

        }
    }
}

