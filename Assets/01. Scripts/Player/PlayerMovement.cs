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
    private float jumpForce = 3f;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField]
    private float jumpBufferTime = 0.2f;

    [SerializeField]
    private float coyoteTime = 0.1f;

    [Space(5)]
    [Header("Air Control"), Space(5)]
    [SerializeField]
    private float airControl = 0.5f;


    private Rigidbody rb;

    private Vector2 moveDir;

    private Vector3 groundNormal = Vector3.up;

    private bool isGrounded;

    private bool isMoving = false;

    private float lastJumpPressedTime = Mathf.NegativeInfinity;

    private float lastGroundedTime = Mathf.NegativeInfinity;

    private PlayerStateMachine stateMachine;

    private IdleState idleState;

    private LocomotionState locomotionState;

    private AirborneState airborneState;

    private RetireState retireState;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        stateMachine = new PlayerStateMachine();

        idleState = new IdleState(this, stateMachine);

        locomotionState = new LocomotionState(this, stateMachine);

        airborneState = new AirborneState(this, stateMachine);

        retireState = new RetireState(this, stateMachine);
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

        StickToGround();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            moveDir = ctx.ReadValue<Vector2>();

            isMoving = moveDir.sqrMagnitude > 0.01f;
        }
        else
        {
            isMoving = false;

            moveDir = Vector2.zero;
        }

        if (GameTimeManager.Instance != null)
        {
            GameTimeManager.Instance.SetMoving(isMoving);
        }
        else
        {
            Debug.LogWarning("TimeSystem missing");
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
        if (moveDir == Vector2.zero)
        {
            return;
        }

        // 입력 방향
        Vector3 inputDir = new Vector3(moveDir.x, 0, moveDir.y).normalized;

        // 경사면 내적용(투영)
        //Vector3 moveDirOnSlope = Vector3.ProjectOnPlane(inputDir, groundNormal).normalized;

        Vector3 moveDirFinal;

        float speedMultiplier = 1f;

        if (isGrounded == true)
        {
            moveDirFinal = Vector3.ProjectOnPlane(inputDir, groundNormal).normalized;

            Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;

            float dot = Vector3.Dot(moveDirFinal, slopeDir);

            if (dot > 0.1f)
            {
                // 내려갈 때
                speedMultiplier = 1.3f; // 임시 값 >> 기획에 따라 수정필요
            }
            else if (dot < -0.1f)
            {
                // 올라갈 때
                speedMultiplier = 0.6f; // 임시 값 >> 기획에 따라 수정필요
            }
        }
        else
        {
            moveDirFinal = inputDir;

            speedMultiplier = 1f;
        }
        
        Vector3 velocity = rb.velocity;

        Vector3 targetVelocity = moveDirFinal * moveSpeed * speedMultiplier;

        if (isGrounded == false)
        {
            targetVelocity.x *= airControl;

            targetVelocity.z *= airControl;
        }

        targetVelocity.y = velocity.y;

        rb.velocity = targetVelocity;

        if (moveDirFinal.sqrMagnitude > 0.001f)
        {
            //Vector3 tempMoveDir = new Vector3(moveDirFinal.x, 0, moveDirFinal.y);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirFinal);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime));
        }
    }

    public void Jump()
    {
        bool hasBufferedJump = (Time.time - lastJumpPressedTime) <= jumpBufferTime;

        bool canUseCoyote = (Time.time - lastGroundedTime) <= coyoteTime;

        if (hasBufferedJump == true && (isGrounded == true || canUseCoyote == true))
        {
            Vector3 velocity = rb.velocity;

            Vector3 horizontal = new Vector3(velocity.x, 0, velocity.z);

            if (horizontal.sqrMagnitude > 0.001f)
            {
                horizontal = horizontal.normalized * moveSpeed;
            }

            rb.velocity = new Vector3(horizontal.x, 0f, horizontal.z);

            //velocity.y = 0f;

            //rb.velocity = velocity; 

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

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;

            groundNormal = hit.normal;

            lastGroundedTime = Time.time;
        }
        else
        {
            isGrounded = false;

            groundNormal = Vector3.up;
        }
    }

    private void StickToGround()
    {
        if (isGrounded == false)
        {
            return;
        }

        rb.AddForce(-groundNormal * 30f, ForceMode.Acceleration);
    }

    public bool IsGrounded => isGrounded;

    public bool IsMoving => isMoving;

    public Vector2 MoveDir => moveDir;

    public void ChangeToIdle() => stateMachine.ChangeState(idleState);

    public void ChangeToLocomotion() => stateMachine.ChangeState(locomotionState);

    public void ChangeToAirborne() => stateMachine.ChangeState(airborneState);

    public void ChaneToRetire() => stateMachine.ChangeState(retireState);

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        Vector3 direction = Vector3.down * groundCheckDistance;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(origin, origin + direction);

        Gizmos.DrawSphere(origin + direction, 0.05f);
    }
}
