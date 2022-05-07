using System;
using UnityEngine;

namespace TaikoFlip
{
    public class test : MonoBehaviour
    {
        TaikoBeatmap beatmap;

        private void Start()
        {
            beatmap = TaikoBeatmap.Parse("Beatmaps\\571171 Sayuri - Heikousen\\Sayuri - Heikousen (Charlotte) [Futsuu]");
            
            print(beatmap);
        }
    }
}