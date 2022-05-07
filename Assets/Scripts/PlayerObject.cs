using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaikoFlip
{
    public class PlayerObject : MonoBehaviour
    {
        [SerializeField] private Transform _noteContainer;
        [SerializeField] private GameObject _notePrefab;
        private readonly Vector3 _spawnPosition = new Vector3(2000, 0, 0);

        

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