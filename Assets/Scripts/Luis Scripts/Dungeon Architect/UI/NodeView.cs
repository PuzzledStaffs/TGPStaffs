using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class NodeView : Node
{

    public Action<NodeView> OnNodeSelected;

    public RoomDungeon room;
    public Dictionary<Port, DungeonDoor> InputPorts = new Dictionary<Port, DungeonDoor>();
    public Dictionary<Port, DungeonDoor> OutputPorts = new Dictionary<Port, DungeonDoor>();

    public NodeView(RoomDungeon room)
    {
        this.room = room;
        this.title = "Dungeon_Room";

        //InputPorts = new Port[];
        //OutputPorts = new Port[];

        //CreateInputPorts();
        //CreateOutputPorts();

    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        evt.menu.AppendAction("Add Entrance-Exit", (a) => CreatePorts(new DungeonDoor()));
        evt.menu.AppendAction("Remove Entrance-Exit", (a) => RemovePorts());
        
    }

    public void CreatePorts(DungeonDoor door)
    {
        CreateInputPorts(door);
        CreateOutputPorts(door);
    }

    public void RemovePorts()
    {
        inputContainer.RemoveAt(inputContainer.childCount - 1);
        outputContainer.RemoveAt(outputContainer.childCount - 1);

        //InputPorts.RemoveAt(InputPorts.Count - 1);
        //OutputPorts.RemoveAt(OutputPorts.Count - 1);
    }

    private void CreateOutputPorts(DungeonDoor door)
    {
    
            Port port = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            if (port != null)
            {
                port.portName = "";
                port.portColor = Color.red;
                outputContainer.Add(port);
            OutputPorts.Add(port, door);
            }
        
    }

    private void CreateInputPorts(DungeonDoor door)
    {
       
            Port port = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            if (port != null)
            {
                port.portName = "";
                port.portColor = Color.green;
                inputContainer.Add(port);
                InputPorts.Add(port, door);
            }
        
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }
}
