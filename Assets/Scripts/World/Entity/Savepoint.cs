using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using VVVVVV.Utils;
using VVVVVV.UI.Utils.Glow;

namespace VVVVVV.World.Entity
{
    public class Savepoint : MonoBehaviour, ISerializable
    {
        public string SerializeKey { get => "last_savepoint"; }
        private int SavepointLayer => LayerMask.NameToLayer("entity");

        public static Savepoint LastSavepoint { get; private set; } = null;
        private static Savepoint[] allSavepoints = null;

        void Awake()
        {
            allSavepoints ??= transform.root.GetComponentsInChildren<Savepoint>();
        }

        public void Activate()
        {
            void disableAll() =>
                allSavepoints
                    .Select(x => x.GetComponentInChildren<SpriteGlowEffect>())
                    .Select(x => x.GlowOn = false)
                    .ToList();

            LastSavepoint = this;

            disableAll();

            GetComponentInChildren<SpriteGlowEffect>().GlowOn = true;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Activate();

                // AutoSave
                GameObject.Find("World").GetComponent<Game>().Save();
            }
        }

        public string Save()
        {
            if (LastSavepoint == this)
            {
                var pos = (transform.position.x, transform.position.y);

                return SaveManager.SerializableObject(pos);
            }
            else
                return LastSavepoint.Save();
        }

        public void Load(string str)
        {
            if (str == "") return;

            Savepoint FindLastSavepoint(string str)
            {
                var res = SaveManager.DeserializeObject<(float, float)>(str);
                return Physics2D.OverlapCircleAll(
                                        new Vector2(res.Item1, res.Item2),
                                        3
                                        )
                                    .Where(x => x.CompareTag("Savepoint"))
                                    .Select(x => x.GetComponent<Savepoint>())
                                    .FirstOrDefault();
            }

            LastSavepoint ??= FindLastSavepoint(str);
            LastSavepoint.Activate();
        }
    }
}