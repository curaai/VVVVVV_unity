using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VVVVVV.UI.Utils.Glow;
using VVVVVV.World.Entity;

namespace VVVVVV.UI.Utils
{
    public class OptionSelector : IControllable
    {
        [SerializeField] protected List<Text> options;
        [SerializeField] protected UnityEvent<int> optionChanged;

        void Awake()
        {
            controlType = IControllable.Type.UI;
            OnMove += Move;
        }

        void OnEnable()
        {
            curOptIdx = 0;
            InputControlManager.Instance.SetFocus(this);
        }
        void OnDisable()
        {
            InputControlManager.Instance.DeFocus();
        }

        void Move(float _direction)
        {
            if (_direction == 0) return;
            var direction = Mathf.FloorToInt(_direction);

            var idx = curOptIdx + direction;
            if (idx < 0)
                idx = options.Count - 1;
            else if (options.Count <= idx)
                idx = 0;
            curOptIdx = idx;
        }

        public int curOptIdx
        {
            get => _curOptIdx;
            set
            {
                void SetGlowOption(int idx, bool x)
                {
                    var glow = options[idx].GetComponent<GlowEffect>();
                    glow.GlowOn = x;
                    var padding = glow.GetComponent<GlowTextPadding>();
                    if (padding != null)
                        padding.enabled = x;
                }

                _curOptIdx = value;
                foreach (var i in Enumerable.Range(0, options.Count))
                    SetGlowOption(i, false);
                SetGlowOption(_curOptIdx, true);

                optionChanged.Invoke(curOptIdx);
            }
        }

        private int _curOptIdx;
    }
}