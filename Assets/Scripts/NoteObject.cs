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
    public static float TimeToReachHit => Speed / 444.44f;
    

    private void Update()
    {
        transform.Translate(Vector3.left * Speed * Time.deltaTime);
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