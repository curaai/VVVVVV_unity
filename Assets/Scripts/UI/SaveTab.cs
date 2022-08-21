
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.World;
using VVVVVV.UI.Utils;
using VVVVVV.World.Entity;

namespace VVVVVV.UI
{
    public class SaveTab : MonoBehaviour
    {
        [SerializeField] Minimap minimap;
        [SerializeField] Clock clock;
        [SerializeField] Text lastSaveUI;
        [SerializeField] Text summaryUI;

        public bool saved { get; private set; }

        void Start()
        {
            LoadSavedData();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !saved)
            {
                GameObject.Find("Game").GetComponent<Game>().Save();
            }
        }

        public void LoadSavedData()
        {
            if (Savepoint.LastSavepoint != null)
            {
                (var areaStr, var clockStr) = GetSavedAreaAndTime();

                lastSaveUI.enabled = true;
                summaryUI.enabled = true;
                summaryUI.text = $"{areaStr}, {clockStr}";
            }
            else
            {
                lastSaveUI.enabled = false;
                summaryUI.enabled = false;
            }
        }

        public void Save()
        {
            // Check current ui opened 
            saved = true;

            transform.Find("NotSaved").gameObject.SetActive(false);
            var curTab = transform.Find("Saved");
            curTab.gameObject.SetActive(true);
            (var areaStr, var clockStr) = GetSavedAreaAndTime();
            curTab.Find("Container/savetime").GetComponent<Text>().text = clockStr;
            curTab.Find("Container/savearea").GetComponent<Text>().text = areaStr;

            // TODO: Set trinket, Crews
        }

        private (string, string) GetSavedAreaAndTime()
        {
            var rx = Mathf.FloorToInt(Savepoint.LastSavepoint.transform.position.x / 640);
            var ry = Mathf.FloorToInt(-Savepoint.LastSavepoint.transform.position.y / 480);
            var areaStr = minimap.room(new Vector2Int(rx, ry)).areaStr();
            var clockStr = Clock.FormatString(clock.savetime);
            return (areaStr, clockStr);
        }
    }
}
