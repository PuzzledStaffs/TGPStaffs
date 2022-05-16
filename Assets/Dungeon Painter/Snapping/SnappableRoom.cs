using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TGP.DungeonEditor.Snapping
{
    public class SnappableRoom : MonoBehaviour
    {
        [SerializeField] public bool LockInPlace;
        [SerializeField] public SnapPoint[] Points;
        [SerializeField] private Vector3 _lastPostion;

        [MenuItem("Tools/SnapRoom &K")]
        public static void SnapRoom()
        {
            var points = Selection.activeGameObject.GetComponentsInChildren<SnapPoint>()
                .Where(t => t.SnapTarget != null).FirstOrDefault();
            points.SnapAndAlign();
            
        }

        [ContextMenu("Clear Snaps")]
        public void ClearSnaps()
        {
            foreach (var snap in Points)
            {
                if (snap.SnapTarget != null)
                {
                    snap.SnapTarget.SnapTarget = null;
                    snap.SnapTarget = null;
                }
            }
        }

        private void OnValidate() => Points = GetComponentsInChildren<SnapPoint>();

        private void Update()
        {
            if (LockInPlace && Vector3.Distance(transform.position, _lastPostion) > 0.1f)
            {
                _lastPostion = transform.position;
                var alreadymoved = new HashSet<SnappableRoom>();
                movechildren(alreadymoved);
            }
        }

        private void movechildren(HashSet<SnappableRoom> alreadymoved)
        {
            alreadymoved.Add(this);
            foreach (var snappoint in Points)
            {
                if (snappoint.SnapTarget != null)
                {
                    snappoint.SnapTarget.SnapAndAlign();
                    if (alreadymoved.Contains(snappoint.SnapTarget._snappablerom) == false)
                    {
                        snappoint.SnapTarget._snappablerom.movechildren(alreadymoved);
                    }
                }
            }
        }

        public static SnappableRoom SpawnRoom(string name)
        {
            var settings = Resources.Load<RPGWorldSettings>("Settings");
            var prefab = settings.SnappableRooms.FirstOrDefault(t => t.name == name);

            var instance = PrefabUtility.InstantiatePrefab((prefab.gameObject));
            Debug.Log($"Instance {instance}");
            var room = ((GameObject) instance).GetComponent<SnappableRoom>();
            return room;
        }

        [ContextMenu("snap Children")]
        void snapChildren()
        {
            var allpoints = FindObjectsOfType<SnapPoint>();
            foreach (var snap in Points)
            {
                var nearest = snap.FindNearestOther(allpoints);
                if (nearest != null)
                {
                    snap.SnapTarget = nearest;
                    nearest.SnapTarget = snap;
                }
            }
        }

        public SnapPoint NextOpenPoint()
        {
            var point = Points.FirstOrDefault(t => t.SnapTarget == null);
            if (point == null)
            {
                return null;
            }

            return point;
        }

        public static SnappableRoom ChooseRandom()
        {
            var all = FindObjectsOfType<SnappableRoom>();
            var index = Random.Range(0, all.Length);
            return all[index];
        }
    }
}
