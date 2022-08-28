using UnityEngine;
using UnityEngine.UI;
using VVVVVV.World;

namespace VVVVVV.UI
{
    public class StatTab : MonoBehaviour
    {
        [SerializeField] Text trinket;
        [SerializeField] Text deaths;
        [SerializeField] Text time;
        [SerializeField] TrinketManager trinketManager;

        void OnEnable()
        {
            trinket.text = trinketManager.CountString().Trim();
            deaths.text = GameObject.FindWithTag("Player").GetComponent<Player>().deathCount.ToString();
        }

        void Update()
        {
            var c = Clock.Instance;
            time.text = Clock.FormatString(c.curPlaytime);
        }
    }
}
