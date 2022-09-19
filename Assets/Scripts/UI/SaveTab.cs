
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.World;
using VVVVVV.UI.Utils;
using VVVVVV.World.Entity;

namespace VVVVVV.UI
{
    public class SaveTab : MonoBehaviour
    {
        [SerializeField] Clock clock;
        [SerializeField] Text lastSaveUI;
        [SerializeField] Text summaryUI;

        Minimap minimap;

        private Transform offSavedUI => transform.Find("NotSaved");
        private Transform onSavedUI => transform.Find("Saved");

        public bool saved { get; private set; }
        void Awake()
        {
            minimap = GameObject.Find("Game").GetComponent<Game>().minimap;
        }

        void OnEnable()
        {
            LoadSavedData();
            offSavedUI.gameObject.SetActive(true);
            onSavedUI.gameObject.SetActive(false);
        }

        void OnDisable()
        {
            saved = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !saved)
            {
                GameObject.Find("Game").GetComponent<Game>().Save();
                saved = true;
                OpenSaveUI();
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

        public void OpenSaveUI()
        {
            offSavedUI.gameObject.SetActive(false);

            onSavedUI.gameObject.SetActive(true);
            (var areaStr, var clockStr) = GetSavedAreaAndTime();
            onSavedUI.Find("Container/savetime").GetComponent<Text>().text = clockStr;
            onSavedUI.Find("Container/savearea").GetComponent<Text>().text = areaStr;

            // TODO: Set trinket, Crews
        }

        private (string, string) GetSavedAreaAndTime()
        {
            var rx = Mathf.FloorToInt(Savepoint.LastSavepoint.transform.position.x / 640);
            var ry = -Mathf.FloorToInt(Savepoint.LastSavepoint.transform.position.y / 480);
            var areaStr = minimap.room(new Vector2Int(rx, ry)).areaStr();
            var clockStr = Clock.FormatString(clock.savetime);
            return (areaStr, clockStr);
        }
    }
}
