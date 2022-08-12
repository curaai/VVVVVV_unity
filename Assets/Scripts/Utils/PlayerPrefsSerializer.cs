using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VVVVVV.Utils
{
    public class PlayerPrefsSerializer
    {
        public static BinaryFormatter bf = new BinaryFormatter();
        // serializableObject is any struct or class marked with [Serializable]
        public static string Serializable(object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            bf.Serialize(memoryStream, serializableObject);
            return System.Convert.ToBase64String(memoryStream.ToArray());
        }

        public static object Deserialize(string prefKey)
        {
            string tmp = PlayerPrefs.GetString(prefKey, string.Empty);
            if (tmp == string.Empty)
                return null;
            MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(tmp));
            return bf.Deserialize(memoryStream);
        }
    }
}
