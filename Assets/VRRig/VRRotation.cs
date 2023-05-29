using UnityEngine;
using UnityEngine.InputSystem;

public class VRRotation : MonoBehaviour
{
    [SerializeField] Transform head;  // The head of the player, used for rotation.

    [SerializeField] InputActionReference rightJoystick;  // Reference to the right joystick input action.

    [Header("Smooth Turn")]
    [SerializeField] float rotationSpeed = 90.0f;  // Speed of the smooth turn.

    [Header("Snap Turn")]
    [SerializeField] bool isSnapTurn = false;  // Determines if snap turning is enabled.
    [SerializeField] float snapTurnRotation = 45.0f;  // Rotation amount for snap turning.
    private bool hasJoystickBeenReleased = true;  // Flag to prevent continuous snap turning.

    private VRInputController input;

    private void OnEnable()
    {
        rightJoystick.action.Enable();  // Enable the right joystick input action.
    }

    private void OnDisable()
    {
        rightJoystick.action.Disable();  // Disable the right joystick input action.
    }

    private void Update()
    {
        Vector2 joystickValue = rightJoystick.action.ReadValue<Vector2>();  // Read the joystick input value.
        float direction = joystickValue.x;  // Get the horizontal value of the joystick input.

        Vector3 initialHeadPosition = head.position;  // Get the initial position of the player's head.

        if (isSnapTurn)
        {
            if (direction > 0.5f && hasJoystickBeenReleased)
            {
                // Snap turn to the right.
                transform.Rotate(0, snapTurnRotation, 0);
                hasJoystickBeenReleased = false;
            }
            else if (direction < -0.5f && hasJoystickBeenReleased)
            {
                // Snap turn to the left.
                transform.Rotate(0, -snapTurnRotation, 0);
                hasJoystickBeenReleased = false;
            }
            else if (direction < 0.5f && direction > -0.5f)
            {
                // Joystick is centered, allow snap turning again.
                hasJoystickBeenReleased = true;
            }
        }
        else
        {
            // Smooth turn the player.
            transform.Rotate(0, direction * rotationSpeed * Time.deltaTime, 0);
        }

        Vector3 headTranslationOffset = initialHeadPosition - head.position;  // Calculate the head's position change.

        transform.position += headTranslationOffset;  // Update the position of the player based on the head's movement.
    }
}
