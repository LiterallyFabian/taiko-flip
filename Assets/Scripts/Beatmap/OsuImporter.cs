using UnityEngine;
using System.IO;
using UnityEditor.AssetImporters;

namespace TaikoFlip
{
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
}