using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TGP.DungeonEditor
{
    public class DungeonPCreationWindow : EditorWindow
    {
		public static DungeonPCreationWindow window;

		private int xRes = 32, yRes = 32;

		[MenuItem("Assets/Create/DP Image")]
		public static void Init()
		{
			// Get existing open window or if none, make new one
			window = (DungeonPCreationWindow)EditorWindow.GetWindow(typeof(DungeonPCreationWindow));
#if UNITY_4_3
		window.title = "New Image";
#elif UNITY_4_6
		window.title = "New Image";
#else
			window.titleContent = new GUIContent("New Image");
#endif
			window.position = new Rect(Screen.width / 2 + 250 / 2f, Screen.height / 2 - 80, 250, 105);
			window.ShowPopup();
		}

		void OnGUI()
		{
			if (window == null)
				Init();

			GUILayout.Label("DP Image Settings", EditorStyles.boldLabel);

			xRes = Mathf.Clamp(EditorGUILayout.IntField("Width: ", xRes), 1, 256);
			yRes = Mathf.Clamp(EditorGUILayout.IntField("Height: ", yRes), 1, 256);

			EditorGUILayout.Space();

			if (GUILayout.Button("Create", GUILayout.Height(30)))
			{
				this.Close();
				DungeonPSession.CreateImage(xRes, yRes);
			}
		}
	}
}
