using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.World;

namespace VVVVVV.UI
{
    [RequireComponent(typeof(Image))]
    public class Teleporter : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int pos { get; private set; }
        [SerializeField] public Sprite ExploredIcon;
        [SerializeField] public Sprite UnexploredIcon;

        public bool Explored { get; private set; } = false;
        public int Idx => Convert.ToInt32(name);

        public void SetExplored(bool x)
        {
            if (Explored == x)
                return;

            Explored = x;
            var icon = Explored ? ExploredIcon : UnexploredIcon;
            GetComponent<Image>().sprite = icon;
        }

        private void OnValidate()
        {
            ((RectTransform)transform).anchoredPosition = new Vector2(
                pos.x * 24,
                pos.y * -18
            );
        }
    }
}