using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace VVVVVV.UI.Utils
{
    public class OptionSelector : SlidePanel
    {
        [SerializeField] protected List<GlowText> options;
        [SerializeField] protected UnityEvent<int> optionChanged = new UnityEvent<int>();

        internal override void Open()
        {
            base.Open();

            curOptIdx = 0;
        }

        protected void Update()
        {
            if (!Opened)
                return;

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
                foreach (var opt in options)
                    opt.glowOn = false;

                options[curOptIdx].glowOn = true;
                optionChanged.Invoke(curOptIdx);
            }
        }

        private int _curOptIdx;
    }
}