using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VVVVVV.World
{
    public class EventTriggerManager : MonoBehaviour, ISerializable
    {
        public string Serialize()
        {
            string GetPath(GameObject obj)
            {
                string path = "/" + obj.name;
                while (obj.transform.parent != null)
                {
                    obj = obj.transform.parent.gameObject;
                    path = "/" + obj.name + path;
                }
                return path;
            }

            var triggers = GameObject.Find("World").GetComponentsInChildren<EventTrigger>(true);
            Dictionary<string, bool> data = triggers.ToDictionary(x => GetPath(x.gameObject), x => x.Excuted);
            return Utils.SerializeHelper.SerializeObject(data);
        }

        public void LoadSerializedData(string str)
        {
            var data = Utils.SerializeHelper.DeserializeObject<Dictionary<string, bool>>(str);
            foreach (var x in data)
                GameObject.Find(x.Key).GetComponent<EventTrigger>().Excuted = x.Value;
        }
    }
}