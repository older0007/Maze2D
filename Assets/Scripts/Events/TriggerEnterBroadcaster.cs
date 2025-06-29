using System;
using UnityEngine;

namespace Events
{
    public class TriggerEnterBroadcaster : MonoBehaviour
    {
        [SerializeField] private TriggerEnterEvent enterEvent;
        private void OnTriggerEnter2D(Collider2D other)
        {
            enterEvent.PlayerEntered();
        }
    }
}