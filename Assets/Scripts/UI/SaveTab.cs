
using UnityEngine;
using VVVVVV.World;
using VVVVVV.UI.Utils;
using VVVVVV.World.Entity;

namespace VVVVVV.UI
{
    public class SaveTab : MonoBehaviour
    {
        [SerializeField] Minimap minimap;
        [SerializeField] Clock clock;
        [SerializeField] GlowText lastSaveUI;
        [SerializeField] GlowText summaryUI;

        public bool saved { get; private set; }
        void Start()
        {
            LoadSavedData();
        }

        public void LoadSavedData()
        {
            if (Savepoint.LastSavepoint != null)
            {
                var rx = Mathf.FloorToInt(Savepoint.LastSavepoint.transform.position.x / 640);
                var ry = Mathf.FloorToInt(Savepoint.LastSavepoint.transform.position.y / 480);
                var areaStr = minimap[new Vector2Int(rx, ry)].areaStr();
                var clockStr = clock.ToString();

                lastSaveUI.enabled = true;
                summaryUI.enabled = true;
                summaryUI.originalText = $"{areaStr}, {clockStr}";
            }
            else
            {
                lastSaveUI.enabled = false;
                summaryUI.enabled = false;
            }
        }

        public void Save()
        {
            saved = true;
        }
    }
}
