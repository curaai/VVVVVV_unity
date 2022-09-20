using UnityEngine;

namespace VVVVVV
{
    public class Room : MonoBehaviour
    {
        public Vector2Int pos;
        public Area area;
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
