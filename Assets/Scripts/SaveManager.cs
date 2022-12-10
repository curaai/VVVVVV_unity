using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace VVVVVV
{
    public class SaveManager
    {
        private List<ISerializable> list;

        public SaveManager(IEnumerable<ISerializable> list)
        {
            this.list = list.ToList();
        }

        public void Save()
        {
            foreach (var item in list)
                PlayerPrefs.SetString(getPrefKey(item), item.Serialize());
            PlayerPrefs.Save();
        }

        public void Load(ISerializable item)
        {
            var loadData = PlayerPrefs.GetString(getPrefKey(item), "");
            if (loadData != "" && list.Contains(item))
                item.LoadSerializedData(loadData);
        }

        public void LoadAll()
        {
            foreach (var serializable in list)
            {
                var serializeData = PlayerPrefs.GetString(getPrefKey(serializable), "");
                if (serializeData != "")
                    serializable.LoadSerializedData(serializeData);
            }
        }

        private string getPrefKey(ISerializable obj) => obj.GetType().Name;
    }
    public interface ISerializable
    {
        public string Serialize();
        public void LoadSerializedData(string str);
    }
}