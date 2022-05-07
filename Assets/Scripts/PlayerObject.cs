using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TaikoFlip
{
    public class PlayerObject : MonoBehaviour
    {
        [SerializeField] private DrumObject _drum;
        [SerializeField] private AudioClip _hitCenter;
        [SerializeField] private AudioClip _hitRim;
        
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
            {            

                StartCoroutine(QueueNote((TaikoNote)note));
            }
        }

        private void Update()
        {
            if (GameManager.Instance.PlayerCount < 3)
            {
                if (Input.GetKeyDown(_centreLeft))
                {
                    AudioSource.PlayClipAtPoint(_hitCenter, Vector3.zero);
                    Slam(true);
                }
                if (Input.GetKeyDown(_centreRight))
                {
                    AudioSource.PlayClipAtPoint(_hitCenter, Vector3.zero);
                    Slam(true);
                }
                if (Input.GetKeyDown(_rimLeft))
                {
                    AudioSource.PlayClipAtPoint(_hitRim, Vector3.zero);
                    Slam(false);
                }
                if (Input.GetKeyDown(_rimRight))
                {
                    AudioSource.PlayClipAtPoint(_hitRim, Vector3.zero);
                    Slam(false);
                }

                _drum.CentreL.enabled = Input.GetKey(_centreLeft);
                _drum.CentreR.enabled = Input.GetKey(_centreRight);
                _drum.RimL.enabled = Input.GetKey(_rimLeft);
                _drum.RimR.enabled = Input.GetKey(_rimRight);
            }
        }

        /// <summary>
        /// Slam the drum with the given input (true = centre, false = rim)
        /// </summary>
        /// <param name="centre">True if this is a center hit, false if rim.</param>
        private void Slam(bool centre)
        {
            NoteObject closestNote = GetClosestNote();
            if (closestNote == null) return;

            if (centre)
            {
                if (closestNote.Note.Type == NoteType.Don)
                {
                    closestNote.Hit();
                }
                else
                {
                    closestNote.Miss();
                }
            }
            else
            {
                if (closestNote.Note.Type == NoteType.Ka)
                {
                    closestNote.Hit();
                }
                else
                {
                    closestNote.Miss();
                }
            }
        }

        private NoteObject GetClosestNote()
        {
            // get note closest to x localPosition -1050
            NoteObject closestNote = null;
            float closestDistance = float.MaxValue;
            foreach (NoteObject note in _noteContainer.GetComponentsInChildren<NoteObject>())
            {
                float distance = Mathf.Abs(note.transform.localPosition.x + 1050);
                if (distance < closestDistance)
                {
                    closestNote = note;
                    closestDistance = distance;
                }
            }

            return closestDistance < 110 ? closestNote : null;
        }
    

        private IEnumerator QueueNote(TaikoNote note)
        {
            yield return new WaitForSeconds(note.Time/1000f);
            SpawnNote(note);
        }
        
        public NoteObject SpawnNote(TaikoNote note)
        {
            NoteObject noteObject = Instantiate(_notePrefab, _noteContainer).GetComponent<NoteObject>();
            noteObject.transform.localPosition = _spawnPosition;
            noteObject.SetNote(note);
            
            return noteObject;
        }
    }
}