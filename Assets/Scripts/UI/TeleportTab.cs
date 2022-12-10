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

        private int curIdx;
        private int dstIdx
        {
            get => _dstIdx;
            set
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

            var curTeleporter = teleporters.Find(t => t.pos == Minimap.Instance.CurRoom.pos);
            curTelHighlight.localPosition = curTeleporter.transform.localPosition;
            curIdx = teleporters.IndexOf(curTeleporter);
            dstIdx = curIdx;
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

        public void Teleport()
        {
            void closeTab()
            {
                GameObject.Find("PanelController")
                .GetComponent<UI.PanelController>()
                .Toggle(gameObject);
            }

            if (curIdx != dstIdx)
            {
            }

            closeTab();
        }
    }
}