using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class InspectorView : VisualElement
{

    public new class UXMLFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    Editor editor;

    public InspectorView()
    {

    }

    internal void UpdateSelection(NodeView node)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(node.room);
        
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);
    }
}
