using HarmonyLib;
using NeosModLoader;
using UnityEngine;

namespace FixAltEnter
{
    public class FixAltEnter : NeosMod
    {
        public override string Name => "FixAltEnter";
        public override string Author => "badhaloninja";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/badhaloninja/FixAltEnter";
        public override void OnEngineInit()
        {
            if (Screen.fullScreen)
            { // Fix resolution on launch if fullscreen
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
            }

            Harmony harmony = new Harmony("me.badhaloninja.FixAltEnter");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(FrooxEngineRunner), "Update")] // Unity MonoBehavior update loop
        class MonoUpdate // I really wanted to just patch alt + enter unity keybind but could not find it 
        {
            static bool wasFullscreen = Screen.fullScreen;
            static int windowedWidth = Screen.width;
            static int windowedHeight = Screen.height;
            public static void Postfix()
            {
                if (Screen.fullScreen == wasFullscreen) return; // Skip if no change detected

                wasFullscreen = Screen.fullScreen; 

                int newWidth = windowedWidth;
                int newHeight = windowedHeight;

                if (wasFullscreen)
                {
                    // Get monitor resolution
                    newWidth = Display.main.systemWidth;
                    newHeight = Display.main.systemHeight;

                    // This is supposed to do that but it seems to have inconsistant behavior while testing 
                    // https://forum.unity.com/threads/get-screen-resolution-not-window-resolution.319511/#post-3104959
                    /*newWidth = Screen.currentResolution.width;
                    newHeight = Screen.currentResolution.height;*/

                    // Store Windowed size
                    windowedWidth = Screen.width;
                    windowedHeight = Screen.height;
                }

                Screen.SetResolution(newWidth, newHeight, wasFullscreen);
            }
        }
    }
}