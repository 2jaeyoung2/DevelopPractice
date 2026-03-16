using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement"), Space(5)]
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotateSpeed;


    [Space(5)]
    [Header("Jump"), Space(5)]
    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField]
    private float jumpBufferTime = 0.2f;

    [SerializeField]
    private float coyoteTime = 0.1f;


    private Rigidbody rb;

    private Vector2 moveDir;

    private bool isGrounded;

    private bool isMoving = false;

    private float lastJumpPressedTime = Mathf.NegativeInfinity;

    private float lastGroundedTime = Mathf.NegativeInfinity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Move();

        Jump();
    }

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
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            lastJumpPressedTime = Time.time; // 점프 입력 시각 기록 (점프 버퍼)
        }
    }

    private void Move()
    {
        if (moveDir != Vector2.zero)
        {
            Vector3 move = moveSpeed * new Vector3(moveDir.x, 0, moveDir.y);

            rb.MovePosition(transform.position + move);

            Vector3 rotateDir = new Vector3(moveDir.x, 0, moveDir.y);

            Quaternion targetRotation = Quaternion.LookRotation(rotateDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    private void Jump()
    {
        // 최근 jumpBufferTime 안에 점프 입력이 있었는지 확인
        bool hasBufferedJump = (Time.time - lastJumpPressedTime) <= jumpBufferTime;

        // 코요테 조건
        bool canUseCoyote = (Time.time - lastGroundedTime) <= coyoteTime;

        if (hasBufferedJump == true && (isGrounded == true || canUseCoyote == true))
        {
            Vector3 velocity = rb.velocity;

            velocity.y = 0f;

            rb.velocity = velocity; 

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // 점프를 이미 사용했으니 버퍼 초기화
            lastJumpPressedTime = Mathf.NegativeInfinity;

            lastGroundedTime = Mathf.NegativeInfinity;
        }
    }

    private void GroundCheck()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded == true)
        {
            lastGroundedTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        Vector3 direction = Vector3.down * groundCheckDistance;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(origin, origin + direction);

        Gizmos.DrawSphere(origin + direction, 0.05f);
    }
}
