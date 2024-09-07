using System.Reflection;
using BepInEx;

[assembly: AssemblyTitle(ProcessorAffinityOverridePatcher.PatcherName)]
[assembly: AssemblyProduct(ProcessorAffinityOverridePatcher.GUID)]
[assembly: AssemblyDescription("BepInEx IL2CPP preloader patcher that finds and replaces a byte pattern in current process memory.")]

[assembly: AssemblyVersion(ProcessorAffinityOverridePatcher.PatcherVersion)]
[assembly: AssemblyFileVersion(ProcessorAffinityOverridePatcher.PatcherVersion)]
