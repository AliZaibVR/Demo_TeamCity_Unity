using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;

public class BuildAutomation {
    static ArthurBuildConfiguration arthurBuildHelper = new ArthurBuildConfiguration();

    public static void BuildAllWindowsStandaloneUsingEditor(bool VR,bool Viewer,bool GuestVR, bool GuestViewer)
    {
        bool locationFlag = true;
        bool returnStatus = true;
        if (Viewer)
        {
            arthurBuildHelper.setConfig(ArthurClient.Develop, ArthurVRMode.ViewerMode, ArthurBuildMode.Standard, locationFlag);
            returnStatus = arthurBuildHelper.BuildPlayer();
            locationFlag = false;
        }
        
        if (VR && returnStatus == true)
        {
            arthurBuildHelper.setConfig(ArthurClient.Develop, ArthurVRMode.VRMode, ArthurBuildMode.Standard, locationFlag);
            returnStatus = arthurBuildHelper.BuildPlayer();
            locationFlag = false;
        }
        if (GuestViewer && returnStatus == true)
        {
            arthurBuildHelper.setConfig(ArthurClient.Develop, ArthurVRMode.ViewerMode, ArthurBuildMode.Guest, locationFlag);
            returnStatus = arthurBuildHelper.BuildPlayer();
            locationFlag = false;
        }
        if (GuestVR && returnStatus == true)
        {
            arthurBuildHelper.setConfig(ArthurClient.Develop, ArthurVRMode.VRMode, ArthurBuildMode.Guest, locationFlag);
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
        var cl = (ArthurClient) Enum.Parse(typeof(ArthurClient) ,strList[0]);
        var vMode = (ArthurVRMode) Enum.Parse(typeof(ArthurVRMode), strList[1]);
        var bMode = (ArthurBuildMode) Enum.Parse(typeof(ArthurBuildMode), strList[2]);
        
        arthurBuildHelper.setConfig(cl,vMode,bMode,outputDir);
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
    static ArthurConfiguration getBuildConfiguration(string args)
    {
        var config = new ArthurConfiguration();
        var configParam = args.Split('-');

        return config;
    }
}

