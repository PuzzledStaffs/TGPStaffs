using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DungeonEditorWindow : EditorWindow
{

    [MenuItem("Window/UI Toolkit/Dungeon Editor")]
    public static void ShowExample()
    {
        DungeonEditorWindow wnd = GetWindow<DungeonEditorWindow>();
        wnd.titleContent = new GUIContent("Dungeon Editor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorTools/DungeonEditorWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorTools/DungeonEditorWindow.uss");

        DungeonEditorManipulator manipulator = new DungeonEditorManipulator(rootVisualElement.Q<VisualElement>("object"));

    }
}