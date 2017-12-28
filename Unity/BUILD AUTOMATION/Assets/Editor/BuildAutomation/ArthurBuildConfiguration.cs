using Arthur.Client.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArthurBuildConfiguration
{
    ArthurConfiguration config;
    bool browseBuildLocationFlag;
    string path;
    
    public ArthurBuildConfiguration()
    {
        Config = new ArthurConfiguration();
    }

    public ArthurConfiguration Config
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
    public void setConfig(ArthurClient cl, ArthurVRMode vrMode, ArthurBuildMode bMode, string buildPath)
    {
        config.client = cl;
        config.vrMode = vrMode;
        config.buildMode = bMode;
        browseBuildLocationFlag = false;
        path = buildPath;
        config.buildFileName = "";
    }
    public void setConfig(ArthurClient cl,ArthurVRMode vrMode,ArthurBuildMode bMode,bool BrowseBuildLocation)
    {
        config.client = cl;
        config.vrMode = vrMode;
        config.buildMode = bMode;
        browseBuildLocationFlag = BrowseBuildLocation;
        config.buildFileName = "";
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

    //   Debug.Log("path: " + path );


        switch (config.client)
        {
            case ArthurClient.Acciluim:
                break;
            case ArthurClient.Boost:
                break;
            case ArthurClient.Develop:
                break;
            case ArthurClient.NextVR:
                break;
        }

        var guestSceneName = "";// SceneManager.GetSceneByName("GuestBrowser").path;
        var mainSceneName = "";// SceneManager.GetSceneByName("MainScene").path;
        foreach(var x in EditorBuildSettings.scenes)
        {
            if (x.path.Contains("GuestBrowser"))
                guestSceneName = x.path;
            if (x.path.Contains("MainScene"))
                mainSceneName = x.path;
        }
//        Debug.Log("GuestSceneName: " + guestSceneName);
//        Debug.Log("Mainscene: " + mainSceneName);



        var scenesList = new List<string>();
        if (config.buildMode == ArthurBuildMode.Guest)
        {
            //
            if (!string.IsNullOrEmpty(guestSceneName) && !string.IsNullOrEmpty(mainSceneName))
            {
                scenesList.Add(guestSceneName);
                scenesList.Add(mainSceneName);
            }
            

            if (config.vrMode == ArthurVRMode.VRMode)
            {
                PlayerSettings.virtualRealitySupported = true;
                ArthurUtilities.ArthurBuildSettings.isGuest = true;
                ArthurUtilities.ArthurBuildSettings.isViewer = false;
                config.buildFileName += "/ArthurGuestVR/ArthurVR-Guest Edition.exe";
            }
            else
            {
                PlayerSettings.virtualRealitySupported = false;
                ArthurUtilities.ArthurBuildSettings.isGuest = true;
                ArthurUtilities.ArthurBuildSettings.isViewer = true;
                config.buildFileName += "/ArthurGuestViewer/ArthurViewer-Guest Edition.exe";
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(mainSceneName))
            {
                scenesList.Add(mainSceneName);
            }
            if (config.vrMode == ArthurVRMode.VRMode)
            {
                PlayerSettings.virtualRealitySupported = true;
                ArthurUtilities.ArthurBuildSettings.isGuest = false;
                ArthurUtilities.ArthurBuildSettings.isViewer = false;
                config.buildFileName += "/ArthurVR/ArthurVR.exe";
            }
            else
            {
                PlayerSettings.virtualRealitySupported = false;
                ArthurUtilities.ArthurBuildSettings.isGuest = false;
                ArthurUtilities.ArthurBuildSettings.isViewer = true;
                config.buildFileName += "/ArthurViewer/ArthurViewer.exe";
            }
        }

        var scenes = new string[scenesList.Count];
        Debug.Log("Scene List: "+ scenesList.Count);

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
        Debug.Log("Quality Settings: " + QualitySettings.GetQualityLevel());
        var error =  BuildPipeline.BuildPlayer(scenes, path + config.buildFileName , BuildTarget.StandaloneWindows64, BuildOptions.None);
        Debug.Log("BuildPlayer: EndBuilding: "+ error);
        if (string.IsNullOrEmpty(error))
            return true;
        else
            return false;
    }
}


public class ArthurConfiguration
{
    public ArthurClient client;
    public ArthurVRMode vrMode;
    public ArthurBuildMode buildMode;
    public string buildFileName;

}
public enum ArthurClient
{
    Boost,
    Acciluim,
    Develop,
    NextVR
}
public enum ArthurVRMode
{
    VRMode,
    ViewerMode
}
public enum ArthurBuildMode
{
    Standard,
    Guest
}

