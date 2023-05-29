using UnityEngine;

public class JoystickTranslateLocomotion : MonoBehaviour
{
    // Joystick, teleporter, grab world, hike around,
    // tongue (?), head controlled (gaze)

    // Two objects, ourself and the rube goldberg machine.

    // Joystick or grab seem fairly easy from a technical PoV

    // Nick votes Joystick

    // Parsanally (<--ART), favour teleport, because it's not continuous motion!

    // Benball: don't do it badly. Thanks Ben.

    public Transform head;

    public float moveSpeed = 1;

    private VRInputController input;

    private void Awake()
    {
        input = GetComponent<VRInputController>();
    }

    private void Update()
    {
        Vector2 moveInput = input.LeftJoystick;

        // Convert our moveDirection from *local* space to *world* space.
        Vector3 forward = head.forward;
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = head.right;
        right.y = 0;
        right = right.normalized;

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
