using System;
using System.Linq;
using System.Collections.Generic;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

using Logger = BepInEx.Logging.Logger;

namespace BepInEx
{
    /// <summary>
    /// Show log entries marked as "Message" on the game screen
    /// </summary>
    [BepInPlugin(GUID, PluginName, PluginVersion)]
    public partial class MessageCenter : BasePlugin
    {
        internal const string GUID = "BepInEx.MessageCenterIL2CPP_net6";
        internal const string PluginName = "Message Center";
        internal const string PluginVersion = "0.7";

        //Game Object shared between all BepInExUtility plugins
        public GameObject BepInExUtility;

        private static ConfigEntry<bool> Enabled;
        private static ConfigEntry<string> BlackList;

        private static string[] BlockedWords;

        public override void Load()
        {
            Enabled = Config.Bind("General", "Show messages in UI", true, "Allow plugins to show on screen messages");
            BlackList = Config.Bind("General", "Black List", "BepInEx Unity", "Console messages containing these words will not be displayed in-game");

            Logger.Listeners.Add(new MessageLogListener());

            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<MessageCenterComponent>();

            BepInExUtility = GameObject.Find("BepInExUtility");

            if (BepInExUtility == null)
            {
                BepInExUtility = new GameObject("BepInExUtility");
                GameObject.DontDestroyOnLoad(BepInExUtility);
                BepInExUtility.hideFlags = HideFlags.HideAndDontSave;
                BepInExUtility.AddComponent<MessageCenterComponent>();
            }
            else BepInExUtility.AddComponent<MessageCenterComponent>();
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
                    fontSize = 20,
                    richText = false
                };
            }

            var textColor = Color.black;
            var outlineColor = Color.white;

            if (_showCounter <= 1)
            {
                textColor.a = _showCounter;
                outlineColor.a = _showCounter;
            }

            ShadowAndOutline.DrawOutline(new Rect(40, 20, Screen.width - 80, 160), _shownLogText, _textStyle, textColor, outlineColor, 2);
        }
    }
}

