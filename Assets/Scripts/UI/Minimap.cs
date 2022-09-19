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
        [SerializeField] Text roomnameText;

        private List<Room> rooms;

        HashSet<(int, int)> explored = new HashSet<(int, int)>(); // use tuple only for serializable 

        void Awake()
        {
            this.rooms = Resources.LoadAll<Room>("Tables/Rooms").ToList();
        }

        public Room room(Vector2Int pos) => rooms.Find(x => x.pos == pos);
        public Room room(string name) => rooms.Find(x => x.name == name);
        public GameObject roomObj(Room r) => GameObject.Find("Grid").transform.Find(r.name).gameObject;

        public void ChangeRoom(Vector2Int pos)
        {
            Debug.Log("Room Changed" + pos.ToString());

            UpdateUI(room(pos));
            SetExplored(pos);
        }

        void UpdateUI(Room r)
        {
            void SetBlinkPos()
            {
                var z = FocusBlink.transform.localPosition.z;
                var localPos = r.pos - Vector2.one * 100;
                FocusBlink.transform.localPosition = new Vector3(24 * localPos.x, -(18 * localPos.y), z);
            }

            roomnameText.text = r.name;
            SetBlinkPos();
        }

        public void SetExplored(Vector2Int r, bool status = true)
        {
            r -= Vector2Int.one * 100;
            if (status)
                explored.Add((r.x, r.y));
            else
                explored.Remove((r.x, r.y));

            UpdateFog();
        }

        private void UpdateFog()
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
                    if (explored.Contains((x, 19 - y)))
                        pixels = Enumerable.Repeat(new UnityEngine.Color(0, 0, 0, 0), rw * rh).ToArray();

                    tex.SetPixels(x * rw, y * rh, rw, rh, pixels);
                }
            }
            tex.Apply(false);
            var newMask = Sprite.Create(tex, new Rect(0, 0, 240, 180), Vector2.one / 2f);
            fogMaskPanel.sprite = newMask;
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
            if (str == "") return;

            explored = SaveManager.DeserializeObject<HashSet<(int, int)>>(str);
        }

        public void ShowShip()
        {
            var dummy = Vector2Int.one * 100;
            SetExplored(new Vector2Int(2, 10) + dummy);
            SetExplored(new Vector2Int(3, 10) + dummy);
            SetExplored(new Vector2Int(4, 10) + dummy);
            SetExplored(new Vector2Int(2, 11) + dummy);
            SetExplored(new Vector2Int(3, 11) + dummy);
            SetExplored(new Vector2Int(4, 11) + dummy);
        }
        public void HideShip()
        {
            var dummy = Vector2Int.one * 100;
            SetExplored(new Vector2Int(2, 10) + dummy, false);
            SetExplored(new Vector2Int(3, 10) + dummy, false);
            SetExplored(new Vector2Int(4, 10) + dummy, false);
            SetExplored(new Vector2Int(2, 11) + dummy, false);
            SetExplored(new Vector2Int(3, 11) + dummy, false);
            SetExplored(new Vector2Int(4, 11) + dummy, false);
        }
    }
}
