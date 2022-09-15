using System;
using UnityEngine;
using UnityEngine.Events;

namespace VVVVVV.Utils
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEventTrigger : MonoBehaviour
    {
        public UnityEvent events;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
                events.Invoke();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                events.Invoke();
        }
    }
}