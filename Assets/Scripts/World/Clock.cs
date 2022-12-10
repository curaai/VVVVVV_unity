using System;
using UnityEngine;

namespace VVVVVV.World
{
    public class Clock : Utils.Singleton<Clock>, ISerializable
    {
        public TimeSpan curPlaytime { get; private set; }
        public TimeSpan savetime { get; private set; }

        void Update()
        {
            curPlaytime += TimeSpan.FromSeconds(Time.deltaTime);
        }

        public string Serialize()
        {
            savetime = curPlaytime;
            return curPlaytime.ToString();
        }

        public void LoadSerializedData(string str)
        {
            if (str != null && str != "")
            {
                curPlaytime = TimeSpan.Parse(str);
                savetime = curPlaytime;
            }
        }

        public static string FormatString(TimeSpan t) => $"{t:m\\:ss}";
    }
}