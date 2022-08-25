using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
            string number2word(int n)
            {
                var words = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty" };
                if (20 < n) throw new InvalidCastException("Cannot convert number to word: " + n.ToString());
                return words[n];
            }

            trinkets.Where(x => x == t).First().Collected = true;
            trinketUI.SetActive(true);
            var textUi = trinketUI.transform.Find("Count").GetComponent<Text>();
            textUi.text = $" {number2word(Collections)} out of twenty";
        }

        public void Load(string str)
        {
            if (str == "") return;

            var arr = SaveManager.DeserializeObject<bool[]>(str);
            for (int i = 0; i < trinkets.Length; i++)
                trinkets[i].Collected = arr[i];
        }

        public string Save()
        {
            var arr = new bool[TOTAL_TRINKET_COUNT];
            foreach (var i in Enumerable.Range(0, trinkets.Length))
                arr[i] = trinkets[i].Collected;
            return SaveManager.SerializableObject(arr);
        }

        public int Collections => trinkets.Where(x => x.Collected).ToList().Count;
    }
}