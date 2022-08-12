using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VVVVVV.UI
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] Image fogMaskPanel;
        [SerializeField] Texture2D fogTile;

        HashSet<(int, int)> explored = new HashSet<(int, int)>();

        void Start()
        {
            var loaded = (HashSet<(int, int)>)Utils.PlayerPrefsSerializer.Deserialize("minimap");
            if (loaded != null)
                explored = loaded;
        }

        public void Exploring(Vector2Int r)
        {

            var len = explored.Count;
            explored.Add((r.x, r.y));

            (int rw, int rh) = (fogTile.width, fogTile.height);
            var tex = new Texture2D(rw * 20, rh * 20);
            tex.alphaIsTransparency = true;
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    var pixels = fogTile.GetPixels();
                    // reverse y for coordnation
                    if (explored.Contains((x + 100, (19 - y) + 100)))
                        pixels = Enumerable.Repeat(new Color(0, 0, 0, 0), rw * rh).ToArray();

                    tex.SetPixels(x * rw, y * rh, rw, rh, pixels);
                }
            }
            tex.Apply(false);
            var newMask = Sprite.Create(tex, new Rect(0, 0, 240, 180), Vector2.one / 2f);
            fogMaskPanel.sprite = newMask;
        }

        public void Save()
        {
            var serilized = Utils.PlayerPrefsSerializer.Serializable(explored);
            PlayerPrefs.SetString("minimap", serilized);
        }
    }
}
