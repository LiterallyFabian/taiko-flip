using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private GameObject _gameOverCanvas;
        [SerializeField] private Transform _scoreBoard;
        [SerializeField] private GameObject _scoreObjectPrefab;
        [SerializeField] private Text _scoreText;
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

            yield return new WaitForSecondsRealtime(_music.clip.length);
            
            _gameOverCanvas.SetActive(true);
            _scoreText.text = "SCORES - " + beatmap.Title.ToUpper();

            List<PlayerObject> players = FindObjectsOfType<PlayerObject>().ToList();
            players.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));

            for (int i = 0; i < players.Count; i++)
            {
                PlayerObject p = players[i];
                GameObject go = Instantiate(_scoreObjectPrefab, _scoreBoard);

                go.GetComponent<ScoreObject>().Placement.text = $"#{i + 1}";
                go.GetComponent<ScoreObject>().PlayerName.text = p.PlayerName.ToUpper();
                go.GetComponent<ScoreObject>().Score.text = $"Score: {p.Score}\nHighest combo: {p.MaxCombo}";
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}