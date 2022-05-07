using System.Collections;
using System.Collections.Generic;
using TaikoFlip;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour
{
    private TaikoNote _note;
    [SerializeField] private Image _background;
    public Color DonColor;
    public Color KaColor;
    
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