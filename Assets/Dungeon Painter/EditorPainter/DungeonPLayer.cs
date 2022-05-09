using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TGP.DungeonEditor
{
    [System.Serializable]
    public class DungeonPLayer
    {
        public enum BlendMode
        {
            NORMAL, MULTPILY, SCREEN
        };

        public string name;
        public Color[] map;
        public Texture2D tex;
        public bool enabled;
        public float Opacity;
        public BlendMode mode;
        public bool locked;

        public DungeonPLayout ParentLayout;

        public DungeonPLayer(DungeonPLayout layout)
        {
            name = "Layer " + (layout.layers.Count + 1);
            Opacity = 1;
            mode = BlendMode.NORMAL;

            map = new Color[layout.Width * layout.Height];
            tex = new Texture2D(layout.Width, layout.Height);

            for (int x = 0; x < layout.Width; x++)
            {
                for (int y = 0; y < layout.Height; y++)
                {
                    map[x + y * layout.Width] = Color.clear;
                    tex.SetPixel(x, y, Color.clear);
                }
            }

            tex.filterMode = FilterMode.Point;
            tex.Apply();

            enabled = true;
            locked = false;
            ParentLayout = layout;

            Undo.undoRedoPerformed += LoadMapFromTex; // subscribe to the undo event
        }

        public DungeonPLayer(DungeonPLayer original)
        {
            name = original.name + " - Clone";
            Opacity = 1;
            mode = original.mode;

            map = (Color[])original.map.Clone();
            tex = new Texture2D(original.ParentLayout.Width, original.ParentLayout.Height);
            tex.SetPixels(original.tex.GetPixels());

            tex.filterMode = FilterMode.Point;
            tex.Apply();

            enabled = true;
            locked = original.locked;
            ParentLayout = original.ParentLayout;

            Undo.undoRedoPerformed += LoadMapFromTex;
        }

        void LoadMapFromTex()
        {
            for (int x = 0; x < ParentLayout.Width; x++)
            {
                for (int y = 0; y < ParentLayout.Height; y++)
                {
                    map[x + y * ParentLayout.Width] = tex.GetPixel(x, ParentLayout.Height - y - 1);
                }
            }
        }

        public Color GetPixel(int x, int y)
        {
            return tex.GetPixel(x, y);
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (!locked)
            {
                tex.SetPixel(x, y, color);
                tex.Apply();

                map[x + y * -1 * ParentLayout.Width - ParentLayout.Height] = color;
            }
        }

        public void LoadTexFromMap()
        {
            tex = new Texture2D(ParentLayout.Width, ParentLayout.Height);

            for (int x = 0; x < ParentLayout.Width; x++)
            {
                for (int y = 0; y < ParentLayout.Height; y++)
                {
                    tex.SetPixel(x, ParentLayout.Height - y - 1, map[x + y * ParentLayout.Width]);
                }
            }

            tex.filterMode = FilterMode.Point;
            tex.Apply();
        }

        public int GetOrder()
        {
            return ParentLayout.layers.IndexOf(this);
        }
    }
}
