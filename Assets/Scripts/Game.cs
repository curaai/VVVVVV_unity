using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV
{
    public class Game : MonoBehaviour
    {
        [SerializeField] public UI.Minimap minimap;
        [SerializeField] public Transform cam;
        [SerializeField] public Transform player;

        public Vector2Int rpos;

        private Dictionary<Vector2Int, Room> rooms;
        public Room room { get; private set; }
        public GameObject roomObj { get; private set; }

        void Start()
        {
            this.rooms = Resources.LoadAll<Room>("Tables/Rooms").ToDictionary(x => x.pos);
            ChangeRoom(new Vector2Int(115, 105));
        }

        void Update()
        {
            Vector2Int? IsRoomChanged()
            {
                var rpos = roomObj.transform.InverseTransformPoint(player.position);
                int xDir = 0;
                if (rpos.x <= -1.25f)
                    xDir = -1;
                else if (38.75f < rpos.x)
                    xDir = 1;

                int yDir = 0;
                if (rpos.y <= -1.5f)
                    yDir = -1;
                else if (28.25f < rpos.y)
                    yDir = 1;

                if (xDir == 0 && yDir == 0)
                    return null;
                else
                    return new Vector2Int(xDir, yDir);
            }

            var newRoomDir = IsRoomChanged();
            if (newRoomDir.HasValue)
            {
                ChangeRoom(room.pos + newRoomDir.Value);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("PausePanel").GetComponent<UI.SlidePanel>().Toggle();
            }
        }

        void ChangeRoom(Vector2Int newRoomPos)
        {
            void AdjustCamPos()
            {
                var camX = room.pos.x * 640;
                var camY = room.pos.y * 480;

                cam.localPosition = new Vector3(camX, camY, cam.localPosition.z);
            }

            void SettingCurRoom()
            {
                var text = GameObject.Find("RoomName").GetComponent<UI.GlowText>();
                text.originalText = room.name;

                var root = GameObject.Find("Grid");
                foreach (Transform _roomObj in root.transform)
                    _roomObj.gameObject.SetActive(false);

                roomObj = root.transform.Find(room.name).gameObject;
                roomObj.SetActive(true);
            }

            this.room = rooms[newRoomPos];
            rpos = newRoomPos;
            Debug.Log("Room Changed" + rpos.ToString());


            AdjustCamPos();
            SettingCurRoom();
            minimap.Exploring(rpos);

            // TODO:FOR DEBUG MOVE TO Savepoint
            minimap.Save();
            PlayerPrefs.Save();
        }
    }
}
