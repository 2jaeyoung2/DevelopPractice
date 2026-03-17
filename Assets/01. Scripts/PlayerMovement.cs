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

    [SerializeField]
    private float groundStopDeceleration = 40f;

    [SerializeField]
    private float groundStopEpsilon = 0.05f;


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

    private PlayerStateMachine stateMachine;

    private IdleState idleState;

    private LocomotionState locomotionState;

    private AirborneState airborneState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        stateMachine = new PlayerStateMachine();

        idleState = new IdleState(this, stateMachine);

        locomotionState = new LocomotionState(this, stateMachine);

        airborneState = new AirborneState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.ChangeState(idleState);
    }

    private void Update()
    {
        GroundCheck();

        stateMachine.HandleInput();

        stateMachine.Tick();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedTick();
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
            lastJumpPressedTime = Time.time;
        }
    }

    public void Move()
    {
        if (moveDir != Vector2.zero)
        {
            Vector3 move = (moveSpeed * Time.fixedDeltaTime) * new Vector3(moveDir.x, 0, moveDir.y);

            rb.MovePosition(transform.position + move);

            Vector3 rotateDir = new Vector3(moveDir.x, 0, moveDir.y);

            Quaternion targetRotation = Quaternion.LookRotation(rotateDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        }
    }

    public void Jump()
    {
        bool hasBufferedJump = (Time.time - lastJumpPressedTime) <= jumpBufferTime;

        bool canUseCoyote = (Time.time - lastGroundedTime) <= coyoteTime;

        if (hasBufferedJump == true && (isGrounded == true || canUseCoyote == true))
        {
            Vector3 velocity = rb.velocity;

            velocity.y = 0f;

            rb.velocity = velocity; 

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            lastJumpPressedTime = Mathf.NegativeInfinity;

            lastGroundedTime = Mathf.NegativeInfinity;
        }
    }

    public void StopGroundDrift()
    {
        if (isGrounded == false)
        {
            return;
        }

        Vector3 v = rb.velocity;

        Vector3 horizontal = new Vector3(v.x, 0f, v.z);

        float speed = horizontal.magnitude;

        if (speed <= groundStopEpsilon)
        {
            v.x = 0f;

            v.z = 0f;

            rb.velocity = v;

            return;
        }

        float newSpeed = Mathf.MoveTowards(speed, 0f, groundStopDeceleration * Time.fixedDeltaTime);

        float ratio = newSpeed / speed;

        horizontal.x *= ratio;

        horizontal.z *= ratio;

        v.x = horizontal.x;

        v.z = horizontal.z;

        rb.velocity = v;
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

    public bool IsGrounded => isGrounded;

    public bool IsMoving => isMoving;

    public Vector2 MoveDir => moveDir;

    public void ChangeToIdle() => stateMachine.ChangeState(idleState);

    public void ChangeToLocomotion() => stateMachine.ChangeState(locomotionState);

    public void ChangeToAirborne() => stateMachine.ChangeState(airborneState);

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        Vector3 direction = Vector3.down * groundCheckDistance;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(origin, origin + direction);

        Gizmos.DrawSphere(origin + direction, 0.05f);
    }
}
