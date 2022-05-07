using System;
using TaikoFlip;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour
{
    private TaikoNote _note;
    [SerializeField] private Image _background;
    public Color DonColor;
    public Color KaColor;
    public const float Speed = 1000;
    public static float TimeToReachHit => 3000 / Speed;
    [SerializeField] private AudioClip[] _hitSounds;
    private Transform _transform;

    private float a;

    private void Awake()
    {
        _transform = transform;
        a = Time.time;
    }

    private void Update()
    {
        if (_note == null) return;
        
        // change x position
        Vector3 localPosition = _transform.localPosition;
        float newX = localPosition.x + Time.deltaTime * -Speed;
        localPosition = new Vector3(newX, localPosition.y, localPosition.z);
        _transform.localPosition = localPosition;


        if (_transform.localPosition.x < -1000)
        {
            print(Time.time - a);
            foreach(HitSound hs in _note.HitSounds)
            {
                AudioSource.PlayClipAtPoint(_hitSounds[(int)hs], Camera.main!.transform.position);
            }
            Destroy(gameObject);
        }
    }

    public void SetNote(TaikoNote newNote)
    {
        _note = newNote;
        _background.color = _note.Type switch
        {
            NoteType.Don => DonColor,
            NoteType.Ka => KaColor,
            _ => _background.color,
        };
    }
}