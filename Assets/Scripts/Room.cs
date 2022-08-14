using UnityEngine;

namespace VVVVVV
{
    [CreateAssetMenu(fileName = "Room", menuName = "VVVVVV/Make Room")]
    public class Room : ScriptableObject
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
