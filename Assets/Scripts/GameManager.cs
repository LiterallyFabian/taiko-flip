using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TaikoFlip
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Image _background;
        
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
        }
        
        public void SetBackground(Sprite sprite)
        {
            _background.sprite = sprite;
        }
    }
}