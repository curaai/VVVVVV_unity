using System;
using System.Linq;
using UnityEngine;
using VVVVVV.World.Entity;


namespace VVVVVV.World
{
    public class TrinketManager : MonoBehaviour, ISerializable
    {
        public const int TOTAL_TRINKET_COUNT = 20;
        public string SerializeKey => "Trinkets";
        private const string CLOSE_UI_TRIGGER = "Close";
        [SerializeField] GameObject trinketUI;


        private Trinket[] trinkets;

        void Awake()
        {
            trinkets = GameObject.FindWithTag("World").GetComponentsInChildren<Trinket>();
            trinketUI.SetActive(false);
        }

        void Update()
        {
            if (trinketUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
            {
                var animator = trinketUI.GetComponent<Animator>();
                animator.SetTrigger("Close");
                StartCoroutine(Utils.AnimationHelper.CheckAnimationCompleted(animator, "Close", () => trinketUI.SetActive(false)));
            }
        }

        public void Collect(Trinket t)
        {
            trinketUI.SetActive(true);
        }

        public void Load(string str)
        {
            throw new System.NotImplementedException();
        }

        public string Save()
        {
            throw new System.NotImplementedException();
            var arr = new bool[TOTAL_TRINKET_COUNT];
            foreach (var i in Enumerable.Range(0, trinkets.Length))
                arr[i] = trinkets[i].Collected;
            return SaveManager.SerializableObject(arr);
        }
    }
}