using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDungeon : ScriptableObject
{
    public int NumberOfDoors;
    [HideInInspector] public NodeView AssociatedNode;
    public List<DungeonDoor> DungeonDoors = new List<DungeonDoor>();


}

public class DungeonDoor: ScriptableObject
{
    public GameObject Entrance; 
    public GameObject Exit;

}
