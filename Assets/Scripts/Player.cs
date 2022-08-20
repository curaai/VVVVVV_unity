using System;
using UnityEngine;
using VVVVVV.World.Entity;


namespace VVVVVV
{
    [RequireComponent(typeof(Animator), typeof(Collider2D), typeof(MoveController))]
    public class Player : MonoBehaviour
    {
        const string DAMAGABLE_TAG = "Damagable";

        Animator animator;
        MoveController controller;

        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<MoveController>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(DAMAGABLE_TAG))
            {
                animator.SetBool("HurtNow", true);
                GameOver();
            }
        }

        public void GameOver()
        {
            GetComponent<Collider2D>().isTrigger = true;
            controller.force = Vector2.zero;
            controller.velocity = Vector2.zero;
        }
    }
}