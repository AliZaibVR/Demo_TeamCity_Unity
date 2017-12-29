using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Arthur.Client.Utilities
{
    public static class ArthurUtilities
    {
        static GameBuildSettings arthurBuildSettings;

        public static GameBuildSettings ArthurBuildSettings
        {
            get
            {
                if (arthurBuildSettings == null)
                {
                    arthurBuildSettings = (GameBuildSettings)Resources.Load("Arthur/ArthurBuildSettings");
                }
                return arthurBuildSettings;
            }
        }

    }
}
