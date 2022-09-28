using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.UI
{
    public enum Color
    {
        CYAN,
        RED,
        GREEN,
        YELLOW,
        BLUE,
        PURPLE,
        WHITE,
        GRAY,
        ORANGE,
    }

    public class TextBoxFrame : MonoBehaviour
    {
        [SerializeField] private AudioClip speakSound = null;
        [SerializeField] public float DestroyTimer = -1;

        [SerializeField]
        private VVVVVV.UI.Color _color;
        public VVVVVV.UI.Color color
        {
            get => _color;
            set
            {
                _color = value;
                setColor(color);
            }
        }


        void OnEnable()
        {
            if (DestroyTimer != -1)
                Destroy(gameObject, DestroyTimer);
            if (speakSound != null)
                SoundManager.Instance.PlayEffect(speakSound);
        }

        private void setColor(Color colorEnum)
        {
            UnityEngine.Color32 convert(Color c)
            {
                switch (c)
                {
                    case Color.CYAN:
                        return new UnityEngine.Color32(164, 164, 255, 255);
                    case Color.RED:
                        return new UnityEngine.Color32(255, 60, 60, 255);
                    case Color.GREEN:
                        return new UnityEngine.Color32(144, 255, 144, 255);
                    case Color.YELLOW:
                        return new UnityEngine.Color32(255, 255, 134, 255);
                    case Color.BLUE:
                        return new UnityEngine.Color32(95, 95, 255, 255);
                    case Color.PURPLE:
                        return new UnityEngine.Color32(255, 134, 255, 255);
                    case Color.WHITE:
                        return new UnityEngine.Color32(244, 244, 244, 255);
                    case Color.GRAY:
                        return new UnityEngine.Color32(174, 174, 174, 255);
                    case Color.ORANGE:
                        return new UnityEngine.Color32(255, 130, 20, 255);
                    default:
                        throw new ArgumentException($"Can't parse color enum {c}");
                }
            }

            var c = convert(colorEnum);

            var backgroundColor = new Color32((byte)(c.r / 255), (byte)(c.g / 255), (byte)(c.b / 255), 255);

            var textObj = transform.GetChild(1);
            textObj.GetComponent<Text>().color = c;
            foreach (Transform boundary in transform.GetChild(0).transform)
                boundary.GetComponent<Image>().color = c;

            var backgroundObj = transform.GetChild(0).GetComponent<Image>();
            backgroundObj.color = backgroundColor;
        }

        void setColorAlpha(float a)
        {
            var color = transform.GetChild(1).GetComponent<Text>().color;
            var newColor = new UnityEngine.Color(color.r, color.g, color.b, a);
            var newBackgroundColor = new UnityEngine.Color(color.r / 6, color.g / 6, color.b / 6, a);

            var textObj = transform.GetChild(1);
            textObj.GetComponent<Text>().color = newColor;
            foreach (Transform boundary in transform.GetChild(0).transform)
                boundary.GetComponent<Image>().color = newColor;
            var backgroundObj = transform.GetChild(0).GetComponent<Image>();
            backgroundObj.color = newBackgroundColor;
        }

        private void OnValidate()
        {
            color = _color;
            transform.GetChild(1).GetComponent<Text>().text = GetComponent<Text>().text;
        }
        private void OnDisable() => GetComponent<Animator>().SetTrigger("OnDisable");
    }
}
