using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using VVVVVV.World;

namespace VVVVVV.UI
{
    public class TeleportTab : IControllable
    {
        public override Type controlType => Type.UI;

        private List<UI.Teleporter> teleporters;

        private Transform curTelHighlight;
        private Transform dstTelHighlight;
        public int dstIdx
        {
            get => _dstIdx;
            private set
            {
                _dstIdx = value;
                dstTelHighlight.localPosition = teleporters[dstIdx].transform.localPosition;

                var anim = GetComponent<Animator>();
                anim.Play("TeleportTab IconBlink", 0);
            }
        }
        private int _dstIdx;

        private void Awake()
        {
            OnMove += ChangeTargetTeleporter;
            OnSpace += DummySpaceFunc;
            OnAction += Teleport;

            curTelHighlight = transform.GetChild(0);
            dstTelHighlight = transform.GetChild(transform.childCount - 1);
        }

        private void OnEnable()
        {
            FocusNow = true;

            teleporters = GameObject.Find("Canvas")
                .GetComponentsInChildren<Teleporter>(true)
                .OrderBy(t => t.Idx)
                .Where(t => t.Explored).ToList();

            Vector2Int getCurrentRoom()
            {
                var minimap = GameObject.Find("World").GetComponentInChildren<Minimap>(true);
                return minimap.CurRoom.pos;
            }

            var pos = getCurrentRoom();
            var curTeleporter = teleporters.Find(t => t.pos == pos);
            curTelHighlight.localPosition = curTeleporter.transform.localPosition;
            dstIdx = teleporters.IndexOf(curTeleporter);
        }
        private void OnDisable()
        {
            FocusNow = false;
        }

        public void ChangeTargetTeleporter(float direction)
        {
            var x = dstIdx + Mathf.FloorToInt(direction);
            if (x < 0) x = teleporters.Count - 1;
            else if (teleporters.Count <= x) x = 0;
            dstIdx = x;
        }

        // throw new NotImplementedException("Show Cur Selected teleporter");
        public void Teleport()
        {
            // throw new NotImplementedException("Move to selected teleporter");
        }
    }
}