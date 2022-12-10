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
        [SerializeField] private AudioClip saveSound;

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

            SoundManager.Instance.PlayEffect(saveSound);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Activate();

                // AutoSave
                Game.Instance.Save();
            }
        }

        public string Serialize()
        {
            if (LastSavepoint == this)
            {
                var pos = (transform.position.x, transform.position.y);
                return Utils.SerializeHelper.SerializeObject(pos);
            }
            else
                return LastSavepoint.Serialize();
        }

        public void LoadSerializedData(string str)
        {
            Savepoint FindLastSavepoint(string str)
            {
                var pos = Utils.SerializeHelper.DeserializeObject<(float, float)>(str);

                return Physics2D.OverlapCircleAll(
                                        new Vector2(pos.Item1, pos.Item2),
                                        3
                                        )
                                    .Select(x => x.GetComponentInParent<Savepoint>())
                                    .Where(x => x != null)
                                    .Where(x => x.gameObject.CompareTag("Savepoint"))
                                    .FirstOrDefault();
            }

            LastSavepoint ??= FindLastSavepoint(str);
            LastSavepoint?.Activate();
        }
    }
}