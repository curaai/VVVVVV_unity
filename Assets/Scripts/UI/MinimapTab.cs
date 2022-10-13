using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VVVVVV.World;

namespace VVVVVV.UI
{
    public class MinimapTab : MonoBehaviour
    {
        [SerializeField] Texture2D fogTile;
        private Image fogMaskPanel;
        private Transform roomBlink;

        private UI.Teleporter[] teleporters;
        private Minimap minimap;

        void Awake()
        {
            minimap = GameObject.Find("World").GetComponentInChildren<Minimap>(true);
            fogMaskPanel = transform.Find("FogMask").GetComponent<Image>();
            roomBlink = transform.Find("BlinkRoom");

            teleporters = GetComponentsInChildren<UI.Teleporter>();
        }

        void OnEnable()
        {
            void SetBlinkPos(Vector2Int v)
            {
                roomBlink.transform.localPosition = new Vector3(
                    24 * v.x,
                    -(18 * v.y),
                    0
                );
            }

            UpdateFog();
            UpdateTeleporter();

            if (roomBlink != null)
                SetBlinkPos(minimap.CurRoom.pos);
        }

        public void UpdateFog()
        {
            (int tw, int th) = (fogTile.width, fogTile.height);
            var fogMask = new Texture2D(tw * 20, th * 20);
            fogMask.alphaIsTransparency = true;
            var transparentTile = Enumerable.Repeat(new UnityEngine.Color(0, 0, 0, 0), tw * th).ToArray();

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    UnityEngine.Color[] pixels;

                    // reverse y for coordnation
                    if (minimap.explored.Contains((x, 19 - y)))
                        pixels = transparentTile;
                    else
                        pixels = fogTile.GetPixels();

                    fogMask.SetPixels(x * tw, y * th, tw, th, pixels);
                }
            }
            fogMask.Apply(false);
            fogMaskPanel.sprite = Sprite.Create(fogMask, new Rect(0, 0, 240, 180), Vector2.one / 2f); ;
        }

        public void UpdateTeleporter()
        {
            foreach (var t in teleporters)
                t.SetExplored(minimap.explored.Contains((t.pos.x, t.pos.y)));
        }
    }
}