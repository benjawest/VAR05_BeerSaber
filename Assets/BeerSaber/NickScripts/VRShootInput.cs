using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VRShootInput : MonoBehaviour
{
    // Use a score manager class to keep track of all score and object hit data
    // enum HitResult {good, bad, Perf}
    // public hitresult registerhit(){
    // add the hit to score;
    //}

    [SerializeField] private bool activateTrigger = false;

    [SerializeField] private float debugLineDuration = 1.0f;

    [SerializeField] private float aimLineLength = 10f;

    private VRInputActions inputActions;
    Transform handTransform;

    private void Awake()
    {
        inputActions = new VRInputActions();
        inputActions.Enable();
        // Assigns the transform of the hand the script is connected to
        handTransform = transform;
    }

    private void Update()
    {
        if (activateTrigger || inputActions.Default.RightTrigger.WasPressedThisFrame())
        {
            Debug.Log("Trigger was pressed this frame");
            // Put shooting logic here
            Shoot();
            activateTrigger = false;
            // Reset the trigger after shooting
        }

        // Update Line Renderer 

        UpdateAimLine();
    }

    private void Shoot()
    {
        // Implement shooting here

        // Debug simulation for testing
        Vector3 shootDirection = handTransform.forward;
        Debug.DrawRay(handTransform.position, shootDirection * 10f, Color.red, debugLineDuration);
    }

    private void UpdateAimLine()
    {

            // Calculate aim direction
            Vector3 aimDirection = handTransform.forward;

            // Calculate end position of the aim line
            Vector3 endPosition = handTransform.position + aimDirection * aimLineLength;

            // Render the aim line as a debug draw line
            Debug.DrawRay(handTransform.position, aimDirection * aimLineLength, Color.yellow);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(VRShootInput))]
    public class ActivateTriggerButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            VRShootInput vrShootInput = (VRShootInput)target;

            if (GUILayout.Button("Activate Trigger"))
            {
                vrShootInput.activateTrigger = true;
            }
        }
    }
#endif
}