using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.World
{
    public class Minimap : MonoBehaviour, ISerializable
    {
        public string SerializeKey { get => "minimap"; }

        [SerializeField] Text roomnameText;

        private List<Room> rooms;
        public Room CurRoom { get; private set; }
        public Room room(Vector2Int pos) => rooms.Find(x => x.pos == pos);

        public HashSet<(int, int)> explored { get; private set; } = new HashSet<(int, int)>(); // use tuple only for serializable 

        void Awake()
        {
            this.rooms = GameObject.Find("World").GetComponentsInChildren<Room>(true).ToList();
        }

        public void ChangeRoom(Vector2Int pos)
        {
            var newRoom = room(pos);

            // if Area Changed 
            if (CurRoom != null && CurRoom.area != newRoom.area)
            {
                CurRoom.transform.parent.gameObject.SetActive(false);
                newRoom.transform.parent.gameObject.SetActive(true);
            }

            CurRoom = newRoom;
            roomnameText.text = CurRoom.name;

            EnableRoom();
            SetExplored(pos);
        }

        void EnableRoom()
        {
            // Disable all 
            foreach (var r in rooms)
                r.gameObject.SetActive(false);

            CurRoom.gameObject.SetActive(true);
        }

        public void SetExplored(Vector2Int r, bool status = true)
        {
            if (status)
                explored.Add((r.x, r.y));
            else
                explored.Remove((r.x, r.y));

            var minimaptabs = GameObject.Find("Canvas").GetComponentsInChildren<UI.MinimapTab>();
            foreach (var tab in minimaptabs)
                tab.UpdateFog();
        }

        public string Save() => SaveManager.SerializableObject(explored);
        public void Load(string str)
        {
            if (str == "") return;

            explored = SaveManager.DeserializeObject<HashSet<(int, int)>>(str);
        }

        public void ShowShip()
        {
            SetExplored(new Vector2Int(2, 10));
            SetExplored(new Vector2Int(3, 10));
            SetExplored(new Vector2Int(4, 10));
            SetExplored(new Vector2Int(2, 11));
            SetExplored(new Vector2Int(3, 11));
            SetExplored(new Vector2Int(4, 11));
        }
        public void HideShip()
        {
            SetExplored(new Vector2Int(2, 10), false);
            SetExplored(new Vector2Int(3, 10), false);
            SetExplored(new Vector2Int(4, 10), false);
            SetExplored(new Vector2Int(2, 11), false);
            SetExplored(new Vector2Int(3, 11), false);
            SetExplored(new Vector2Int(4, 11), false);
        }
    }
}
