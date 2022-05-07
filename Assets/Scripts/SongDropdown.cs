using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TaikoFlip
{
    public class SongDropdown : MonoBehaviour
    {
        private Dropdown _dropdown;
        [SerializeField] private string[] _paths;
        private string[] _cleanPaths;
        
        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _dropdown.ClearOptions();
            _cleanPaths = new string[_paths.Length];
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData> {new Dropdown.OptionData()};
            for (int i = 0; i < _paths.Length; i++)
            {
                _cleanPaths[i] = _paths[i].Replace("Assets/Resources/", "").Replace(".osu", "");
                TaikoBeatmap beatmap = TaikoBeatmap.Parse(_cleanPaths[i]);
                GameManager.Beatmaps.Add(beatmap);
                options.Add(new Dropdown.OptionData(beatmap.Artist + " - " + beatmap.Title));
            }

            _dropdown.AddOptions(options);
        }
    }
}