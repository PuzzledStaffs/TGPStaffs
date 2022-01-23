using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    [SerializeField] private int NumberOfRooms;
    [SerializeField] private int EndRoomDistance;

   [SerializeField] private Room[,] rooms;

   [SerializeField] private Room CurrentRoom;

    private static DungeonGeneration instance = null;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            this.CurrentRoom = GenerateDungeon();
        }
        else
        {
            string roomprefabname = instance.CurrentRoom.PrefabName();
            GameObject roomobject = (GameObject)Instantiate(Resources.Load(roomprefabname));
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //this.CurrentRoom = GenerateDungeon();
        string RoomPrefabName = this.CurrentRoom.PrefabName();
        GameObject RoomObject = (GameObject)Instantiate(Resources.Load(RoomPrefabName));
    }

    private Room GenerateDungeon()
    {
        int Gridsize = 3 * NumberOfRooms;

        rooms = new Room[Gridsize, Gridsize];

        Vector2Int InitRoomCoordinate = new Vector2Int((Gridsize / 2) - 1, (Gridsize / 2) - 1);

        Queue<Room> RoomsToCreate = new Queue<Room>();
        RoomsToCreate.Enqueue(new Room(InitRoomCoordinate.x, InitRoomCoordinate.y));
        List<Room> CreatedRooms = new List<Room>();
        while (RoomsToCreate.Count > 0 && CreatedRooms.Count < NumberOfRooms)
        {
            Room CurrentRoom = RoomsToCreate.Dequeue();
            this.rooms[CurrentRoom.RoomCoordinates.x, CurrentRoom.RoomCoordinates.y] = CurrentRoom;
            CreatedRooms.Add(CurrentRoom);
            AddNeighbors(CurrentRoom, RoomsToCreate);
        }

            foreach (Room r in CreatedRooms)
            {
                List<Vector2Int> NeighborCoordinates = r.NeighborCoordinates();
                foreach (Vector2Int I in NeighborCoordinates)
                {
                    Room Neighbor = this.rooms[I.x, I.y];
                    if (Neighbor != null)
                    {
                        r.Connect(Neighbor);
                    }
                }
            }
            return this.rooms[InitRoomCoordinate.x, InitRoomCoordinate.y];
        }


    private void AddNeighbors(Room CurrentRoom, Queue<Room> RoomsToCreate)
    {
        List<Vector2Int> NeighborCoords = CurrentRoom.NeighborCoordinates();
        List<Vector2Int> AvalibleNeighbors = new List<Vector2Int>();
        foreach (Vector2Int Coord in NeighborCoords)
        {
            if (this.rooms[Coord.x, Coord.y] == null)
            {
                AvalibleNeighbors.Add(Coord);
            }
        }

        int NumOfNeighbors = Random.Range(1, AvalibleNeighbors.Count);//Check

        for (int neighborindex = 0; neighborindex < NumOfNeighbors; neighborindex++)
        {
            float RandomNumber = Random.value;
            float RoomFrac = 1f / (float)AvalibleNeighbors.Count;
            Vector2Int ChosenNeighbor = new Vector2Int(0, 0);
            foreach (Vector2Int coord in AvalibleNeighbors)
            {
                if (RandomNumber < RoomFrac)
                {
                    ChosenNeighbor = coord;
                    break;
                }
                else
                {
                    RoomFrac += 1f / (float)AvalibleNeighbors.Count;
                }
            }
            RoomsToCreate.Enqueue(new Room(ChosenNeighbor));
            AvalibleNeighbors.Remove(ChosenNeighbor);
        }
    }

    public Room currentroom()
    {
        return this.CurrentRoom;
    }

    public void MoveToRoom(Room room)
    {
        this.CurrentRoom = room;
    }
}


public class DungeonGeneration1
{

}

[System.Serializable]
public class Room
{
    public DungeonRoomType RT;
    public Vector2Int RoomCoordinates;
    public Dictionary<string, Room> neihboringrooms;

    public Room(int Xcor, int YCor)
    {
        this.RoomCoordinates = new Vector2Int(Xcor, YCor);
        this.neihboringrooms = new Dictionary<string, Room>();
    }

    public Room(Vector2Int RoomCoordinates)
    {
        this.RoomCoordinates = RoomCoordinates;
        this.neihboringrooms = new Dictionary<string, Room>();
    }

    public string PrefabName()
    {
        string name = "Room_";
        foreach (KeyValuePair<string,Room> neighborpair in neihboringrooms)
        {
            name += neighborpair.Key;
        }
        return name;
    }

    public List<Vector2Int> NeighborCoordinates()
    {
        List<Vector2Int> NeighborCoordinates = new List<Vector2Int>();
        NeighborCoordinates.Add(new Vector2Int(this.RoomCoordinates.x, this.RoomCoordinates.y - 1));
        NeighborCoordinates.Add(new Vector2Int(this.RoomCoordinates.x + 1, this.RoomCoordinates.y));
        NeighborCoordinates.Add(new Vector2Int(this.RoomCoordinates.x, this.RoomCoordinates.y + 1));
        NeighborCoordinates.Add(new Vector2Int(this.RoomCoordinates.x - 1, this.RoomCoordinates.y));

        return NeighborCoordinates;
    }

    public void Connect(Room neighbor)
    {
        string Direction = "";
        if (neighbor.RoomCoordinates.y < this.RoomCoordinates.y)
        {
            Direction = "N";
        }
        if (neighbor.RoomCoordinates.x > this.RoomCoordinates.x)
        {
            Direction = "E";
        }
        if (neighbor.RoomCoordinates.y > this.RoomCoordinates.y)
        {
            Direction = "S";
        }
        if (neighbor.RoomCoordinates.x < this.RoomCoordinates.x)
        {
            Direction = "W";
        }
        this.neihboringrooms.Add(Direction, neighbor);
    }

    public Room Neighbor(string direction)
    {
        return this.neihboringrooms[direction];
    }


}
public enum DungeonRoomType
{
    StartRoom,
    EndRoom,
    TransitionRoom
}
