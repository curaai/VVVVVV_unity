using System;
using UnityEngine;
using VVVVVV.Utils;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World.Entity
{
    public class DisappearingPlatform : MonoBehaviour
    {

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                var anim = GetComponent<Animator>();
                anim.SetTrigger("Collide");
            }
        }

        public void Disppear()
        {
            Destroy(gameObject);
        }
    }
}
