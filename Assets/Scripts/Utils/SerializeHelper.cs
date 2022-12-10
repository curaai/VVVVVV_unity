using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VVVVVV.Utils
{
    public class SerializeHelper
    {
        public static string SerializeObject(object serializableObject)
        {
            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, serializableObject);
            return System.Convert.ToBase64String(ms.ToArray());
        }

        public static T DeserializeObject<T>(string serializeStr)
        {
            var ms = new MemoryStream(System.Convert.FromBase64String(serializeStr));
            return (T)new BinaryFormatter().Deserialize(ms);
        }
    }
}