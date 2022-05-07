using System;
using TaikoFlip;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour
{
    public TaikoNote Note { get; private set; }
    [SerializeField] private Image _background;
    public Color DonColor;
    public Color KaColor;
    public const float Speed = 1000;
    public static float TimeToReachHit => 3000 / Speed;
    [SerializeField] private AudioClip[] _hitSounds;
    private Transform _transform;
    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        _transform = transform;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Note == null) return;
        
        // change x position
        Vector3 localPosition = _transform.localPosition;
        float newX = localPosition.x + Time.deltaTime * -Speed;
        localPosition = new Vector3(newX, localPosition.y, localPosition.z);
        _transform.localPosition = localPosition;
        
        if(localPosition.x < -1150) 
            Miss();
    }

    public void SetNote(TaikoNote newNote)
    {
        Note = newNote;
        _background.color = Note.Type switch
        {
            NoteType.Don => DonColor,
            NoteType.Ka => KaColor,
            _ => _background.color,
        };
    }

    public int Hit()
    {
        float distance = Mathf.Abs(transform.position.x);
        foreach(HitSound hs in Note.HitSounds)
        {
            AudioSource.PlayClipAtPoint(_hitSounds[(int)hs], Camera.main!.transform.position);
        }
        
        Destroy(gameObject);
        
        return distance > 50 ? 150 : 300;
    }
    
    public void Miss()
    {
        Destroy(gameObject);
    }
}