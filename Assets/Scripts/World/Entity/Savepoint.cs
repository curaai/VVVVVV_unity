using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using VVVVVV.Utils;

namespace VVVVVV.World.Entity
{
    public class Savepoint : MonoBehaviour, ISerializable
    {
        public string SerializeKey { get => "last_savepoint"; }

        public static Savepoint LastSavepoint { get; private set; } = null;
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

            SetColor();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                // TODO: disable other savepoints

                CollidePlayerNow = true;
                ActivatedPoint = true;
                LastSavepoint = this;

                // AutoSave
                GameObject.Find("Game").GetComponent<Game>().Save();
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player")) CollidePlayerNow = false;
        }

        public string Save()
        {
            if (LastSavepoint == this)
            {
                var room = (transform.position.x / 40, transform.position.y / 30);
                var local = (transform.localPosition.x, transform.localPosition.y);

                return SaveManager.SerializableObject((room, local));
            }
            else
                return LastSavepoint.Save();
        }

        public void Load(string str)
        {
            var res = SaveManager.DeserializeObject(str) as ((float, float), (float, float))?;
            // TODO: respawn player, not implemented now 
            if (res.HasValue)
                return;
        }
    }
}