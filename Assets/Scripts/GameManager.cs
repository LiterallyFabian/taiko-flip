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
        public int PlayerCount { get; private set; }
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
                TaikoBeatmap.Parse("Beatmaps/932283 Fujiwara Chika - Chikatto Chika Chika/Fujiwara Chika (CV Kohara Konomi) - Chikatto Chika Chika (TV Size) (zhu) [Nardo's Inner Oni]");
            
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