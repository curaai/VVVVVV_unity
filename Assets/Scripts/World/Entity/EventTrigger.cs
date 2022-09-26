using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Analytics;

namespace VVVVVV.World
{
    [RequireComponent(typeof(Rigidbody2D))]
    [DisallowMultipleComponent]
    public class EventTrigger : MonoBehaviour
    {
        [Serializable]
        public class Condition : SerializableCallback<bool> { }

        public bool Excuted { get; set; } = false;

        public UnityEvent events;
        public List<Condition> conditions;
        public List<Condition> notConditions;

        public string SerializeKey => throw new NotImplementedException();

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (Excuted) return;

            foreach (var condition in conditions)
                if (!condition.Invoke())
                    return;

            foreach (var nCondition in notConditions)
                if (nCondition.Invoke())
                    return;

            if (collider.gameObject.CompareTag("Player"))
            {
                Debug.Log($"{name} event invoked");
                events.Invoke();
                Excuted = true;
            }
        }
    }
}