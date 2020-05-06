using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PK
{
    public class PlayerInputWrapper : MonoBehaviour
    {
        private Vector2Int inputDirection = Vector2Int.zero;
        private BaseActor actor;

        private void Awake()
        {
            actor = GetComponent<BaseActor>();
        }

        public void UpdateMovementInput(InputAction.CallbackContext c)
        {
            Vector2 rawInput = c.ReadValue<Vector2>();
            inputDirection = new Vector2Int((int)rawInput.x, (int)rawInput.y);
        }

        void FixedUpdate()
        {
            if (inputDirection != Vector2.zero)
            {
                actor.Move(inputDirection);
            }
            else
            {
                actor.StopAnimation();
            }
        }
    }
}
