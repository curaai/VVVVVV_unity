using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.World.Entity;


namespace VVVVVV.World
{
    public class TrinketManager : MonoBehaviour, ISerializable
    {
        public string SerializeKey => "Trinkets";
        public const int TOTAL_TRINKET_COUNT = 20;

        [SerializeField] GameObject trinketUI;

        private Trinket[] trinkets;
        public bool HasAnyTrinket => trinkets.Any(x => x.Collected);

        void Awake()
        {
            trinkets = GameObject.FindWithTag("World").GetComponentsInChildren<Trinket>(true);
        }

        public void Collect(Trinket t)
        {
            var target = trinkets.Where(x => x == t).First();
            target.Collected = true;
            target.gameObject.SetActive(false);

            var textUi = trinketUI.transform.Find("Count").GetComponent<Text>();
            textUi.text = CountString();

            GameObject.Find("CutScene").GetComponent<UI.CutSceneController>().Open(trinketUI);
        }

        public void Load(string str)
        {
            if (str == "") return;

            var arr = SaveManager.DeserializeObject<bool[]>(str);
            for (int i = 0; i < trinkets.Length; i++)
            {
                trinkets[i].Collected = arr[i];
                trinkets[i].gameObject.SetActive(!arr[i]);
            }
        }

        public string Save()
        {
            var arr = new bool[TOTAL_TRINKET_COUNT];
            foreach (var i in Enumerable.Range(0, trinkets.Length))
                arr[i] = trinkets[i].Collected;
            return SaveManager.SerializableObject(arr);
        }

        public string CountString()
        {
            var cnt = trinkets.Where(x => x.Collected).Count();

            string number2word(int n)
            {
                var words = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty" };
                if (20 < n) throw new InvalidCastException("Cannot convert number to word: " + n.ToString());
                return words[n];
            }
            return $" {number2word(cnt)} out of Twenty ";
        }
    }
}