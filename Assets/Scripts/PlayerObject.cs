using System;
using UnityEngine;

namespace TaikoFlip
{
    public class PlayerObject : MonoBehaviour
    {
        [SerializeField] private Transform _noteContainer;
        [SerializeField] private GameObject _notePrefab;
        
        private void Start()
        {
            SpawnNote();
        }

        public void SpawnNote()
        {
            GameObject note = Instantiate(_notePrefab, _noteContainer);
            
        }
    }
}