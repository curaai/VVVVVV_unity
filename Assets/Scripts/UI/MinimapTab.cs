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

        private Minimap minimap;

        void Awake()
        {
            minimap = GameObject.Find("World").GetComponentInChildren<Minimap>(true);
            roomBlink = transform.Find("BlinkRoom");
            fogMaskPanel = transform.Find("FogMask").GetComponent<Image>();
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
            SetBlinkPos(minimap.CurRoom.pos);
        }

        public void UpdateFog()
        {
            (int rw, int rh) = (fogTile.width, fogTile.height);
            var tex = new Texture2D(rw * 20, rh * 20);
            tex.alphaIsTransparency = true;
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    var pixels = fogTile.GetPixels();
                    // reverse y for coordnation
                    if (minimap.explored.Contains((x, 19 - y)))
                        pixels = Enumerable.Repeat(new UnityEngine.Color(0, 0, 0, 0), rw * rh).ToArray();

                    tex.SetPixels(x * rw, y * rh, rw, rh, pixels);
                }
            }
            tex.Apply(false);
            var newMask = Sprite.Create(tex, new Rect(0, 0, 240, 180), Vector2.one / 2f);
            fogMaskPanel.sprite = newMask;
        }
    }
}