using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DungeonRandomizer))]
public class DungeonArchitect : MonoBehaviour
{
    [Header("Dungeon Themes")]
    [SerializeField] DungeonTheme[] DungeonThemes;
    [SerializeField] bool DebugDraw;
}
