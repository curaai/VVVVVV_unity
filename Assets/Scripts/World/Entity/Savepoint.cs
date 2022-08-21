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
        private int SavepointLayer => LayerMask.NameToLayer("entity");

        public static Savepoint LastSavepoint { get; private set; } = null;
        public bool ActivatedPoint { get; private set; } = false;
        public bool CollidePlayerNow { get; private set; } = false;

        public void Activate()
        {
            ActivatedPoint = true;
            LastSavepoint = this;

            GetComponent<UI.Utils.Glow.SpriteGlowEffect>().GlowOn = true;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                // TODO: disable other savepoints

                CollidePlayerNow = true;
                Activate();

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
                var roompos = (Mathf.FloorToInt(transform.position.x / 640),
                                -Mathf.FloorToInt(transform.position.y / 480));

                var local = (transform.position.x, transform.position.y);

                return SaveManager.SerializableObject((roompos, local));
            }
            else
                return LastSavepoint.Save();
        }

        public void Load(string str)
        {
            if (str == "")
                return;

            Savepoint FindLastSavepoint(string str)
            {
                var res = SaveManager.DeserializeObject<((int, int), (float, float))>(str);
                var rpos = res.Item1;

                return Physics2D.OverlapCircleAll(
                                        new Vector2(res.Item2.Item1, res.Item2.Item2),
                                        3
                                        )
                                    .Where(x => x.CompareTag("Savepoint"))
                                    .Select(x => x.GetComponent<Savepoint>())
                                    .First();
            }

            var savepoint = FindLastSavepoint(str);

            // TODO: respawn player, not implemented now 

            if (savepoint == null)
                return;
            else
            {
                LastSavepoint = savepoint;
                LastSavepoint.Activate();
            }
        }
    }
}