using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private Rigidbody rb;

    private Vector2 moveDir;

    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            moveDir = ctx.ReadValue<Vector2>();
        }
        else
        {
            moveDir = Vector2.zero;
        }
    }

    private void Update()
    {
        if (moveDir != Vector2.zero)
        {

        }
    }
}
