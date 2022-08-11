using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using VVVVVV.Utils;

namespace VVVVVV.World.Entity
{
    public class Savepoint : MonoBehaviour
    {
        public bool ActivatedPoint { get; private set; }
        public bool CollidePlayerNow { get; private set; }

        public void Update()
        {
            /*
            TODO
            - Show UI when action
            - Set main SavePoint to game obj
            */

            void SetColor()
            {
                Color32 color;
                if (ActivatedPoint)
                {
                    var r = (byte)(164 + RandomHelper.fRand() * 64);
                    var g = (byte)(164 + RandomHelper.fRand() * 64);
                    var b = (byte)(255 - (RandomHelper.fRand() * 64));
                    color = new Color32(r, g, b, 255);
                }
                else
                {
                    var c = (byte)Mathf.FloorToInt(
                       GlowColorAnimation.glow / 2
                     + RandomHelper.fRand() * 8
                     + 80);
                    color = new Color32(c, c, c, 255);
                }
                GetComponent<SpriteRenderer>().color = color;
            }

            void OpenMapPage()
            {
                GameObject.Find("MapPanel").GetComponent<UI.SlidePanel>().Toggle();
            }

            SetColor();

            var pressEnter = Input.GetKeyDown(KeyCode.Return);
            if (CollidePlayerNow && pressEnter)
            {
                OpenMapPage();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player")) CollidePlayerNow = true;
        }
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player")) CollidePlayerNow = false;
        }
    }
}