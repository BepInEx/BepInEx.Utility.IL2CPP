//========================================= For Beginners ============================================
// If you are a beginner, I HIGHLY recommed that you make at least the first module of this course:
// https://learn.unity.com/pathway/junior-programmer
//====================================================================================================

//========================================= Documentation ============================================
// We will use BepInEx 6.0.0-pre.1: https://docs.bepinex.dev/v6.0.0-pre.1/
//====================================================================================================

//==================================== Getting IL2CPP Libraries ======================================
// Before start, we need to make the libraries. Patch the game with BepInEx 6.0.0-pre.1 and let it
// populate BepInEx folders. Internet is needed.
// Make a new folder in the project for the libraries. Copy all the content from both BepInEx/core
// and BepInEx\unhollowed to your library folder.
//====================================================================================================

// ========================================= REFERENCES ==============================================
// System is always needed. If asked, use IntPtr from here
using System;
using System.Collections;

// From BepInEx.core.dll
using BepInEx;
using BepInEx.Configuration;
// From BepInEx.IL2CPP.dll
using BepInEx.IL2CPP;
using BepInEx.IL2CPP.Utils.Collections;
// From UnhollowerBaseLib.dll 
using UnhollowerRuntimeLib;

// From UnityEngine.CoreModule.dll. It must be the one from BepInEx\unhollowed folder
using UnityEngine;
using UnityEngine.SceneManagement;

// Also make a reference on your library to Il2Cppmscorlib.dll, from BepInEx\unhollowed folder

//=========================================== MAIN PATCH =============================================
namespace IL2CppMonobehaviorTemplate
{
    /// <summary>
    /// Add here a brief description
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]

    // BasePlugin don't inherits things from Monobehavior. That's why this tempalte exists...
    public class MonoTemplate : BasePlugin
    {
        // Nothing new here
        internal const string GUID = "com.yourName.pluginName";
        internal const string PluginName = "Monobehavrion Injection Template";
        internal const string PluginVersion = "0.0.0";

        // New Configuration Manager things:
        // Watch out the new configuration manager. Don't rely on the "Apply Settinge" button.
        // Open BepInEx\config\com.sinai.BepInExConfigManager.cfg and enable Auto-save settings to see
        // if your config really works as intended.
        private static ConfigEntry<bool> ShowConsoleMessage;

        // Make an Game Object shared between all your plugins. This will host all your monobehavior components.
        // Is possible to not use it, but since IL2CPP is kinda unstable, is safe to put your things in an
        // separated object. Also don't create a new one for each project, or soon the game will have 
        // to deal with hundreds of different GameObjects from modders.
        public GameObject YourName;

        // "Load" is the new "Main"
        public override void Load()
        {
            ShowConsoleMessage = Config.Bind("General", 
                                             "Show a message in the console",
                                             false, 
                                             "A test message is shown in the console when its enabled/disabled");
            ShowConsoleMessage.SettingChanged += (sender, args) => Debug.Log("Toggle from Configuration Manager");

            // IL2CPP don't automatically inherits Monobehavior, so needs to add a component separatelly
            ClassInjector.RegisterTypeInIl2Cpp<MonoTemplateComponent>();

            // As said before, we will share all our monobehavior scripts from all your projects in the same Game Object,
            // so we need to search if it was already created by other or your plugins.
            YourName = GameObject.Find("YourName");
            if (YourName == null)
            {
                YourName = new GameObject("YourName");
                GameObject.DontDestroyOnLoad(YourName);
                YourName.hideFlags = HideFlags.HideAndDontSave;
                YourName.AddComponent<MonoTemplateComponent>();
            }
            else YourName.AddComponent<MonoTemplateComponent>();
        }
    }

    //=================================== MONOBEHAVIOR COMPONENT =========================================
    //Now we can finally use MonoBehavior things like Start() and Update()!
    public class MonoTemplateComponent : MonoBehaviour
    {
        // Nothing works without this hook. Beginners: Just copy and paste
        public MonoTemplateComponent(IntPtr handle) : base(handle) { }

        // Now we can finally use the old things!!!
        // For Beginners: Don't forget to cache variables used in methods that are often called, like Update or coroutines
        private string textInUpdate = "Monobehavior Update";
        private float timer = 0;
        private WaitForSeconds fiveSeconds = new WaitForSeconds(5f);

        private void Start()
        {
            Debug.Log("Monobehavior Start");

            //Coroutines are done differently, use WrapToIl2Cpp() from BepInEx.IL2CPP.Utils.Collections
            StartCoroutine(MonoCoroutine().WrapToIl2Cpp());

            //SceneManager.SceneLoaded is also done differently
            SceneManager.add_sceneLoaded(new Action<Scene, LoadSceneMode>((s, lsm) => OnSceneLoaded()));
        }

        private IEnumerator MonoCoroutine()
        {
            // In IL2CPP, coroutines don't have always the expected behavior. You really need to test if its working as expected.
            while (true)
            {
                Debug.Log("Monobehavior Coroutine");
                yield return fiveSeconds;
            }
        }

        private void OnSceneLoaded()
        {
            Debug.Log("Monobehavior Sceneloaded");
        }

        private void Update()
        {
            // In IL2CPP, Update is stable and reliable
            // But remember that Update is called every single frame.

            // For Beginners: Avoid declaring variables here, chache them before. Don't use GameObject.Find("YourThing") here.
            // If you are changing strings, use StringBuilder, DON'T USE myString = "firstpart" + "secondpart";
            // Don't be lazy, use guard clauses! For beginners: https://www.youtube.com/watch?v=rHRbBXWT3Kc

            // For Everyone: https://www.youtube.com/watch?v=Xd4UhJufTx4

            timer += Time.deltaTime;
            if (timer < 6) return; // guard clause example

            Debug.Log(textInUpdate);
            timer = 0;
        }

        private void OnGui()
        {
            // GUI is The Achilles' Heel from IL2CPP
            // GUIStyle references sometimes work, sometimes don't
            // Many, MANY BUGS
            // Nothing is documented
            // WELCOME TO IL2CPP HELL
            // (recommended soundtrack: https://www.youtube.com/watch?v=Jm932Sqwf5E )
        }
    }
}
