using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon", menuName = "Map/Dungeon", order = 5000)]
public class Dungeon : ScriptableObject
{
    public RoomArea[,] Rooms;
}
