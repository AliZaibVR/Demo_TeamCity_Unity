using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArthurBuildWindow : EditorWindow
{
    bool VR,Viewer;
    bool GuestVR, GuestViewer;

    [MenuItem("Arthur/Build Panel")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ArthurBuildWindow));
    }

    void OnGUI()
    {
        // The actual window code goes here
        GUILayout.Label("Arthur Build Panel", EditorStyles.boldLabel);

        VR = EditorGUILayout.Toggle("Arthur VR", VR);

        Viewer = EditorGUILayout.Toggle("Arthur Viewer", Viewer);

        GuestVR = EditorGUILayout.Toggle("Guest VR", GuestVR);

        GuestViewer = EditorGUILayout.Toggle("Guest Viewer", GuestViewer);
        GUILayout.Space(30);
        if (GUILayout.Button("Build"))
        {
            BuildAutomation.BuildAllWindowsStandaloneUsingEditor(VR, Viewer, GuestVR, GuestViewer);

        }
    }
}

