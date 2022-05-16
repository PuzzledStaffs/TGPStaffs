using System.Collections;
using System.Collections.Generic;
using TGP.Utilites;
using UnityEngine;

namespace TGP.DungeonEditor
{
    [System.Serializable]
    public class DungeonPRoom : ScriptableObject
    {


       
        public List<SerializableMultiDimensionalArray<GameObject>> ObjectsInRoom =
            new List<SerializableMultiDimensionalArray<GameObject>>();
        public DungeonPRoom(){}
    }
}
