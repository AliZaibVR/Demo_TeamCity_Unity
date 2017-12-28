using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArthurBuildSettings", menuName = "Arthur Build Settings", order = 1)]
public class ArthurBuildSettings : ScriptableObject
{
    public bool isGuest;
    public bool isViewer;
}
