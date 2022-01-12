using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wizboyd
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform CameObject;
        InputHandler InputHandler;
        Vector3 MoveDir;

        [HideInInspector]
        public Transform MyTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody RB;
        public GameObject NormalCamera;

        [Header("Stats")]
        [SerializeField] float MovementSpeed = 5;
        [SerializeField] float RotationSpeed = 10;

        // Start is called before the first frame update
        void Start()
        {
            RB = GetComponent<Rigidbody>();
            InputHandler = GetComponent<InputHandler>();
            CameObject = Camera.main.transform;
            MyTransform = transform;
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            animatorHandler.Initalize();
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            InputHandler.TickInput(delta);

            MoveDir = CameObject.forward * InputHandler.Vertical;
            MoveDir += CameObject.right * InputHandler.Horizontal;
            MoveDir.Normalize();

            float Speed = MovementSpeed;
            MoveDir *= Speed;

            Vector3 Projectedvelocity = Vector3.ProjectOnPlane(MoveDir, NoramlVector);
            RB.velocity = Projectedvelocity;

            if (animatorHandler.CanRotate)
            {
                HandleRotation(delta);
            }
        }

        #region Movement

        Vector3 NoramlVector;
        Vector3 TargetPos;

        private void HandleRotation(float delta)
        {
            Vector3 TargetDir = Vector3.zero;
            float moveOverrid = InputHandler.MoveAmount;

            TargetDir = CameObject.forward * InputHandler.Vertical;
            TargetDir += CameObject.right * InputHandler.Horizontal;
            TargetDir.Normalize();
            TargetDir.y = 0;

            if (TargetDir == Vector3.zero) { }
            {
                TargetDir = MyTransform.forward;
            }

            float RS = RotationSpeed;

            Quaternion TR = Quaternion.LookRotation(TargetDir);
            Quaternion TargetRotation = Quaternion.Slerp(MyTransform.rotation, TR, RS * delta);

            MyTransform.rotation = TargetRotation;
        }

        #endregion
    }
}

