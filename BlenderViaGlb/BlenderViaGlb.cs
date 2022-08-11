using HarmonyLib;
using NeosModLoader;
using BaseX;
using CodeX;
namespace BlenderViaGlb
{
    public class BlenderViaGlb : NeosMod
    {
        public override string Name => "BlenderViaGlb";
        public override string Author => "eia485";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/EIA485/NeosBlenderViaGlb/";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.eia485.BlenderViaGlb");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(BlenderInterface), "ExportToGLTF")]
        private class BlenderViaGlbPatch
        {
            public static bool Prefix(string input, string output)
            {
                output = output.Replace("/", "\\").Replace("\\", "\\\\");
                output = output.EscapeUnicodeCharacters();
                typeof(BlenderInterface).GetMethod("RunScript").Invoke(null,new object[] { "import bpy\r\nbpy.ops.export_scene.glb(filepath=u\"" + output + "\")", "\"" + input + "\" --background --python \"{0}\"" });
                return false;
            }
        }
    }
}