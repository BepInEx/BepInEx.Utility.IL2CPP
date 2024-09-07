using System.Reflection;
using BepInEx;

[assembly: AssemblyTitle(ByteFiddlerPatcher.PatcherName)]
[assembly: AssemblyProduct(ByteFiddlerPatcher.GUID)]
[assembly: AssemblyDescription("BepInEx IL2CPP preloader patcher that finds and replaces a byte pattern in current process memory.")]

[assembly: AssemblyVersion(ByteFiddlerPatcher.PatcherVersion)]
[assembly: AssemblyFileVersion(ByteFiddlerPatcher.PatcherVersion)]
