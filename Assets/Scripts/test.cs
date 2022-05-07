using System;
using UnityEngine;

namespace TaikoFlip
{
    public class test : MonoBehaviour
    {
        public TextAsset textAsset;
        TaikoBeatmap beatmap;

        private void Start()
        {
            beatmap = new TaikoBeatmap(textAsset);
            
            print(beatmap);
        }
    }
}