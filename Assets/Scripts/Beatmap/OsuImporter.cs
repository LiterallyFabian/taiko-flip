using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor.AssetImporters;
#endif
namespace TaikoFlip
{
#if UNITY_EDITOR
    [ScriptedImporter(1, "osu")]
    public class OsuImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            TextAsset subAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("text", subAsset);
            ctx.SetMainObject(subAsset);
        }
    }
#endif
}