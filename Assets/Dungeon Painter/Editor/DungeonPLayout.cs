using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TGP.Utilites;

namespace TGP.DungeonEditor
{
    [System.Serializable]
    public class DungeonPLayout : ScriptableObject
    {
        #region Getter And Setters

        private Rect window
        {
            get { return DungeonPEditorWindow.window.position; }
        }

        #endregion

        #region Layout Data

        public int Width; //Width Of the Dungeon
        public int Height; //Height of the Dungeon (2D space)

        public List<DungeonPLayer> layers;
        public int layercount
        {
            get { return layers.Count; }
        }

        public Texture2D FinalLayout;

        #endregion

        #region View & Nav Settings

        [SerializeField] private float _gridSpacing = 20f;
        public float gridspacing
        {
            get { return _gridSpacing + 1f; }
            set { _gridSpacing = Mathf.Clamp(value, 0, 140f); }
        }

        public float gridOffsetY = 0;
        public float gridOffsetX = 0;

        private int _selectedLayer = 0;
        public int selectedLayer
        {
            get { return Mathf.Clamp(_selectedLayer, 0, layercount); }
            set { _selectedLayer = value; }
        }
        #endregion

        #region Painitng Settings
        public Color selectedColor = new Color(1, 0, 0, 1);
        public DPTool tool = DPTool.PaintBrush;
        public int gridBGIndex = 0;
        #endregion

        #region Other Stuff
        public bool dirty = false;

       public ColorAndGameObjectDictonary KeyData;
        #endregion

        public DungeonPLayout() { }

        public void Init(int w, int h)
        {
            Width = w;
            Height = h;

            layers = new List<DungeonPLayer>();
            KeyData = new ColorAndGameObjectDictonary();
            DungeonPLayer newLayer = new DungeonPLayer(this);
            layers.Add(newLayer);

            EditorUtility.SetDirty(this);
            dirty = true;
        }

        public void SetPixelByPos(Color color, Vector2 pos, int layer)
        {
            Vector2 pixelCordinate = GetPixelCordinate(pos);

            if (pixelCordinate == new Vector2(-1, -1))
                return;

            Undo.RecordObject(layers[layer].tex, "ColorPixel");

            layers[layer].SetPixel((int)pixelCordinate.x, (int)pixelCordinate.y, color);

            EditorUtility.SetDirty(this);
            dirty = true;

        }

        public Color GetPixelbyPos(Vector2 pos, int layer)
        {
            Vector2 pixelCoordinate = GetPixelCordinate(pos);

            if (pixelCoordinate == new Vector2(-1, -1))
            {
                return Color.clear;
            }
            else
            {
                return layers[layer].GetPixel((int)pixelCoordinate.x, (int)pixelCoordinate.y);
            }
        }

        public Color GetBlendedPixel(int x, int y)
        {
            Color color = Color.clear;

            for (int i = 0; i < layers.Count; i++)
            {
                if (!layers[i].enabled)
                    continue;

                Color pixel = layers[i].tex.GetPixel(x, y);

                // This is a blend between two methods of calculating color blending; Alpha blending and premultiplied alpha blending
                // I have no clue why this actually works but it's very accurate :D
                float newR = Mathf.Lerp(1f * pixel.r + (1f - pixel.a) * color.r, pixel.a * pixel.r + (1f - pixel.a) * color.r, color.a);
                float newG = Mathf.Lerp(1f * pixel.g + (1f - pixel.a) * color.g, pixel.a * pixel.g + (1f - pixel.a) * color.g, color.a);
                float newB = Mathf.Lerp(1f * pixel.b + (1f - pixel.a) * color.b, pixel.a * pixel.b + (1f - pixel.a) * color.b, color.a);

                float newA = pixel.a + color.a * (1 - pixel.a);

                color = new Color(newR, newG, newB, newA);
            }

            return color;
        }

        public Vector2 GetPixelCordinate(Vector2 pos)
        {
            Rect texpos = GetIMGRect();

            if (!texpos.Contains(pos))
                return new Vector2(-1f, -1f);

            float re1X = (pos.x - texpos.x) / texpos.width;
            float re1Y = (texpos.y - pos.y) / texpos.height;

            int pixelX = (int)(Width * re1X);
            int pixelY = (int)(Height * re1Y) - 1;

            return new Vector2(pixelX, pixelY);
        }

        public Rect GetIMGRect()
        {
            float ratio = (float)Height / (float)Width;
            float w = gridspacing * 30;
            float h = ratio * gridspacing * 30;

            float xpos = window.width / 2f - w / 2f + gridOffsetX;
            float ypos = window.height / 2f - h / 2f + 20 + gridOffsetY;

            return new Rect(xpos, ypos, w, h);
        }

        public void ChangeLayerPosition(int from, int to)
        {
            if (from >= layers.Count || to >= layers.Count || from < 0 || to < 0)
            {
                Debug.LogError("Cannot ChangeLayerPosition, out of range.");
                return;
            }

            DungeonPLayer layer = layers[from];
            layers.RemoveAt(from);
            layers.Insert(to, layer);

            dirty = true;
        }

        public Vector2 GetReadablePixelCoordinate(Vector2 pos)
        {
            Vector2 coord = GetPixelCordinate(pos);

            if (coord.x == -1)
            {
                return coord;
            }

            coord.x += 1;
            coord.y *= -1;
            return coord;
        }

        public Texture2D GetFinalImage(bool update)
        {
            if (!dirty && FinalLayout != null || !update && FinalLayout != null)
                return FinalLayout;

            FinalLayout = DungeonPDrawer.CalculateBlendedTex(layers);

            FinalLayout.filterMode = FilterMode.Point;
            FinalLayout.Apply();

            dirty = false;
            return FinalLayout;
        }

        public void LoadAllTexsFromMaps()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].tex == null)
                    layers[i].LoadTexFromMap();
            }
        }

        public void AddLayer()
        {
            Undo.RecordObject(this, "AddLayer");
            EditorUtility.SetDirty(this);
            this.dirty = true;

            DungeonPLayer newLayer = new DungeonPLayer(this);
            layers.Add(newLayer);
        }

        public void RemoveLayerAt(int index)
        {
            Undo.RecordObject(this, "RemoveLayer");
            EditorUtility.SetDirty(this);
            this.dirty = true;

            layers.RemoveAt(index);
            if (selectedLayer == index)
            {
                selectedLayer = index - 1;
            }
        }
    }
}
