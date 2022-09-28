using System;
using UnityEngine;
using UnityEngine.Events;

namespace VVVVVV.World.Entity
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EntityCollider : MonoBehaviour
    {
        public int CollideTargetLayer = -1;

        public bool OnWallLeft { get; private set; }
        public bool OnWallRight { get; private set; }
        public bool OnGround { get; private set; }
        public bool OnRoof { get; private set; }
        protected int lastContactPointSize;

        public UnityEvent CollisionEvent;

        void Awake()
        {
            if (CollideTargetLayer == -1)
                CollideTargetLayer = LayerMask.NameToLayer("wall");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer != CollideTargetLayer) return;
            UpdateTouchStatus(collision.contacts);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer != CollideTargetLayer) return;
            UpdateTouchStatus(collision.contacts);
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (name != "Player") return;

            if (collision.collider.gameObject.layer != CollideTargetLayer) return;

            if (lastContactPointSize != collision.contacts.Length)
                UpdateTouchStatus(collision.contacts);
        }


        protected void UpdateTouchStatus(ContactPoint2D[] contacts)
        {
            // disable all status 
            OnWallLeft = OnWallRight = OnGround = OnRoof = false;

            // Enable each
            foreach (var c in contacts)
            {
                var n = c.normal;
                // sometimes not perfect one
                var nx = Math.Round(n.x, 2);
                var ny = Math.Round(n.y, 2);
                if (nx == +1) OnWallLeft = true;
                if (nx == -1) OnWallRight = true;
                if (ny == +1) OnGround = true;
                if (ny == -1) OnRoof = true;
            }

            lastContactPointSize = contacts.Length;
            CollisionEvent.Invoke();
        }
    }
}
