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

    private bool isGrounded;

    private bool isMoving = false;

    private bool isJumpPressed;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            isMoving = true;

            moveDir = ctx.ReadValue<Vector2>();
        }
        else
        {
            isMoving = false;

            moveDir = Vector2.zero;

            rb.velocity = Vector3.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            // TODO: มกวม
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Jump()
    {

    }

    private void Move()
    {
        if (moveDir != Vector2.zero)
        {
            moveSpeed = 0.1f;

            rotateSpeed = 10f;

            Vector3 move = moveSpeed * new Vector3(moveDir.x, 0, moveDir.y);

            rb.MovePosition(transform.position + move);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }
}
