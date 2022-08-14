using System;
using UnityEngine;

namespace VVVVVV.World
{
    public class Clock : MonoBehaviour, ISerializable
    {
        public TimeSpan curPlaytime { get; private set; }
        public TimeSpan savetime { get; private set; }

        public string SerializeKey => "clock";

        void Update()
        {
            curPlaytime += TimeSpan.FromSeconds(Time.deltaTime);
        }

        public void Load(string str)
        {
            if (str != null && str != "")
            {
                curPlaytime = TimeSpan.Parse(str);
                savetime = curPlaytime;
            }
        }

        public string Save()
        {
            savetime = curPlaytime;
            return curPlaytime.ToString();
        }

        public static string FormatString(TimeSpan t) => $"{t:m\\:ss}";
    }
}