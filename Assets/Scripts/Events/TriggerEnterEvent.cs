using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Maze/Trigger Enter Event")]
    public class TriggerEnterEvent : ScriptableObject
    {
        public event Action OnPlayerEnter;

        public void PlayerEntered()
        {
            OnPlayerEnter?.Invoke();
        }
    }
}