using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using TGP.Utilites;

namespace TGP.DungeonEditor
{
    public class DungeonPSession
    {
		public static DungeonPLayout CreateImage(int w, int h)
		{
			string path = EditorUtility.SaveFilePanel("Create UPAImage",
													   "Assets/", "Pixel Image.asset", "asset");
			if (path == "")
			{
				return null;
			}

			path = FileUtil.GetProjectRelativePath(path);

			DungeonPLayout img = ScriptableObject.CreateInstance<DungeonPLayout>();
			AssetDatabase.CreateAsset(img, path);

			AssetDatabase.SaveAssets();

			img.Init(w, h);
			EditorUtility.SetDirty(img);
			DungeonPEditorWindow.CurrentLayout = img;

			EditorPrefs.SetString("currentImgPath", AssetDatabase.GetAssetPath(img));

			if (DungeonPEditorWindow.window != null)
				DungeonPEditorWindow.window.Repaint();
			else
				DungeonPEditorWindow.Init();

			img.gridspacing = 10 - Mathf.Abs(img.Width - img.Height) / 100f;
			return img;
		}

		public static DungeonPLayout OpenImage()
		{
			string path = EditorUtility.OpenFilePanel(
				"Find an Image (.asset | .png | .jpg)",
				"Assets/",
				"Image Files;*.asset;*.jpg;*.png");

			if (path.Length != 0)
			{
				// Check if the loaded file is an Asset or Image
				if (path.EndsWith(".asset"))
				{
					path = FileUtil.GetProjectRelativePath(path);
					DungeonPLayout img = AssetDatabase.LoadAssetAtPath(path, typeof(DungeonPLayout)) as DungeonPLayout;
					EditorPrefs.SetString("currentImgPath", path);
					return img;
				}
				else
				{
					// Load Texture from file
					Texture2D tex = LoadImageFromFile(path);
					// Create a new Image with textures dimensions
					DungeonPLayout img = CreateImage(tex.width, tex.height);
					// Set pixel colors
					img.layers[0].tex = tex;
					img.layers[0].tex.filterMode = FilterMode.Point;
					img.layers[0].tex.Apply();
					for (int x = 0; x < img.Width; x++)
					{
						for (int y = 0; y < img.Height; y++)
						{
							img.layers[0].map[x + y * tex.width] = tex.GetPixel(x, tex.height - 1 - y);
						}
					}
				}
			}

			return null;
		}

		public static Texture2D LoadImageFromFile(string path)
		{
			Texture2D tex = null;
			byte[] fileData;
			if (File.Exists(path))
			{
				fileData = File.ReadAllBytes(path);
				tex = new Texture2D(2, 2);
				tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			}
			return tex;
		}

		public static DungeonPLayout OpenImageByAsset(DungeonPLayout img)
		{

			if (img == null)
			{
				Debug.LogWarning("Image is null. Returning null.");
				EditorPrefs.SetString("currentImgPath", "");
				return null;
			}

			string path = AssetDatabase.GetAssetPath(img);
			EditorPrefs.SetString("currentImgPath", path);

			return img;
		}

		public static DungeonPLayout OpenImageAtPath(string path)
		{
			if (path.Length != 0)
			{
				DungeonPLayout img = AssetDatabase.LoadAssetAtPath(path, typeof(DungeonPLayout)) as DungeonPLayout;

				if (img == null)
				{
					EditorPrefs.SetString("currentImgPath", "");
					return null;
				}

				EditorPrefs.SetString("currentImgPath", path);
				return img;
			}

			return null;
		}

		public static void ExportKeyData(Dictionary<Color, GUID> KeyData)
        {

        }

		public static bool ExportImage(DungeonPLayout img, TextureType type, TextureExtension extension)
		{
			string path = EditorUtility.SaveFilePanel(
				"Export image as " + extension.ToString(),
				"Assets/",
				img.name + "." + extension.ToString().ToLower(),
				extension.ToString().ToLower());

			if (path.Length == 0)
				return false;

			byte[] bytes;
			if (extension == TextureExtension.PNG)
			{
				// Encode texture into PNG
				bytes = img.GetFinalImage(true).EncodeToPNG();
			}
			else
			{
				// Encode texture into JPG

#if UNITY_4_2
			bytes = img.GetFinalImage(true).EncodeToPNG();
#elif UNITY_4_3
			bytes = img.GetFinalImage(true).EncodeToPNG();
#elif UNITY_4_5
			bytes = img.GetFinalImage(true).EncodeToJPG();
#else
				bytes = img.GetFinalImage(true).EncodeToJPG();
#endif
			}

			path = FileUtil.GetProjectRelativePath(path);

			//Write to a file in the project folder
			File.WriteAllBytes(path, bytes);
			AssetDatabase.Refresh();

			TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;

			if (type == TextureType.texture)
				texImp.textureType = TextureImporterType.Default;
			else if (type == TextureType.sprite)
			{
				texImp.textureType = TextureImporterType.Sprite;

#if UNITY_4_2
			texImp.spritePixelsToUnits = 10;
#elif UNITY_4_3
			texImp.spritePixelsToUnits = 10;
#elif UNITY_4_5
			texImp.spritePixelsToUnits = 10;
#else
				texImp.spritePixelsPerUnit = 10;
#endif
			}

			texImp.filterMode = FilterMode.Point;
			texImp.textureFormat = TextureImporterFormat.AutomaticTruecolor;

			AssetDatabase.ImportAsset(path);

			return true;
		}

		public static bool ExportImageAsScriptableObject(DungeonPLayout img)
		{
			DungeonPRoom room = ScriptableObject.CreateInstance<DungeonPRoom>();
			EditorUtility.SetDirty(room);

			foreach (var layer in img.layers)
			{
			
				GameObject[,] Array2D = new GameObject[layer.tex.width, layer.tex.height];
				for (int x = 0; x < layer.tex.width; x++)
				{
					for (int z = 0; z < layer.tex.height; z++)
					{
						Color XZColor = layer.tex.GetPixel(x, z);
						if (layer.ParentLayout.KeyData.ContainsKey(XZColor))
						{
							GameObject Object;
							if (layer.ParentLayout.KeyData.TryGetValue(XZColor, out Object))
							{
								Array2D[x, z] = Object;
							}
							else
							{
								Array2D[x, z] = null;
								Debug.Log($"{x}, {z} is null");
							}
						}
						else
						{
							Array2D[x, z] = null;
						}
					}
				}
				
				room.ObjectsInRoom.Add(new SerializableMultiDimensionalArray<GameObject>(Array2D)); //ADD THIS SOME MULTIDIMENSIONAL ARRAY
			}
			
			Debug.Log(room.ObjectsInRoom.Count);

			string Location = EditorUtility.SaveFilePanel(
				"Export image as .asset",
				"Assets/",
				img.name + ".asset",
				"asset");
			string path = FileUtil.GetProjectRelativePath(Location);
			AssetDatabase.CreateAsset(room,path);
			AssetDatabase.SaveAssets();
			return Directory.Exists(Location);
		}
	}
}
