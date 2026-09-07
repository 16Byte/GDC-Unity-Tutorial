using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PLACEHOLDER. Throwaway physics controller so the shooting tutorial
/// has something to walk around with. It is deliberately the dumbest
/// thing that works, and it is meant to be deleted the moment the real
/// controller lands.
///
/// Drop it on a primitive Capsule that has a Rigidbody, and child the
/// Main Camera to that capsule at roughly head height.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlaceholderPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpHeight = 1.2f;

    [Header("Look")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float maxPitch = 85f;

    [Header("Ground check")]
    [SerializeField] private LayerMask groundLayers = ~0;
    [SerializeField] private float groundCheckDistance = 0.15f;

    private Rigidbody body;
    private CapsuleCollider capsule;

    // Where the player is looking. Yaw turns the whole capsule so that
    // "forward" means the same thing for movement and for the weapon
    // raycast. Pitch only tilts the camera, because a capsule that
    // leans back is a capsule that falls over.
    private float pitch;

    private Vector2 moveInput;
    private bool jumpQueued;
    private bool isGrounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        // Physics must never spin the capsule. Rotation is ours alone.
        body.freezeRotation = true;

        // Movement happens in FixedUpdate, so interpolate for a smooth
        // picture at framerates that do not match the physics step.
        body.interpolation = RigidbodyInterpolation.Interpolate;

        if (cameraPivot == null && Camera.main != null)
            cameraPivot = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Reading devices directly keeps this script self-contained:
        // no action asset to wire up, nothing to break during the demo.
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        ReadMoveInput(keyboard);

        // Jump is an edge, not a state, so latch it here and spend it
        // in FixedUpdate. Polling it there would drop presses.
        if (keyboard.spaceKey.wasPressedThisFrame)
            jumpQueued = true;

        Look(mouse.delta.ReadValue());
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        Move();

        if (jumpQueued)
        {
            if (isGrounded)
                Jump();

            jumpQueued = false;
        }
    }

    private void ReadMoveInput(Keyboard keyboard)
    {
        float x = 0f;
        float y = 0f;

        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.sKey.isPressed) y -= 1f;
        if (keyboard.wKey.isPressed) y += 1f;

        // Normalize so walking diagonally is not faster than walking straight.
        moveInput = new Vector2(x, y);
        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();
    }

    private void Look(Vector2 mouseDelta)
    {
        // Mouse delta is already "how far since the last frame",
        // so it must NOT be multiplied by Time.deltaTime.
        transform.Rotate(Vector3.up, mouseDelta.x * mouseSensitivity);

        pitch = Mathf.Clamp(pitch - mouseDelta.y * mouseSensitivity, -maxPitch, maxPitch);

        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Move()
    {
        Vector3 wish = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Set the horizontal velocity outright and leave Y to gravity.
        // A real controller would accelerate instead of teleporting the
        // velocity like this, which is exactly the part worth replacing.
        Vector3 velocity = body.linearVelocity;
        velocity.x = wish.x * moveSpeed;
        velocity.z = wish.z * moveSpeed;

        body.linearVelocity = velocity;
    }

    private void Jump()
    {
        // The speed needed to just barely reach jumpHeight: v = sqrt(2gh).
        float jumpSpeed = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);

        Vector3 velocity = body.linearVelocity;
        velocity.y = jumpSpeed;
        body.linearVelocity = velocity;
    }

    /// <summary>
    /// A small sphere just under the capsule's feet. Cheap, and good
    /// enough for flat test geometry.
    /// </summary>
    private bool CheckGrounded()
    {
        float radius = capsule.radius * 0.9f;

        Vector3 feet = transform.TransformPoint(capsule.center)
                     - transform.up * (capsule.height * 0.5f - capsule.radius);

        return Physics.CheckSphere(
            feet - transform.up * groundCheckDistance,
            radius,
            groundLayers,
            QueryTriggerInteraction.Ignore);
    }

    public bool IsGrounded => isGrounded;
}
