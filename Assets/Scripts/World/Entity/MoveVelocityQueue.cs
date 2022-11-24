using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VVVVVV.World.Entity
{
    public class MoveVelocityQueue : MonoBehaviour
    {
        [SerializeField] public List<Vector2> VelocityQueue;
        public MoveController TargetController;

        public void Update()
        {
            if (VelocityQueue.Count == 0)
            {
                TargetController.AdditionalVelocity = Vector2.zero;
                enabled = false;
                return;
            }

            var v = VelocityQueue[0];
            TargetController.AdditionalVelocity = v;
            VelocityQueue.RemoveAt(0);
        }
    }
}
