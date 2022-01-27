using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine;
using System;

public class DungeonThemeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory: UxmlFactory<DungeonThemeView, GraphView.UxmlTraits> { };

    public DungeonThemeView()
    {
        Insert(0,new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Luis Scripts/Dungeon Architect/UI/DungeonArchitectThemeEditor.uss");
        styleSheets.Add(stylesheet);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            
            evt.menu.AppendAction("Create Room", (a) => CreateDungeonRoom(typeof(RoomDungeon))); //Don't actually need typeof unless we are deriving dungeonroom class aka having diffrent types of rooms
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endport =>
        endport.direction != startPort.direction && endport.node != startPort.node).ToList();
    }

    void CreateDungeonRoom(System.Type type) //Irellavent parameter
    {
        RoomDungeon room = new RoomDungeon();
        CreateNodeView(room); // Honestly Might want to change it so there is an inspector in the editor for changeing room count

    }

   public void CreateNodeView(RoomDungeon room)
    {
        NodeView nodeview = new NodeView(room);
        room.AssociatedNode = nodeview;
        nodeview.OnNodeSelected = OnNodeSelected;
        AddElement(nodeview);
    }

    //private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    //{
    //    if (graphViewChange.edgesToCreate != null)
    //    {
    //        graphViewChange.edgesToCreate.ForEach(edge =>
    //        {

    //            NodeView Entrance = edge.input.node as NodeView;
    //            NodeView Exit = edge.output.node as NodeView;

    //            Entrance.room.DungeonDoors.Add();

                
                

    //        });
    //    }
    //}
}

//public class DungeonRoomCountWindow: EditorWindow
//{
//    public int count;


//    public static DungeonRoomCountWindow ShowWindow()
//    {
//       return (DungeonRoomCountWindow) GetWindow(typeof(DungeonRoomCountWindow));
//    }

//    private void OnGUI()
//    {
//        count = EditorGUILayout.IntField(0);

//        if (GUILayout.Button("Confirm Room Numbers"))
//        {
//            GUIUtility.ExitGUI();
//        }
//    }
//}