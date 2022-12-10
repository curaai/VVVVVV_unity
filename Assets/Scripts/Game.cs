using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using VVVVVV.World;
using VVVVVV.World.Entity;

namespace VVVVVV
{
    public class Game : MonoBehaviour
    {
        [SerializeField] public Player player;
        [SerializeField] public Transform cam;

        [SerializeField] public SoundManager soundManager;
        [SerializeField] private List<GameObject> serializables;
        // TODO: Serialize timeline
        [SerializeField] public PlayableDirector teleportTimeline = null;

        private SaveManager saveManager;
        public Minimap minimap;

        void Awake()
        {
            Application.targetFrameRate = 60;
            minimap = GetComponentInChildren<Minimap>();

            var saveTargetList = new List<ISerializable>() {
                minimap,
                GameObject.Find("Clock").GetComponent<World.Clock>(),
                GameObject.FindGameObjectWithTag("Savepoint").GetComponent<World.Entity.Savepoint>(),
                player,
                GameObject.Find("EventTriggerManager").GetComponent<EventTriggerManager>(),
            };

            saveTargetList.AddRange(serializables.Select(x => x.GetComponent<ISerializable>()));
            saveManager = new SaveManager(saveTargetList);
        }

        void Start()
        {
            saveManager.LoadAll();

            if (minimap.CurRoom == null)
            {
                Vector2Int lastRoom;
                if (Savepoint.LastSavepoint != null)
                    lastRoom = Savepoint.LastSavepoint.GetComponentInParent<Room>().pos;
                else
                    lastRoom = player.GetComponentInParent<Room>().pos;

                ChangeRoom(lastRoom);
            }

            soundManager.Play(BGM.PUSHINGONWARDS);
        }

        void Update()
        {
            Vector2Int? PlayerMovedRoom()
            {
                if (player.GetComponent<MoveController>().velocity == Vector2.zero)
                    return null;

                var lpos = player.transform.localPosition;
                var res = Vector2Int.zero;

                if (lpos.x <= 0f) res.x = -1;
                else if (40f < lpos.x) res.x = 1;

                if (lpos.y <= -1.5f) res.y = 1;
                else if (28.25f < lpos.y) res.y = -1;

                return res == Vector2Int.zero ? (Vector2Int?)null : res;
            }

            if (!player.transform.parent.CompareTag("Room")) return;
            if (Teleporter.WarpNow || player.IsMoveNow) return;

            var newRoomDir = PlayerMovedRoom();
            if (newRoomDir.HasValue)
            {
                ChangeRoom(minimap.CurRoom.pos + newRoomDir.Value);
            }
        }

        public void Respawn()
        {
            saveManager.Load(player);
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

        public void ChangeRoom(Vector2Int newRoomPos, Vector3 inRoomPos)
        {
            ChangeRoom(newRoomPos);
            player.transform.localPosition = inRoomPos;
        }

        public void Save()
        {
            saveManager.Save();
        }
    }
}
