using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VVVVVV.UI.Utils.Glow;
using VVVVVV.World.Entity;

namespace VVVVVV.UI.Utils
{
    public class OptionSelector : MonoBehaviour
    {
        [SerializeField] protected List<Text> options;
        [SerializeField] protected UnityEvent<int> optionChanged = new UnityEvent<int>();
        private MoveController player => GameObject.Find("Player").GetComponent<MoveController>();

        void OnEnable()
        {
            activeChildren(true);
            player.enabled = false;
            curOptIdx = 0;
        }

        void OnDisable()
        {
            activeChildren(false);

            // TODO: refactoring need
            player.enabled = true;
        }

        public void Toggle()
        {
        }

        private void activeChildren(bool activate)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(activate);
        }

        void Update()
        {
            int direction = 0;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                direction = -1;
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                direction = 1;
            else
                return;

            var idx = curOptIdx + direction;
            if (idx < 0) idx = options.Count - 1;
            else if (options.Count <= idx) idx = 0;
            curOptIdx = idx;
        }

        public int curOptIdx
        {
            get => _curOptIdx;
            set
            {
                _curOptIdx = value;
                var effects = options.Select(x => x.GetComponent<GlowEffect>()).ToList();
                foreach (var opt in options.Select(x => x.GetComponent<GlowEffect>()))
                {
                    opt.GlowOn = false;
                    var _pad = opt.GetComponent<GlowTextPadding>();
                    if (_pad != null)
                        _pad.enabled = false;
                }

                options[curOptIdx].GetComponent<GlowEffect>().GlowOn = true;
                var pad = options[curOptIdx].GetComponent<GlowTextPadding>();
                if (pad != null)
                    pad.enabled = true;

                optionChanged.Invoke(curOptIdx);
            }
        }

        private int _curOptIdx;
    }
}