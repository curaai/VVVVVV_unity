using UnityEngine;

namespace VVVVVV
{
    [CreateAssetMenu(fileName = "Room", menuName = "VVVVVV/Make Room")]
    public class Room : ScriptableObject
    {
        public Vector2Int pos;
    }
}
