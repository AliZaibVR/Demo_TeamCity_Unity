using Arthur.Client.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBuildConfiguration
{
    GameConfiguration config;
    bool browseBuildLocationFlag;
    string path;
    
    public GameBuildConfiguration()
    {
        Config = new GameConfiguration();
    }

    public GameConfiguration Config
    {
        get
        {
            return config;
        }
        set
        {
            config = value;
        }
    }
    public void setConfig(GameBuildType build, string buildPath)
    {
        config.buildType = build;
        
        browseBuildLocationFlag = false;
        path = buildPath;
        config.buildFileName = "";
    }
    public void setConfig(GameBuildType buildType,bool BrowseBuildLocation)
    {
        browseBuildLocationFlag = BrowseBuildLocation;
        config.buildFileName = "";
        config.buildType = buildType;
    }
    public virtual bool BuildPlayer()
    {
        Debug.Log("BuildPlayer: Start Building");

        foreach (var s in EditorBuildSettings.scenes)
        {
            s.enabled = false;
        }
        if (browseBuildLocationFlag)
        {
            path = EditorUtility.SaveFolderPanel("Choose Location of Built Client", "", "");
            if (string.IsNullOrEmpty(path))
                return false;
        }

        var sceneName = "";// SceneManager.GetSceneByName("GuestBrowser").path;
        //   Debug.Log("path: " + path );
        switch (config.buildType) 
        {
            case GameBuildType.GameLobby:

                foreach (var x in EditorBuildSettings.scenes)
                {
                    if (x.path.Contains("GameLobby"))
                        sceneName = x.path;
                }
                Debug.Log("SceneName: " + sceneName);
                config.buildFileName += "/GameLobby/GameLobby.exe";

                break;

            case GameBuildType.GameServer:

                foreach (var x in EditorBuildSettings.scenes)
                {
                    if (x.path.Contains("GameServer"))
                        sceneName = x.path;
                }
                Debug.Log("SceneName: " + sceneName);
                config.buildFileName += "/GameServer/GameServer.exe";


                break;
            case GameBuildType.AdventureServer:

                foreach (var x in EditorBuildSettings.scenes)
                {
                    if (x.path.Contains("AdventureServer"))
                        sceneName = x.path;
                }
                Debug.Log("SceneName: " + sceneName);
                config.buildFileName += "/AdventureServer/AdventureServer.exe";


                break;

            case GameBuildType.GameClient:

                foreach (var x in EditorBuildSettings.scenes)
                {
                    if (x.path.Contains("GameClient"))
                        sceneName = x.path;
                }
                Debug.Log("SceneName: " + sceneName);
                config.buildFileName += "/GameClient/GameClient.exe";

                break;

        }

        var scenesList = new List<string>();
        
        if (!string.IsNullOrEmpty(sceneName))
        {
            scenesList.Add(sceneName);
        }
        var scenes = new string[scenesList.Count];

        for (int i = 0; i < scenesList.Count; i++)
        {
            scenes[i] = scenesList[i];
            Debug.Log("Scene: "+scenesList[i]);
        }
        //turn off unity splash
        PlayerSettings.SplashScreen.showUnityLogo = false;
        // set quality settings to fantastic
        QualitySettings.SetQualityLevel((int)QualityLevel.Fantastic);
        // Build player.
        Debug.Log("BuildPath: "+ path);
        var error = BuildPipeline.BuildPlayer(scenes, path + config.buildFileName, BuildTarget.StandaloneWindows64, BuildOptions.None);
        Debug.Log("BuildPlayer: EndBuilding: " + error);
        if (string.IsNullOrEmpty(error))
            return true;
        else
            return false;
    }
}


public class GameConfiguration
{
    public GameBuildType buildType;
    public string buildFileName;

}
