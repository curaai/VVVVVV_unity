using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VVVVVV.World
{
    [RequireComponent(typeof(Rigidbody2D))]
    [DisallowMultipleComponent]
    public class EventTrigger : MonoBehaviour
    {
        public bool Excuted = false;

        public UnityEvent events;

        public string SerializeKey => throw new NotImplementedException();

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (Excuted) return;

            Excuted = true;
            if (collider.gameObject.CompareTag("Player"))
                events.Invoke();
        }
    }
}