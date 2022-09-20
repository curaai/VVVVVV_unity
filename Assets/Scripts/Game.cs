using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.UI.Utils;
using VVVVVV.UI;

namespace VVVVVV
{
    public class Game : MonoBehaviour
    {
        [SerializeField] public UI.Minimap minimap;
        [SerializeField] public World.TrinketManager trinketManager;
        [SerializeField] public Player player;
        [SerializeField] public Transform cam;

        [SerializeField] private GameObject PausePanel;
        [SerializeField] private GameObject MapPanel;


        private SaveManager saveManager;
        private PanelController panelController;

        void Awake()
        {
            Application.targetFrameRate = 60;
            var saveTargetList = new List<ISerializable>() {
                // minimap,
                // GameObject.FindGameObjectWithTag("Savepoint").GetComponent<World.Entity.Savepoint>(),
                // GameObject.Find("Clock").GetComponent<World.Clock>(),
                player,
                // trinketManager
            };
            saveManager = new SaveManager(saveTargetList);
            // panelController = GameObject.Find("RootPanel").GetComponent<PanelController>();
        }

        void Start()
        {
            saveManager.Load();

            if (minimap.CurRoom == null)
                ChangeRoom(player.transform.parent.GetComponent<Room>().pos);
        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Escape))
            //     panelController.Toggle(PausePanel);
            // if (Input.GetKeyDown(KeyCode.Return))
            //     panelController.Toggle(MapPanel);

            Vector2Int? PlayerMovedRoom()
            {
                var lpos = player.transform.localPosition;
                var res = Vector2Int.zero;

                if (lpos.x <= 0f) res.x = -1;
                else if (40f < lpos.x) res.x = 1;

                if (lpos.y <= -1.5f) res.y = 1;
                else if (28.25f < lpos.y) res.y = -1;

                return res == Vector2Int.zero ? (Vector2Int?)null : res;
            }
            var newRoomDir = PlayerMovedRoom();
            if (newRoomDir.HasValue)
                ChangeRoom(minimap.CurRoom.pos + newRoomDir.Value);
        }

        public void Respawn()
        {
            saveManager.Load(player.SerializeKey);
            // TODO: Separate stat members from player
            player.deathCount++;
        }

        public void ChangeRoom(Vector2Int newRoomPos)
        {
            void AdjustCamPos()
            {
                var camX = newRoomPos.x * 40 + 20;
                var camY = newRoomPos.y * -30 + 15;

                cam.localPosition = new Vector3(camX, camY, cam.localPosition.z);
            }

            minimap.ChangeRoom(newRoomPos);
            player.transform.SetParent(minimap.CurRoom.transform);
            AdjustCamPos();
        }

        public void Save()
        {
            saveManager.Save();
        }
    }
}
