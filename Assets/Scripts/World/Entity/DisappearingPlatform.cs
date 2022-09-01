using System;
using UnityEngine;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World.Entity
{
    public class DisappearingPlatform : MonoBehaviour
    {

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                GetComponent<Animator>().SetTrigger("Collide");
        }

        public void Disppear()
        {
            gameObject.SetActive(false);
        }
    }
}
