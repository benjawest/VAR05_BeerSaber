using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VRInputController : MonoBehaviour
{
    // Publics are usually prefaced with a capital letter.
    public Vector2 LeftJoystick;
    public Vector2 RightJoystick;
    public float RightTrigger;

    private VRInputActions actions;

    // This is called ONLY in the editor when you modify any public
    // fields.
    private void OnValidate()
    {
        // Set the *length* of the joystick vector to never exceed 1.
        LeftJoystick = Vector3.ClampMagnitude(LeftJoystick, 1);
        RightJoystick = Vector3.ClampMagnitude(RightJoystick, 1);
        RightTrigger = Mathf.Clamp01(RightTrigger);
    }

    private void Awake()
    {
        actions = new VRInputActions();

        // If you don't call this, you won't be able to read input.
        // (Why is this not enabled by default? Beats me, ask Unity.)
        actions.Enable();
    }

    private void Update()
    {
        XRHMD hmd = InputSystem.GetDevice<XRHMD>();

        if (hmd != null)
        {
            LeftJoystick = actions.Default.LeftJoystick.ReadValue<Vector2>();
            RightJoystick = actions.Default.RightJoystick.ReadValue<Vector2>();
            RightTrigger = actions.Default.RightTrigger.ReadValue<float>();
        }
    }
}
