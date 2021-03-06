using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        
        [Header("Swtich")]
        [SerializeField] private Image _switchImage;
        [SerializeField] private Sprite _switchOn;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioClip _switchOnSound;
        
        [Header("Drum")]
        [SerializeField] private DrumObject _drum;
        [SerializeField] private AudioClip _hitCenter;
        [SerializeField] private AudioClip _hitRim;
        
        [Header("UI")]
        [SerializeField] private Image _power;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _comboText;
        
        public int Score;
        public int Combo;
        public int MaxCombo;
        private int _hits;
        private TaikoBeatmap _beatmap;
        private int _hitsToFlipTheSwitch => _beatmap == null ? 10000 : (int)(_beatmap.Objects.Count * 0.8);
        public string PlayerName; 

        public bool SwitchFlipped = false;
        

        public void StartBeatmap(TaikoBeatmap beatmap)
        {
            _beatmap = beatmap;
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
                
                _drum.CentreLText.text = _centreLeft.ToString();
                _drum.CentreRText.text = _centreRight.ToString();
                _drum.RimLText.text = _rimLeft.ToString();
                _drum.RimRText.text = _rimRight.ToString();
            }
            else
            {
                if (Input.GetKeyDown(_centre))
                {
                    AudioSource.PlayClipAtPoint(_hitCenter, Vector3.zero);
                    Slam(true);
                }

                if (Input.GetKeyDown(_rim))
                {
                    AudioSource.PlayClipAtPoint(_hitRim, Vector3.zero);
                    Slam(false);
                }

                _drum.CentreL.enabled = false;
                _drum.CentreR.enabled = Input.GetKey(_centre);
                _drum.RimL.enabled = false;
                _drum.RimR.enabled = Input.GetKey(_rim);

                _drum.CentreLText.text = "";
                _drum.CentreRText.text = _centre.ToString();
                _drum.RimLText.text = "";
                _drum.RimRText.text = _rim == KeyCode.RightBracket ? "Å" : _rim.ToString();
            }

            _power.fillAmount = Mathf.Clamp((float) _hits / _hitsToFlipTheSwitch, 0, 1);
            _scoreText.text = Score.ToString();
            _comboText.text = Combo.ToString();

            if (!SwitchFlipped && _hits >= _hitsToFlipTheSwitch)
            {
                SwitchFlipped = true;
                _power.color = Color.red;
                _animator.Play("flippedtheswitch");
                _switchImage.sprite = _switchOn;
                AudioSource.PlayClipAtPoint(_switchOnSound, Vector3.zero);
            }
        }

        /// <summary>
        /// Slam the drum with the given input (true = centre, false = rim)
        /// </summary>
        /// <param name="centre">True if this is a center hit, false if rim.</param>
        private void Slam(bool centre)
        {
            NoteObject closestNote = GetLastNote();
            if (closestNote == null) return;

            if (centre)
            {
                if (closestNote.Note.Type == NoteType.Don)
                {
                    Score += closestNote.Hit() + Combo / 10;
                    Combo++;
                    MaxCombo = Mathf.Max(MaxCombo, Combo);
                    _hits++;
                }
                else
                {
                    closestNote.Miss();
                    Combo = 0;
                }
            }
            else
            {
                if (closestNote.Note.Type == NoteType.Ka)
                {
                    Score += closestNote.Hit() + Combo / 10;
                    Combo++;
                    MaxCombo = Mathf.Max(MaxCombo, Combo);
                    _hits++;
                }
                else
                {
                    closestNote.Miss();
                    Combo = 0;
                }
            }
        }

        private NoteObject GetLastNote()
        {
            // Get the note with the lowest local position X
            NoteObject lastNote = null;
            float lastDistance = float.MaxValue;
            foreach (NoteObject note in GetComponentsInChildren<NoteObject>())
            {
                float distance = note.transform.localPosition.x;
                if (distance < lastDistance)
                {
                    lastNote = note;
                    lastDistance = distance;
                }
            }
            
            return lastDistance < -900 ? lastNote : null;
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
            noteObject.SetNote(note, this);
            
            return noteObject;
        }
    }
}