using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx;
using BepInEx.IL2CPP;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnhollowerRuntimeLib;
using Logger = BepInEx.Logging.Logger;

namespace MessageCenterIL2CPP_netFm
{
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public partial class MessageCenterIL2CPP_netFm : BasePlugin
    {
        public const string GUID = "SpockBauru.MessageCenterIL2CPP_netFm";
        public const string PluginName = "Message Center";
        public const string PluginVersion = "0.5";

        //Game Object shared between all SpockPlugins_BepInEx plugins
        public GameObject SpockPlugins_BepInEx;

        public static ConfigEntry<bool> Enabled { get; private set; }
        public static ConfigEntry<string> BlackList { get; private set; }

        private static string[] BlockedWords;

        public override void Load()
        {
            Enabled = Config.Bind("General", "Show messages in UI", true, "Allow plugins to show on screen messages");
            BlackList = Config.Bind("General", "Black List", "BepInEx Unity", "Don't show console messages containing words from this list.");

            Logger.Listeners.Add(new MessageLogListener());

            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<MessageCenterComponent>();

            SpockPlugins_BepInEx = GameObject.Find("SpockPlugins_BepInEx");

            if (SpockPlugins_BepInEx == null)
            {
                SpockPlugins_BepInEx = new GameObject("SpockPlugins_BepInEx");
                GameObject.DontDestroyOnLoad(SpockPlugins_BepInEx);
                SpockPlugins_BepInEx.hideFlags = HideFlags.HideAndDontSave;
                SpockPlugins_BepInEx.AddComponent<MessageCenterComponent>();
            }
            else SpockPlugins_BepInEx.AddComponent<MessageCenterComponent>();
        }

        internal static void OnEntryLogged(LogEventArgs logEventArgs)
        {
            if (!Enabled.Value) return;

            BlockedWords = BlackList.Value.Split(' ');
            foreach (string word in BlockedWords)
                if (word.Equals(logEventArgs.Source.SourceName, StringComparison.Ordinal)) return;

            if ((logEventArgs.Level & LogLevel.Message) == LogLevel.None) return;

            if (logEventArgs.Data != null)
                MessageCenterComponent.ShowText(logEventArgs.Data.ToString());
        }
    }

    public partial class MessageCenterComponent : MonoBehaviour
    {
        //Got this from BepInEx Discord pinned messages
        public MessageCenterComponent(IntPtr handle) : base(handle) { }

        private static readonly List<LogEntry> _shownLogLines = new List<LogEntry>();

        private static float _showCounter;
        private static string _shownLogText = string.Empty;

        private void Start()
        {
            List<string> dependencyErrors = IL2CPPChainloader.Instance.DependencyErrors;
            foreach (var dependencyError in dependencyErrors)
                ShowText(dependencyError);
        }

        internal static void ShowText(string logText)
        {
            if (_showCounter <= 0)
                _shownLogLines.Clear();

            _showCounter = Mathf.Clamp(_showCounter, 7, 12);

            var logEntry = _shownLogLines.FirstOrDefault(x => x.Text.Equals(logText, StringComparison.Ordinal));
            if (logEntry == null)
            {
                logEntry = new LogEntry(logText);
                _shownLogLines.Add(logEntry);

                _showCounter += 0.8f;
            }

            logEntry.Count++;

            var logLines = _shownLogLines.Select(x => x.Count > 1 ? $"{x.Count}x {x.Text}" : x.Text).ToArray();
            _shownLogText = string.Join("\r\n", logLines);
        }

        private void Update()
        {
            if (_showCounter > 0)
                _showCounter -= Time.deltaTime;
        }

        private GUIStyle _textStyle;
        private void OnGUI()
        {
            if (_showCounter <= 0) return;

            if (_textStyle == null)
            {
                _textStyle = new GUIStyle
                {
                    alignment = TextAnchor.UpperLeft,
                    fontSize = 20
                };
            }

            var textColor = Color.black;
            var outlineColor = Color.white;

            if (_showCounter <= 1)
            {
                textColor.a = _showCounter;
                outlineColor.a = _showCounter;
            }

            ShadowAndOutline.DrawOutline(new Rect(40, 20, Screen.width - 80, 160), _shownLogText, _textStyle, textColor, outlineColor, 3);
        }
    }
}

