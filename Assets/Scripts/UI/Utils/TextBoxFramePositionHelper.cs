using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.UI.Utils
{
    [RequireComponent(typeof(TextBoxFrame))]
    public class TextBoxFramePositionHelper : MonoBehaviour
    {
        public enum PositionType
        {
            ABOVE,
            BELOW,
        }

        [SerializeField] PositionType positionType;

        private Transform player => GameObject.Find("Player").transform;
        private string txt => GetComponent<Text>().text;

        void OnEnable()
        {
            Vector2 playerPosInUI()
            {
                var rt = player.transform as RectTransform;
                return new Vector2(
                    (rt.localPosition.x - rt.rect.width / 2) / 40 * 640,
                    (rt.localPosition.y + rt.rect.height / 2) / 30 * 480
                );
            }
            var playerLT = playerPosInUI();
            var rt = (RectTransform)transform;

            var isLeft = player.localScale.x == 1;
            var additionalLines = txt.Count(c => c == '\n');

            float textx;
            float texty = -(480 - playerLT.y);
            var textw = rt.rect.width;// exclude boundary
            var texth = rt.rect.height;// exclude boundary

            if (positionType == PositionType.BELOW)
                texty += 26;
            else if (isLeft)
                texty += 32 + (additionalLines * 16);
            else
                texty += 36 + (additionalLines * 16);

            if (isLeft)
                textx = playerLT.x - textw;
            else
                textx = playerLT.x - 32;

            if (textx < 20) textx = 20;
            if (-20 < texty) texty = -20;
            if (620 < textx + textw + 32) textx = 620 - (textw + 32);
            if (texty + texth + 32 < -460) texty = -460 + (texth + 32);

            rt.anchoredPosition = new Vector2(textx, texty);
        }
    }
}