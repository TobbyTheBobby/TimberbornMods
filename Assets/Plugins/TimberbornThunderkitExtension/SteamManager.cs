using Steamworks;
using UnityEditor;
using UnityEngine;

namespace TimberbornThunderkitExtension
{
    [InitializeOnLoad]
    public static class SteamManagerEditor
    {
        private static bool initialized = false;

        static SteamManagerEditor()
        {
            Initialize();
            EditorApplication.update += Update;
        }

        public static bool Initialize()
        {
            if (initialized) 
                return true;
            
            try
            {
                if (SteamAPI.RestartAppIfNecessary(SteamAppId.AppId))
                {
                    Debug.LogError("Steam must be running to use Steamworks API.");
                    return false;
                }
            }
            catch (System.DllNotFoundException e)
            {
                Debug.LogError("Could not load [lib]steam_api.dll/so/dylib.\n" + e);
                return false;
            }

            if (!SteamAPI.Init())
            {
                Debug.LogError("SteamAPI_Init() failed.");
                return false;
            }

            initialized = true;
            return true;
        }

        private static void Update()
        {
            if (initialized)
            {
                SteamAPI.RunCallbacks();
            }
        }

        public static void Shutdown()
        {
            if (initialized)
            {
                SteamAPI.Shutdown();
                initialized = false;
            }
        }
    }
}