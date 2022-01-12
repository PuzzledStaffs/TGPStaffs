using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wizboyd
{
    public class InputHandler : MonoBehaviour
    {
        public float Horizontal;
        public float Vertical;
        public float MoveAmount;
        public float MouseX;
        public float MouseY;

        PlayerControls InputActions;

        Vector2 MovementInput;
        Vector2 CameraInput;

        public void OnEnable()
        {
            if (InputActions == null)
            {
                InputActions = new PlayerControls();
                InputActions.PlayerMovement.Movement.performed += InputActions => MovementInput = InputActions.ReadValue<Vector2>();
                InputActions.PlayerMovement.Camera.performed += i => CameraInput = i.ReadValue<Vector2>();
            }

            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        private void MoveInput(float delta)
        {
            Horizontal = MovementInput.x;
            Vertical = MovementInput.y;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
            MouseX = CameraInput.x;
            MouseY = CameraInput.y;
        }
    }
}
