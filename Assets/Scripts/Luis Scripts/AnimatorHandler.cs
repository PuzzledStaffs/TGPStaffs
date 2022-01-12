using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wizboyd
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator Anim;
        int vertical;
        int horizontal;
        public bool CanRotate;

        public void Initalize()
        {
            Anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void updateAnimatorValue(float verticalMovement, float HorizontalMovement)
        {
            #region Vertical

            float V = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                V = 0.5f;
            }
            else if(vertical < 0.55f)
            {
                V = 1;
            }
            else if(verticalMovement < 0 && verticalMovement > -0.55f)
            {
                V = -0.5f;
            }
            else if(verticalMovement < -0.55f)
            {
                V = -1;
            }
            else
            {
                V = 0;
            }
            #endregion
            #region Horizontal

            float H = 0;

            if (HorizontalMovement > 0 && HorizontalMovement < 0.55f)
            {
                H = 0.5f;
            }
            else if (vertical < 0.55f)
            {
                H = 1;
            }
            else if (HorizontalMovement < 0 && HorizontalMovement > -0.55f)
            {
                H = -0.5f;
            }
            else if (HorizontalMovement < -0.55f)
            {
                H = -1;
            }
            else
            {
                H = 0;
            }
            #endregion
            Anim.SetFloat(vertical, V, 0.1f, Time.deltaTime);
            Anim.SetFloat(horizontal, H, 0.1f, Time.deltaTime);
        }

        public void canrotate()
        {
            CanRotate = true;
        }

        public void Stoprotation()
        {
            CanRotate = false;
        }
    }
}
