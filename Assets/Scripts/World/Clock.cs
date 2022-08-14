using System;
using UnityEngine;

namespace VVVVVV.World
{
    public class Clock : MonoBehaviour, ISerializable
    {
        public TimeSpan playtime { get; private set; }

        public string SerializeKey => "clock";

        void Update()
        {
            playtime += TimeSpan.FromSeconds(Time.deltaTime);
        }

        public void Load(string str)
        {
            if (str != null)
                playtime = TimeSpan.Parse(str);
        }

        public string Save()
        {
            return playtime.ToString();
        }
    }
}