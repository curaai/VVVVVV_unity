using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VVVVVV.UI
{
    public class Minimap : MonoBehaviour
    {
        public static readonly string SerializeKey = "minimap";

        [SerializeField] Image fogMaskPanel;
        [SerializeField] Texture2D fogTile;

        public Room room { get; private set; }
        public Vector2Int RoomPos => room.pos;
        public GameObject RoomObj { get; private set; }
        private Dictionary<Vector2Int, Room> rooms;

        public Vector2Int LocalRoomPos => new Vector2Int(RoomPos.x - 100, RoomPos.y - 100);

        // use tuple only for serializable 
        HashSet<(int, int)> explored = new HashSet<(int, int)>();

        void Start()
        {
            this.rooms = Resources.LoadAll<Room>("Tables/Rooms").ToDictionary(x => x.pos);

            var loaded = (HashSet<(int, int)>)Utils.PlayerPrefsSerializer.Deserialize(SerializeKey);
            if (loaded != null)
                explored = loaded;

            ChangeRoom(new Vector2Int(115, 105));
        }

        void Update()
        {
            var pressEnter = Input.GetKeyDown(KeyCode.Return);
            if (pressEnter)
                GameObject.Find("MapPanel").GetComponent<UI.SlidePanel>().Toggle();
        }

        public void ChangeRoom(Vector2Int pos)
        {
            room = rooms[pos];
            Debug.Log("Room Changed" + RoomPos.ToString());

            UpdateUI();
            Exploring(RoomPos);
        }

        void UpdateUI()
        {
            void Grid()
            {
                var root = GameObject.Find("Grid");
                foreach (Transform t in root.transform)
                    t.gameObject.SetActive(false);

                RoomObj = root.transform.Find(room.name).gameObject;
                RoomObj.SetActive(true);
            }

            void Name()
            {
                var text = GameObject.Find("RoomName").GetComponent<UI.GlowText>();
                text.originalText = room.name;
            }

            Name();
            Grid();
        }

        void Exploring(Vector2Int r)
        {
            void UpdateFog()
            {
                (int rw, int rh) = (fogTile.width, fogTile.height);
                var tex = new Texture2D(rw * 20, rh * 20);
                tex.alphaIsTransparency = true;
                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 20; x++)
                    {
                        var pixels = fogTile.GetPixels();
                        // reverse y for coordnation
                        if (explored.Contains((x + 100, (19 - y) + 100)))
                        {
                            pixels = Enumerable.Repeat(new Color(0, 0, 0, 0), rw * rh).ToArray();
                            Debug.Log("Opened room" + (x, y).ToString());
                        }

                        tex.SetPixels(x * rw, y * rh, rw, rh, pixels);
                    }
                }
                tex.Apply(false);
                var newMask = Sprite.Create(tex, new Rect(0, 0, 240, 180), Vector2.one / 2f);
                fogMaskPanel.sprite = newMask;
            }
            void Save()
            {
                var serilized = Utils.PlayerPrefsSerializer.Serializable(explored);
                PlayerPrefs.SetString(SerializeKey, serilized);
            }

            var len = explored.Count;
            explored.Add((r.x, r.y));

            UpdateFog();
            Save();
        }

        public void Save()
        {
            var serilized = Utils.PlayerPrefsSerializer.Serializable(explored);
            PlayerPrefs.SetString(SerializeKey, serilized);
        }
    }
}
