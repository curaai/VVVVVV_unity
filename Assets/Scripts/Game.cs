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
        [SerializeField] public UI.SaveTab savetab;
        [SerializeField] public World.TrinketManager trinketManager;
        [SerializeField] public Player player;
        [SerializeField] public Transform cam;

        private SaveManager saveManager;
        private PanelController panelController;

        void Awake()
        {
            Application.targetFrameRate = 60;
            var saveTargetList = new List<ISerializable>() {
                minimap,
                GameObject.FindGameObjectWithTag("Savepoint").GetComponent<World.Entity.Savepoint>(),
                GameObject.Find("Clock").GetComponent<World.Clock>(),
                player,
                trinketManager
            };
            saveManager = new SaveManager(saveTargetList);
            panelController = GameObject.Find("RootPanel").GetComponent<PanelController>();
        }

        void Start()
        {
            saveManager.Load();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                GameObject.Find("PausePanel").GetComponent<SlidePanel>().Toggle();
            if (Input.GetKeyDown(KeyCode.Return))
                GameObject.Find("MapPanel").GetComponent<SlidePanel>().Toggle();
        }

        public void Respawn()
        {
            saveManager.Load(player.SerializeKey);
            // TODO: Separate stat members from player
            player.deathCount++;
        }

        public void ChangeRoom(Vector2Int newRoomPos)
        {
            void EnableRoom()
            {
                void Tilemap()
                {
                    // Disable all 
                    var root = GameObject.Find("Grid");
                    foreach (Transform t in root.transform)
                        t.gameObject.SetActive(false);

                    minimap.roomObj(minimap.room(newRoomPos)).SetActive(true);
                }
                void Canvas()
                {
                    var root = GameObject.Find("Canvas").transform.Find("Rooms");
                    foreach (Transform t in root.transform)
                        t.gameObject.SetActive(false);

                    root.Find(player.room.name)?.gameObject.SetActive(true);
                }

                Tilemap();
                Canvas();
            }

            void AdjustCamPos()
            {
                var camX = newRoomPos.x * 640;
                var camY = newRoomPos.y * -480;

                cam.localPosition = new Vector3(camX, camY, cam.localPosition.z);
            }

            minimap.ChangeRoom(newRoomPos);

            EnableRoom();
            AdjustCamPos();

            saveManager.Save(minimap.SerializeKey);
        }

        public void Save()
        {
            saveManager.Save();
        }
    }
}
