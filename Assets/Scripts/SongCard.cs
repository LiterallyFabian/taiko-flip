using System;
using UnityEngine;
using UnityEngine.UI;

namespace TaikoFlip
{
    public class SongCard : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _text;
        [SerializeField] private string _path;

        private TaikoBeatmap _beatmap;

        private void Awake()
        {
            _beatmap = TaikoBeatmap.Parse(_path);
            
            _image.sprite = _beatmap.Background;
            _text.text = _beatmap.Title;
        }

        public void Play()
        {
            GameManager.Instance.StartBeatmapWrapper(_beatmap);
        }
    }
}