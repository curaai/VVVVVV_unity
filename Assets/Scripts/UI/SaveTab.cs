
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.World;
using VVVVVV.UI.Utils;
using VVVVVV.World.Entity;

namespace VVVVVV.UI
{
    // public class SaveTab : MonoBehaviour
    public class SaveTab : IControllable
    {
        [SerializeField] private AudioClip savedSound;

        [SerializeField] Clock clock;
        [SerializeField] Text lastSaveUI;
        [SerializeField] Text summaryUI;

        private Transform offSavedUI => transform.Find("NotSaved");
        private Transform onSavedUI => transform.Find("Saved");
        public override Type controlType => Type.UI;

        public bool saved { get; private set; }
        void Awake()
        {
            OnSpace += Save;
        }

        protected void OnEnable()
        {
            LoadSavedData();
            offSavedUI.gameObject.SetActive(true);
            onSavedUI.gameObject.SetActive(false);

            FocusNow = true;
        }

        protected void OnDisable()
        {
            saved = false;
            FocusNow = false;
        }

        void Save()
        {
            if (!saved)
            {
                GameObject.Find("World").GetComponent<Game>().Save();
                saved = true;
                OpenSaveUI();

                SoundManager.Instance.PlayEffect(savedSound);
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
            var minimap = GameObject.Find("World").GetComponent<Game>().minimap;

            var rx = Mathf.FloorToInt(Savepoint.LastSavepoint.transform.position.x / 640);
            var ry = -Mathf.FloorToInt(Savepoint.LastSavepoint.transform.position.y / 480);
            var areaStr = minimap.room(new Vector2Int(rx, ry)).areaStr();
            var clockStr = Clock.FormatString(clock.savetime);
            return (areaStr, clockStr);
        }
    }
}
