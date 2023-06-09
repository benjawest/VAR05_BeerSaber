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

    //Input stuff
    [SerializeField] private bool activateTrigger = false;
    [SerializeField] private float debugLineDuration = 1.0f;
    [SerializeField] private float aimLineLength = 10f;
    [SerializeField] private Vector3 handRotationOffset;
    private VRInputActions inputActions;
    Transform handTransform;
    //Bullet stuff
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootForce = 10f;

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
        // Adjust + Set shoot direction
        Quaternion shootDirectionRotation = handTransform.rotation * Quaternion.Euler(handRotationOffset);

        // Instantiate the projectile prefab
        GameObject projectile = Instantiate(projectilePrefab, handTransform.position, shootDirectionRotation);

        // Get the Rigidbody component of the projectile
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

        // Apply a force or velocity to propel the projectile forward
        projectileRigidbody.AddForce(shootDirectionRotation * Vector3.forward * shootForce, ForceMode.Impulse);

        // Debug simulation for testing
        Debug.DrawRay(handTransform.position, shootDirectionRotation * Vector3.forward, Color.red, debugLineDuration);
    }

    private void UpdateAimLine()
    {

        // Calculate aim direction
        Quaternion aimDirection = handTransform.rotation * Quaternion.Euler(handRotationOffset);

        // Calculate end position of the aim line
        Vector3 endPosition = handTransform.position + aimDirection * Vector3.forward * aimLineLength;

        // Render the aim line as a debug draw line
        Debug.DrawRay(handTransform.position, aimDirection * Vector3.forward * aimLineLength, Color.yellow);
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