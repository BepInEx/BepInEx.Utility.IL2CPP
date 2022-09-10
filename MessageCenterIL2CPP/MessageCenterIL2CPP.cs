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


namespace MessageCenterIL2CPP
{
    [BepInPlugin(GUID, PluginName, Version)]
    public partial class MessageCenter : BasePlugin
    {
        public const string GUID = "SpockBauru.bepinex.messagecenter";
        public const string PluginName = "Message Center IL2CPP";
        public const string Version = "0.0.1";

        public static ConfigEntry<bool> Enabled { get; private set; }

        private static readonly List<LogEntry> _shownLogLines = new List<LogEntry>();

        internal static float _showCounter;
        internal static string _shownLogText = string.Empty;

        public override void Load()
        {
            Enabled = Config.Bind("General", "Show messages in UI", true, "Allow plugins to show on screen messages");
            Logger.Listeners.Add(new MessageLogListener());

            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            ClassInjector.RegisterTypeInIl2Cpp<MessageCenterComponent>();
            GameObject MessageCenterIL2CPP = new GameObject("MessageCenter_IL2CPP");
            GameObject.DontDestroyOnLoad(MessageCenterIL2CPP);
            MessageCenterIL2CPP.hideFlags = HideFlags.HideAndDontSave;
            MessageCenterIL2CPP.AddComponent<MessageCenterComponent>();
        }

        public static void OnEntryLogged(LogEventArgs logEventArgs)
        {
            if (!Enabled.Value) return;
            if ("BepInEx".Equals(logEventArgs.Source.SourceName, StringComparison.Ordinal)) return;
            if ("Unity".Equals(logEventArgs.Source.SourceName, StringComparison.Ordinal)) return;
            if ((logEventArgs.Level & LogLevel.Message) == LogLevel.None) return;


            if (logEventArgs.Data != null)
                ShowText(logEventArgs.Data.ToString());
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
    }

    
    public partial class MessageCenterComponent : MonoBehaviour
    {
        //Got this from BepInEx Discord pinned messages
        public MessageCenterComponent(IntPtr handle) : base(handle) { }

        private void Start()
        {
            List<string> dependencyErrors = IL2CPPChainloader.Instance.DependencyErrors;
            foreach (var dependencyError in dependencyErrors)
            {
                MessageCenter.ShowText(dependencyError);
            }
        }

        private void Update()
        {
            if (MessageCenter._showCounter > 0)
                MessageCenter._showCounter -= Time.deltaTime;
        }

        private GUIStyle _textStyle;
        private void OnGUI()
        {
            if (MessageCenter._showCounter <= 0) return;

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

            if (MessageCenter._showCounter <= 1)
            {
                textColor.a = MessageCenter._showCounter;
                outlineColor.a = MessageCenter._showCounter;
            }

            ShadowAndOutline.DrawOutline(new Rect(40, 20, Screen.width - 80, 160), MessageCenter._shownLogText, _textStyle, textColor, outlineColor, 3);
        }
    }
}
