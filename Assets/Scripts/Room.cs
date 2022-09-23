using System;
using UnityEngine;

namespace VVVVVV
{
    public class Room : MonoBehaviour
    {
        public Vector2Int pos;
        public Area area;
        public GameObject UI { get; private set; }

        void Awake()
        {
            UI = GameObject.Find("UI Rooms").transform.Find(name)?.gameObject;
        }

        void OnEnable() => UI?.SetActive(true);
        void OnDisable() => UI?.SetActive(false);

        void OnValidate()
        {
            transform.localPosition = new Vector3(
                pos.x * 40,
                -(pos.y * 30),
                0
            );
        }

        public string areaStr()
        {
            switch (area)
            {
                case Area.SpaceStation:
                    return "Space Station";
                default:
                    return "???";
            }
        }
    }

    public enum Area
    {
        SpaceStation = 5,
    }
}
