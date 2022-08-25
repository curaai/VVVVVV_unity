using System;
using UnityEngine;

namespace VVVVVV.World.Entity
{
    public class Trinket : MonoBehaviour
    {
        [SerializeField] public int Idx;
        private TrinketManager manager => GameObject.Find("TrinketManager").GetComponent<TrinketManager>();

        private bool _collected;
        public bool Collected
        {
            get => _collected;
            set
            {
                _collected = value;
                GetComponent<SpriteRenderer>().enabled = !Collected;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (Collected) return;

            manager.Collect(this);
        }
    }
}