using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TGP.DungeonEditor
{
	public class DungeonPLayerSettings : EditorWindow
	{
		public static DungeonPLayerSettings window;

		public DungeonPLayer layer;

		private new string name;

		public static void Init(DungeonPLayer layer)
		{
			// Get existing open window or if none, make new one
			window = (DungeonPLayerSettings)EditorWindow.GetWindow(typeof(DungeonPLayerSettings));
#if UNITY_4_3
		window.title = layer.name + " - Settings";
#elif UNITY_4_6
		window.title = layer.name + " - Settings";
#else
			window.titleContent = new GUIContent(layer.name + " - Settings");
#endif

			window.position = new Rect(Screen.width / 2 + 260 / 2f, Screen.height / 2 - 80, 360, 170);
			window.ShowPopup();

			window.layer = layer;
		}

		void OnGUI()
		{
			// Edit name and visibility
			GUILayout.Label("General", EditorStyles.boldLabel);
			layer.name = EditorGUILayout.TextField("Name: ", layer.name);
			layer.enabled = EditorGUILayout.Toggle("Enabled: ", layer.enabled);
			//exportImg = (UPAImage)EditorGUILayout.ObjectField (exportImg, typeof(UPAImage), false);

			// Edit blend mode and opacity
			GUILayout.Label("Blending", EditorStyles.boldLabel);
			layer.mode = (DungeonPLayer.BlendMode)EditorGUILayout.EnumPopup("Mode: ", layer.mode);
			if (layer.mode != DungeonPLayer.BlendMode.NORMAL)
			{
				GUILayout.Label("Some blend modes are still in testing and might not produce\nentirely accurate results.");
			}
			layer.Opacity = EditorGUILayout.IntSlider("Opacity: ", Mathf.RoundToInt(layer.Opacity * 100), 0, 100) / 100f;
		}
	}
}
