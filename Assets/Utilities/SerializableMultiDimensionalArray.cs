using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;

namespace TGP.Utilites
{
    [System.Serializable]
    public struct Package2D<T>
    {
        public int Index0;
        public int Index1;
        public T Element;

        public Package2D(int inx0, int inx1, T element)
        {
            Index0 = inx0;
            Index1 = inx1;
            Element = element;
        }
    }

    [System.Serializable]
    public class SerializableMultiDimensionalArray<T> : ISerializationCallbackReceiver
    {
        public T[,] Unserializable;
        private int x, y;

        [SerializeField] public List<Package2D<T>> serializable;

        public SerializableMultiDimensionalArray(T[,] array)
        {
            Unserializable = array;
            x = array.GetLength(0);
            y = array.GetLength(1);
        }

        public void OnBeforeSerialize()
        {
            serializable = new List<Package2D<T>>();
            for (int i = 0; i < Unserializable.GetLength(0); i++)
            {
                for (int j = 0; j < Unserializable.GetLength(1); j++)
                {
                    serializable.Add(new Package2D<T>(i, j, Unserializable[i, j]));
                }
            }
        }

        public void OnAfterDeserialize()
        {
            Unserializable = new T[x,y];
            foreach (var package in serializable)
            {
                Unserializable[package.Index0, package.Index1] = package.Element;
            }
        }
    }
}