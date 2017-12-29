using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;

public class BuildAutomation {
    static GameBuildConfiguration arthurBuildHelper = new GameBuildConfiguration();

    public static void BuildAllWindowsStandaloneUsingEditor(bool gameLobby,bool gameServer,bool adventureServer,bool gameClient)
    {
        bool locationFlag = true;
        bool returnStatus = true;
        if (gameLobby)
        {
            arthurBuildHelper.setConfig(GameBuildType.GameLobby, locationFlag);
            returnStatus = arthurBuildHelper.BuildPlayer();
            locationFlag = false;
        }
        
        if (gameServer && returnStatus == true)
        {
            arthurBuildHelper.setConfig(GameBuildType.GameServer, locationFlag);
            returnStatus = arthurBuildHelper.BuildPlayer();
            locationFlag = false;
        }
        if (adventureServer && returnStatus == true)
        {
            arthurBuildHelper.setConfig(GameBuildType.AdventureServer, locationFlag);
            returnStatus = arthurBuildHelper.BuildPlayer();
            locationFlag = false;
        }
        if (gameClient && returnStatus == true)
        {
            arthurBuildHelper.setConfig(GameBuildType.GameClient, locationFlag);
            returnStatus = arthurBuildHelper.BuildPlayer();
            locationFlag = false;
        }
    }
   
    public static void BuildWindowsStandaloneUsingCMD()
    {
        var outputDir = GetArg("-outputDir");
        Debug.LogError("buildClientCMD: "+outputDir);
        if (outputDir == null)
        {
            throw new ArgumentException("No output folder specified.");
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        var configStr = GetArg("-buildConfig");
        var strList = configStr.Split('-');
        var buildType = (GameBuildType) Enum.Parse(typeof(GameBuildType) ,strList[0]);
        
        arthurBuildHelper.setConfig(buildType, outputDir);
        arthurBuildHelper.BuildPlayer();
    }
    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
    static GameConfiguration getBuildConfiguration(string args)
    {
        var config = new GameConfiguration();
        var configParam = args.Split('-');

        return config;
    }
}

