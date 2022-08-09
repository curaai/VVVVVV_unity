using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public Transform cam;
    // TODO: 
    /*
    Disable when room changed 
    Check when player touch Boundary
    240
    */

    private Dictionary<Vector2Int, Room> rooms;
    public Room room { get; private set; }
    public GameObject roomObj { get; private set; }

    void Start()
    {
        this.rooms = Resources.LoadAll<Room>("Tables/Rooms").ToDictionary(x => x.pos);
        ChangeRoom(new Vector2Int(52, 41));
    }

    void ChangeRoom(Vector2Int newRoomPos)
    {
        void AdjustCamPos()
        {
            var camX = room.pos.x * 640;
            var camY = room.pos.y * 480 - 40;

            cam.localPosition = new Vector3(camX, camY, cam.localPosition.z);
        }

        void SettingCurRoom()
        {
            var text = GameObject.Find("RoomName").GetComponent<Text>();
            text.text = room.name;

            var root = GameObject.Find("Grid");
            foreach (Transform _roomObj in root.transform)
                _roomObj.gameObject.SetActive(false);

            roomObj = root.transform.Find(room.name).gameObject;
            roomObj.SetActive(true);
        }

        this.room = rooms[newRoomPos];

        AdjustCamPos();
        SettingCurRoom();
    }
}
