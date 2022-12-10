using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.World
{
    public class Minimap : Utils.SingletonMonoBehaviour<Minimap>, ISerializable
    {
        [SerializeField] Text roomnameText;

        private List<Room> rooms;
        public Room CurRoom { get; private set; }
        public Room GetRoom(Vector2Int pos) => rooms.Find(x => x.pos == pos);

        private HashSet<(int, int)> explored = new HashSet<(int, int)>(); // use tuple only for serializable 

        private UI.MinimapTab uiMinimap;

        void Awake()
        {
            this.rooms = GameObject.Find("World").GetComponentsInChildren<Room>(true).ToList();
            uiMinimap = GameObject.Find("Canvas").GetComponentInChildren<UI.MinimapTab>();
        }

        public void ChangeRoom(Vector2Int pos)
        {
            var newRoom = GetRoom(pos);

            // if Area Changed 
            if (CurRoom != null && CurRoom.area != newRoom.area)
            {
                CurRoom.transform.parent.gameObject.SetActive(false);
                newRoom.transform.parent.gameObject.SetActive(true);
            }

            CurRoom = newRoom;
            roomnameText.text = CurRoom.name;

            enableRoom();
            SetExplored(pos);
        }

        private void enableRoom()
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

            uiMinimap.UpdateFog();
        }
        public bool IsExplored(Vector2Int pos) => explored.Contains((pos.x, pos.y));

        public string Serialize() => Utils.SerializeHelper.SerializeObject(explored);
        public void LoadSerializedData(string str)
        {
            if (str == "") return;

            explored = Utils.SerializeHelper.DeserializeObject<HashSet<(int, int)>>(str);
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
