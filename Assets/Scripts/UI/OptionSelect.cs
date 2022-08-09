using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.UI
{
    public class OptionSelect : EnterUI
    {
        private static readonly Color NON_SELECT_OPTION_COLOR = new Color32(96, 96, 96, 255);

        protected Color highlightColor => new Color32(196, 196, (byte)(255 - GlowColorAnimation.glow), 255);
        [SerializeField] protected List<Text> highlightTexts;
        [SerializeField] protected List<Text> options;
        private List<string> originalOptTexts;

        private void Start()
        {
            originalOptTexts = options.Select(x => x.text).ToList();
        }

        protected override void Pause()
        {
            base.Pause();

            curOptIdx = 0;
        }

        protected override void Update()
        {
            base.Update();
            if (!Paused)
                return;

            options[curOptIdx].color = highlightColor;
            foreach (var text in highlightTexts)
                text.color = highlightColor;

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
                var curOpt = options[curOptIdx];

                foreach (var i in Enumerable.Range(0, options.Count))
                {
                    options[i].color = NON_SELECT_OPTION_COLOR;
                    options[i].text = originalOptTexts[i];
                }

                curOpt.text = $"[ {curOpt.text.ToUpper()} ]";
            }
        }

        private int _curOptIdx;
    }
}