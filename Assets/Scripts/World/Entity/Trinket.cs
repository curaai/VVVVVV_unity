using System;
using UnityEngine;

namespace VVVVVV.World.Entity
{
    public class Trinket : MonoBehaviour
    {
        [SerializeField] public int Idx;
        public bool Collected = false;

        private TrinketManager manager => GameObject.Find("TrinketManager").GetComponent<TrinketManager>();

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (Collected) return;

            manager.Collect(this);
        }
    }
}