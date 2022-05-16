using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TGP.DungeonEditor.Snapping
{
    public enum SnapPointDirection
    {
        NONE,
        EAST,
        WEST,
        NORTH,
        SOUTH
    }
    
    [SelectionBase]
    public class SnapPoint : MonoBehaviour
    {
        public SnapPoint SnapTarget;

        public SnapPointDirection Direction;
        public SnappableRoom _snappablerom => GetComponentInParent<SnappableRoom>();

        [ContextMenu("SnapToMyTarget")]
        public void SnapToMySnapTarget() => SnapTo(SnapTarget);

        private void SnapTo(SnapPoint other)
        {
            Undo.RecordObject(_snappablerom.transform, "SnapTo");
            var offset = _snappablerom.transform.position - transform.position;
            var newPosition = other.transform.position + offset;
            _snappablerom.transform.position = newPosition;
            
            
        }

        public void AllignTo(SnapPoint other)
        {
            var rotationOffset = transform.rotation.eulerAngles.y - _snappablerom.transform.rotation.eulerAngles.y;
            Undo.RecordObject(_snappablerom.transform, "AlignTo");
            _snappablerom.transform.rotation = other.transform.rotation;
            _snappablerom.transform.Rotate(0, 180, 0);
            _snappablerom.transform.Rotate(0, -rotationOffset, 0);
        }

        [ContextMenu("Align and Snap To Me &m")]
        public void SnapAndAlignOtherToMe()
        {
            if (SnapTarget != null)
            {
                SnapTarget.AllignTo(this);
                SnapTarget.SnapTo(this);
            }
        }

        [ContextMenu("Connect 2 SnapPoints")]
        public void Connect2()
        {
            if (Selection.objects.Length != 2)
            {
                Debug.LogError("Connect 2 requires 2 snap points to be selected");
                return;
            }

            var snap01 = Selection.objects[0];
            var snap1 = (snap01 as GameObject)?.GetComponent<SnapPoint>();
            var snap02 = Selection.objects[1];
            var snap2 = (snap02 as GameObject)?.GetComponent<SnapPoint>();
            if (snap1 && snap2)
            {
                snap2.SnapTarget = snap1;
                snap1.SnapTarget = snap2;
            }
        }


        [ContextMenu("Snap And Align To Other")]
        public void SnapAndAlign()
        {
            if (SnapTarget != null)
            {
                AllignTo(SnapTarget);
                SnapTo(SnapTarget);
            }
        }

        public void JustSnap()
        {
            if (SnapTarget != null)
            {
                SnapTarget.SnapTo(this);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = UnityEngine.Color.green;
            if (SnapTarget != null)
            {
                Gizmos.DrawLine(transform.position, SnapTarget.transform.position);
            }
        }

        private void OnValidate()
        {
            if (SnapTarget == null)
            {
                SnapTarget = null;
            }

            bool showVisuals = true;
            foreach (var child in transform.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "Snap Point Visualizer")
                {
                    child.gameObject.SetActive(showVisuals);
                }
            }
        }

        public SnapPoint FindNearestOther(IEnumerable<SnapPoint> others, float maxDistance = 10f)
        {
            SnapPoint closest = null;

            var closestDistance = float.MaxValue;

            foreach (var other in others)
            {
                if (other == this || other._snappablerom == _snappablerom)
                    continue;
                if (other.SnapTarget != null)
                    continue;

                var dist = Vector3.Distance(transform.position, other.transform.position);
                if (dist > maxDistance)
                    continue;

                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = other;
                }
            }

            return closest;
        }
    }
}
