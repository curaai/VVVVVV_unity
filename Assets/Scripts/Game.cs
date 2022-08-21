using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.UI.Utils;

namespace VVVVVV
{
    public class Game : MonoBehaviour
    {
        [SerializeField] public UI.Minimap minimap;
        [SerializeField] public UI.SaveTab savetab;
        [SerializeField] public Player player;
        [SerializeField] public Transform cam;

        private SaveManager saveManager;

        void Awake()
        {
            Application.targetFrameRate = 60;
            var saveTargetList = new List<ISerializable>() {
                minimap,
                GameObject.FindGameObjectWithTag("Savepoint").GetComponent<World.Entity.Savepoint>(),
                GameObject.Find("Clock").GetComponent<World.Clock>(),
            };
            saveManager = new SaveManager(saveTargetList);
            saveManager.Load();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                GameObject.Find("PausePanel").GetComponent<SlidePanel>().Toggle();
            if (Input.GetKeyDown(KeyCode.Return))
                GameObject.Find("MapPanel").GetComponent<SlidePanel>().Toggle();
        }

        public void ChangeRoom(Vector2Int newRoomPos)
        {
            void EnableRoom()
            {
                // Disable all 
                var root = GameObject.Find("Grid");
                foreach (Transform t in root.transform)
                    t.gameObject.SetActive(false);

                minimap.roomObj(minimap.room(newRoomPos)).SetActive(true);
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
            savetab.Save();
            // savetab.LoadSavedData();
        }
    }
}
