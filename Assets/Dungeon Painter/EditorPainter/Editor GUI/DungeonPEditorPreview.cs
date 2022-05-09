using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TGP.DungeonEditor
{
    [CustomEditor(typeof(DungeonPLayout))]
    public class DungeonPEditorPreview : Editor
    {
		public override void OnInspectorGUI()
		{
			DungeonPLayout img = (DungeonPLayout)target;

			GUILayout.BeginArea(new Rect(5, 53, Screen.width - 10, Screen.height));

			//if (GUILayout.Button("Open", GUILayout.Height(40)))
			//{
			//	DungeonPEditorWindow.CurrentLayout = DungeonPSession.OpenImageByAsset(img);
			//	if (DungeonPEditorWindow.window != null)
			//	{
			//		DungeonPEditorWindow.window.Repaint();
			//	}
			//}

			//if (GUILayout.Button("Export", GUILayout.Height(40)))
			//{
			//	DungeonPExportWindow.Init(img);
			//}

			

			GUILayout.EndArea();

			//Make sure the textures are loaded
			//img.LoadAllTexsFromMaps();



			//float ratio = (float)img.Width / (float)img.Height;
			//EditorGUI.DrawTextureTransparent(new Rect(5, 150, Screen.width - 10, (Screen.width - 10) * ratio), img.GetFinalImage(true), ScaleMode.ScaleToFit, 0);
			DrawDefaultInspector();
		}
	}
}
