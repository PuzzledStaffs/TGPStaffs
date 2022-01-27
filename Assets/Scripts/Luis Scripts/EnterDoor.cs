using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
   [SerializeField] string Direction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
            DungeonGeneration dungeonGeneration = dungeon.GetComponent<DungeonGeneration>();
            Room room = dungeonGeneration.currentroom();
            dungeonGeneration.MoveToRoom(room.Neighbor(this.Direction));
            SceneManager.LoadScene("LuisScene");
        }
    }
}
