using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TGP.DungeonEditor
{
    public class DungeonPEditorWindow : EditorWindow
    {

        public static DungeonPEditorWindow window; //Static Instance of this window

        public static DungeonPLayout CurrentLayout; //Current Layout Being Edited

        #region Getters And Setters

        private float gridspacing
        {
            get
            {
                return CurrentLayout.gridspacing;
            }

            set
            {
                CurrentLayout.gridOffsetX = value;
            }
        }

		private float gridOffsetX
		{
			get { return CurrentLayout.gridOffsetX; }
			set { CurrentLayout.gridOffsetX = value; }
		}
		private float gridOffsetY
		{
			get { return CurrentLayout.gridOffsetY; }
			set { CurrentLayout.gridOffsetY = value; }
		}

		private DPTool tool
		{
			get { return CurrentLayout.tool; }
			set { CurrentLayout.tool = value; }
		}
		private Color32 selectedColor
		{
			get { return CurrentLayout.selectedColor; }
			set { CurrentLayout.selectedColor = value; }
		}
		private int gridBGIndex
		{
			get { return CurrentLayout.gridBGIndex; }
			set { CurrentLayout.gridBGIndex = value; }
		}


		#endregion

		private static DPTool lasttool = DPTool.Empty;

        #region Initilization

		[MenuItem("Window/Dungeon Editor %#p")]
		public static void Init()
		{
			// Get existing open window or if none, make new one
			window = (DungeonPEditorWindow)EditorWindow.GetWindow(typeof(DungeonPEditorWindow));
#if UNITY_4_3
		window.title = "Pixel Art Editor";
#elif UNITY_4_6
		window.title = "Pixel Art Editor";
#else
			window.titleContent = new GUIContent("Pixel Art Editor");
#endif

			string path = EditorPrefs.GetString("currentImgPath", "");

			if (path.Length != 0)
				CurrentLayout = DungeonPSession.OpenImageAtPath(path);
		}

		#endregion

		void OnGUI()
		{
			if (window == null)
				Init();

			if (CurrentLayout == null)
			{

				string curImgPath = EditorPrefs.GetString("currentImgPath", "");

				if (curImgPath.Length != 0)
				{
					CurrentLayout = DungeonPSession.OpenImageAtPath(curImgPath);
					return;
				}

				if (GUI.Button(new Rect(window.position.width / 2f - 140, window.position.height / 2f - 25, 130, 50), "New Image"))
				{
					DungeonPCreationWindow.Init();
				}
				if (GUI.Button(new Rect(window.position.width / 2f + 10, window.position.height / 2f - 25, 130, 50), "Open Image"))
				{
					CurrentLayout = DungeonPSession.OpenImage();
					return;
				}

				return;
			}

			// Init the textures correctly, won't cost performance if nothing to load
			CurrentLayout.LoadAllTexsFromMaps();

			EditorGUI.DrawRect(window.position, new Color32(30, 30, 30, 255));


			#region Event handling
			Event e = Event.current;    //Init event handler

			//Capture mouse position
			Vector2 mousePos = e.mousePosition;

			// If key is pressed
			if (e.button == 0)
			{

				// Mouse buttons
				if (e.isMouse && mousePos.y > 40 && e.type != EventType.MouseUp)
				{
					if (!DungeonPDrawer.GetLayerPanelRect(window.position).Contains(mousePos))
					{

						if (tool == DPTool.Eraser)
							CurrentLayout.SetPixelByPos(Color.clear, mousePos, CurrentLayout.selectedLayer);
						else if (tool == DPTool.PaintBrush)
							CurrentLayout.SetPixelByPos(selectedColor, mousePos, CurrentLayout.selectedLayer);
						else if (tool == DPTool.BoxBrush)
							Debug.Log("TODO: Add Box Brush tool.");
						else if (tool == DPTool.ColorPicker)
						{
							Vector2 pCoord = CurrentLayout.GetPixelCordinate(mousePos);
							Color? newColor = CurrentLayout.GetBlendedPixel((int)pCoord.x, (int)pCoord.y);
							if (newColor != null && newColor != Color.clear)
							{
								selectedColor = (Color)newColor;
							}
							tool = lasttool;
						}

					}
				}

				// Key down
				if (e.type == EventType.KeyDown)
				{
					if (e.keyCode == KeyCode.W)
					{
						gridOffsetY += 20f;
					}
					if (e.keyCode == KeyCode.S)
					{
						gridOffsetY -= 20f;
					}
					if (e.keyCode == KeyCode.A)
					{
						gridOffsetX += 20f;
					}
					if (e.keyCode == KeyCode.D)
					{
						gridOffsetX -= 20f;
					}

					if (e.keyCode == KeyCode.Alpha1)
					{
						tool = DPTool.PaintBrush;
					}
					if (e.keyCode == KeyCode.Alpha2)
					{
						tool = DPTool.Eraser;
					}
					if (e.keyCode == KeyCode.P)
					{
						lasttool = tool;
						tool = DPTool.ColorPicker;
					}

					if (e.keyCode == KeyCode.UpArrow)
					{
						gridspacing *= 1.2f;
					}
					if (e.keyCode == KeyCode.DownArrow)
					{
						gridspacing *= 0.8f;
						gridspacing -= 2;
					}

				}

				if (e.control)
				{
					if (lasttool == DPTool.Empty)
					{
						lasttool = tool;
						tool = DPTool.Eraser;
					}
				}
				else if (e.type == EventType.KeyUp && e.keyCode == KeyCode.LeftControl)
				{
					if (lasttool != DPTool.Empty)
					{
						tool = lasttool;
						lasttool = DPTool.Empty;
					}
				}
			}

			// TODO: Better way of doing this?
			if (e.type == EventType.ScrollWheel)
			{
				gridspacing -= e.delta.y;
			}
			#endregion

			// DRAW IMAGE
			DungeonPDrawer.DrawImage(CurrentLayout);

			DungeonPDrawer.DrawToolbar(window.position, mousePos);

			DungeonPDrawer.DrawLayerPanel(window.position);

			e.Use();    // Release event handler
		}
	}
}
