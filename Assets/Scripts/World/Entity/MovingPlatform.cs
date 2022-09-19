using System;
using System.Linq;
using UnityEngine;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World.Entity
{
    public class MovingPlatform : MonoBehaviour
    {
        private RectTransform player = null;
        private Transform orignialParent;

        void FixedUpdate()
        {
            if (player != null)
            {
                player.GetComponent<MoveController>().AdditionalVelocity = GetComponent<MoveController>().velocity;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Player")) return;
            if (player != null) return;

            player = collision.collider.transform as RectTransform;
            orignialParent = player.parent;
            player.gameObject.transform.SetParent(gameObject.transform, true);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            player.SetParent(orignialParent);
            player.GetComponent<MoveController>().AdditionalVelocity = Vector2.zero;
            player = null;
        }
    }
}
