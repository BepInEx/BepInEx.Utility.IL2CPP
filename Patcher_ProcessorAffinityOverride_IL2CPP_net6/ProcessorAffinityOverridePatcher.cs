using System;
using System.Diagnostics;
using BepInEx.Preloader.Core.Patching;

namespace BepInEx
{
    [PatcherPluginInfo(GUID, PatcherName, PatcherVersion)]
    public class ProcessorAffinityOverridePatcher : BasePatcher
    {
        public const string GUID = "ProcessAffinityOverride";
        public const string PatcherName = "Process Affinity Override";
        public const string PatcherVersion = "1.0";

        public override void Initialize()
        {
            var currentProcess = Process.GetCurrentProcess();
            var currentAffinity = (long)currentProcess.ProcessorAffinity;
            var currentStr = Convert.ToString(currentAffinity, 2);
            var cfgStr = Config.Bind("Override", "ProcessorAffinity", currentStr, "A byte field representing which processors this process can run on (e.g. 0000 1101 1111).").Value.Replace(" ", "");
            try
            {
                var cfgAffinity = (long)Convert.ToUInt64(cfgStr, 2);
                if (cfgAffinity == 0)
                    throw new Exception();
                if (cfgAffinity != currentAffinity)
                {
                    currentProcess.ProcessorAffinity = (IntPtr)cfgAffinity;
                    Log.LogInfo($"Processor affinity changed from {currentStr} to {cfgStr}");
                }
            }
            catch (Exception e)
            {
                Log.LogFatal($"Invalid setting format, it has to be a byte field (e.g. 0000 1101 1111). Value: {cfgStr}\n{e}");
            }
        }
    }
}
