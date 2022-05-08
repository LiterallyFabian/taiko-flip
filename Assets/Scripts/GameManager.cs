using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TaikoFlip
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private AudioSource _music;
        [SerializeField] private GameObject[] _players;
        [SerializeField] private GameObject _canvas;
        public TaikoBeatmap Beatmap;
        public static List<TaikoBeatmap> Beatmaps = new List<TaikoBeatmap>();
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
            
            SetPlayerCount(1);
        }
        
        public void SetPlayerCount(int count)
        {
            PlayerCount = count;
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].SetActive(i < count);
            }
        }

        public void StartBeatmapWrapper(TaikoBeatmap beatmap)
        {
            Beatmap = beatmap;
            StartCoroutine(StartBeatmap(beatmap));
        }

        private IEnumerator StartBeatmap(TaikoBeatmap beatmap)
        {
            _background.sprite = beatmap.Background;
            _canvas.SetActive(false);

            foreach (PlayerObject p in FindObjectsOfType<PlayerObject>())
            {
                p.StartBeatmap(beatmap);
            }
            yield return new WaitForSecondsRealtime(NoteObject.TimeToReachHit);
            _music.clip = beatmap.Music;
            _music.Play();
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}