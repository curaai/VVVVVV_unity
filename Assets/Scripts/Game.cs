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
        [SerializeField] public Transform player;
        [SerializeField] public Transform cam;

        private SaveManager saveManager;

        void Start()
        {
            var saveTargetList = new List<ISerializable>() {
                minimap,
                GameObject.FindGameObjectWithTag("Savepoint").GetComponent<World.Entity.Savepoint>(),
            };
            saveManager = new SaveManager(saveTargetList);
            saveManager.Load();
        }

        void Update()
        {
            Vector2Int? IsRoomChanged()
            {
                var rpos = minimap.RoomObj.transform.InverseTransformPoint(player.position);
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
                ChangeRoom(minimap.RoomPos + newRoomDir.Value);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                GameObject.Find("PausePanel").GetComponent<UI.SlidePanel>().Toggle();
            if (Input.GetKeyDown(KeyCode.Return))
                GameObject.Find("MapPanel").GetComponent<UI.SlidePanel>().Toggle();
        }

        void ChangeRoom(Vector2Int newRoomPos)
        {
            void AdjustCamPos()
            {
                var camX = minimap.RoomPos.x * 640;
                var camY = minimap.RoomPos.y * 480;

                cam.localPosition = new Vector3(camX, camY, cam.localPosition.z);
            }

            minimap.ChangeRoom(newRoomPos);
            saveManager.Save(minimap.SerializeKey);

            AdjustCamPos();
        }

        public void Save()
        {
            saveManager.Save();
        }
    }
}
