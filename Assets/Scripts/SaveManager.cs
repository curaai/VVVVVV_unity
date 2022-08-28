using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace VVVVVV
{
    public class SaveManager
    {
        private List<ISerializable> list;
        private ISerializable this[string key] => list.Find(x => x.SerializeKey == key);

        public SaveManager(IEnumerable<ISerializable> list)
        {
            this.list = list.ToList();
        }

        public void Save()
        {
            foreach (var x in list)
                Save(x.SerializeKey);
        }
        public void Save(string key)
        {
            PlayerPrefs.SetString(key, this[key].Save());
            PlayerPrefs.Save();
        }
        public void Load()
        {
            foreach (var x in list)
                x.Load(PlayerPrefs.GetString(x.SerializeKey, ""));
        }

        public void Load(string key) => list.Find(x => x.SerializeKey == key).Load(PlayerPrefs.GetString(key, ""));

        // serializableObject is any struct or class marked with [Serializable]
        public static BinaryFormatter bf = new BinaryFormatter();
        public static string SerializableObject(object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            bf.Serialize(memoryStream, serializableObject);
            return System.Convert.ToBase64String(memoryStream.ToArray());
        }

        public static T DeserializeObject<T>(string str)
        {
            MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(str));
            return (T)bf.Deserialize(memoryStream);
        }
    }

    public interface ISerializable
    {
        public string SerializeKey { get; }
        public string Save();
        public void Load(string str);
    }
}