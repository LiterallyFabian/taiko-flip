using System.Collections;
using UnityEngine;

namespace TaikoFlip
{
    public class PlayerObject : MonoBehaviour
    {
        [SerializeField] private Transform _noteContainer;
        [SerializeField] private GameObject _notePrefab;
        private readonly Vector3 _spawnPosition = new Vector3(2000, 0, 0);

        [Header("1-2p config")]
        [SerializeField] private KeyCode _centreLeft;
        [SerializeField] private KeyCode _centreRight;
        [SerializeField] private KeyCode _rimLeft;
        [SerializeField] private KeyCode _rimRight;
        
        [Header("3-4p config")]
        [SerializeField] private KeyCode _centre;
        [SerializeField] private KeyCode _rim;

        public void StartBeatmap(TaikoBeatmap beatmap)
        {
            foreach (TaikoObject note in beatmap.Objects)
            {            print("Spawning note");

                StartCoroutine(QueueNote((TaikoNote)note));
            }
        }

        private IEnumerator QueueNote(TaikoNote note)
        {
            yield return new WaitForSeconds(note.Time/1000f);
            SpawnNote(note);
        }
        
        public void SpawnNote(TaikoNote note)
        {
            NoteObject noteObject = Instantiate(_notePrefab, _noteContainer).GetComponent<NoteObject>();
            noteObject.transform.localPosition = _spawnPosition;
            noteObject.SetNote(note);
        }
    }
}