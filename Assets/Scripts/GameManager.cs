using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TaikoFlip
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private AudioSource _music;
        public TaikoBeatmap Beatmap;
        
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Beatmap =
                TaikoBeatmap.Parse("Beatmaps/879516 Koga Tomoe (CV_ Touyama Nao) - Fukashigi no Carte (TV Size) [no video]/Koga Tomoe (CV Touyama Nao) - Fukashigi no Carte (TV Size) (zhu) [- Fapu's Oni -]");
            
            print($"Playing {Beatmap}");
            StartCoroutine(StartBeatmap(Beatmap));
        }

        public IEnumerator StartBeatmap(TaikoBeatmap beatmap)
        {
            _background.sprite = beatmap.Background;

            foreach (PlayerObject p in FindObjectsOfType<PlayerObject>())
            {
                p.StartBeatmap(beatmap);
            }

            yield return new WaitForSeconds(NoteObject.TimeToReachHit);
            _music.clip = beatmap.Music;
            _music.Play();
        }
    }
}