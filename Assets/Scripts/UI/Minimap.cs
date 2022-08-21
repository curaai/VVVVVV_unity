using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.UI.Utils;

namespace VVVVVV.UI
{
    public class Minimap : MonoBehaviour, ISerializable
    {
        public string SerializeKey { get => "minimap"; }
        public static readonly float BlinkFastDuration = 2f;

        [SerializeField] GameObject FocusBlink;
        [SerializeField] Image fogMaskPanel;
        [SerializeField] Texture2D fogTile;

        private List<Room> rooms;

        HashSet<(int, int)> explored = new HashSet<(int, int)>(); // use tuple only for serializable 

        void Awake()
        {
            this.rooms = Resources.LoadAll<Room>("Tables/Rooms").ToList();
        }

        void Start()
        {
            ChangeRoom(new Vector2Int(115, 103));
        }

        public Room room(Vector2Int pos) => rooms.Find(x => x.pos == pos);
        public Room room(string name) => rooms.Find(x => x.name == name);
        public GameObject roomObj(Room r) => GameObject.Find("Grid").transform.Find(r.name).gameObject;

        public void ChangeRoom(Vector2Int pos)
        {
            Debug.Log("Room Changed" + pos.ToString());

            UpdateUI(room(pos));
            Exploring(pos);
        }

        void UpdateUI(Room r)
        {
            void Name()
            {
                var text = GameObject.Find("RoomName").GetComponent<Text>();
                text.text = r.name;
            }

            void SetBlinkPos()
            {
                var z = FocusBlink.transform.localPosition.z;
                var localPos = r.pos - Vector2.one * 100;
                FocusBlink.transform.localPosition = new Vector3(24 * localPos.x, -(18 * localPos.y), z);
            }

            Name();
            SetBlinkPos();
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
                            pixels = Enumerable.Repeat(new Color(0, 0, 0, 0), rw * rh).ToArray();

                        tex.SetPixels(x * rw, y * rh, rw, rh, pixels);
                    }
                }
                tex.Apply(false);
                var newMask = Sprite.Create(tex, new Rect(0, 0, 240, 180), Vector2.one / 2f);
                fogMaskPanel.sprite = newMask;
            }

            var len = explored.Count;
            explored.Add((r.x, r.y));

            UpdateFog();
        }

        public void OpenMinimapTab(int tabIdx)
        {
            IEnumerator SetBlinkSlowAnim()
            {
                yield return new WaitForSeconds(BlinkFastDuration);
                FocusBlink.GetComponent<Animator>().SetTrigger("3 Seconds Later");
                yield return null;
            }

            // TODO : Replace to Constant? 
            if (tabIdx != 0)
                return;

            StartCoroutine(SetBlinkSlowAnim());
        }

        public string Save()
        {
            return SaveManager.SerializableObject(explored);
        }

        public void Load(string str)
        {
            if (str == "")
                return;

            explored = SaveManager.DeserializeObject<HashSet<(int, int)>>(str);
        }
    }
}
