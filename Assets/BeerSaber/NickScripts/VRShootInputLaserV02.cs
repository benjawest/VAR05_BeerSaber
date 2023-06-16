using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VRShootInputLaserV02 : MonoBehaviour
{
    // Use a score manager class to keep track of all score and object hit data
    // enum HitResult {good, bad, Perf}
    // public hitresult registerhit(){
    // add the hit to score;
    //}

    //Input stuff
    [SerializeField] private float debugLineDuration = 1.0f;
    [SerializeField] private float aimLineLength = 10f;

    private VRInputActions inputActions;
    [SerializeField] private BeatMapLevelManager beatMapLevelManager;

    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Vector3 rightHandRotationOffset;
    [SerializeField] private Vector3 leftHandRotationOffset;
    private bool rightTriggerActivated = false;
    private bool leftTriggerActivated = false;

    //Bullet stuff
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float forceMagnitude = 1;
    [SerializeField] private bool destroyOnHit = false;
    [SerializeField] private bool physicsHit = false;
    [SerializeField] private AudioSource rightGunAudioSource;
    [SerializeField] private AudioSource leftGunAudioSource;
    // AudioClip list of sound to cycle through for gun shots
    [SerializeField] private AudioClip[] shootSounds;


    private void Awake()
    {
        inputActions = new VRInputActions();
        inputActions.Enable();

        // Initialize the trigger states
        rightTriggerActivated = false;
        leftTriggerActivated = false;
    }

    private void Update()
    {
        if (rightTriggerActivated || inputActions.Default.RightTrigger.WasPressedThisFrame())
        {
            Debug.Log("Right Trigger was pressed this frame");
            // Put shooting logic here
            Transform handTransform = rightHandTransform;
            Vector3 handRotationOffset = rightHandRotationOffset;
            Shoot(handTransform, handRotationOffset);
            rightTriggerActivated = false;
            // Reset the trigger after shooting
        }

        if (leftTriggerActivated || inputActions.Default.LeftTrigger.WasPressedThisFrame())
        {
            Debug.Log("Left Trigger was pressed this frame");
            // Put shooting logic here
            Transform handTransform = leftHandTransform;
            Vector3 handRotationOffset = leftHandRotationOffset;
            Shoot(handTransform, handRotationOffset);
            leftTriggerActivated = false;
            // Reset the trigger after shooting
        }

        // Update Line Renderer 

        UpdateAimLine(rightHandTransform, rightHandRotationOffset);
        UpdateAimLine(leftHandTransform, leftHandRotationOffset);
    }

    private void Shoot(Transform handTransform, Vector3 handRotationOffset)
    {
        // Implement shooting here
        // Adjust + Set shoot direction
        Quaternion shootDirectionRotation = handTransform.rotation * Quaternion.Euler(handRotationOffset);

        // Perform a raycast to detect hits
        RaycastHit hit;
        if (Physics.Raycast(handTransform.position, shootDirectionRotation * Vector3.forward, out hit, Mathf.Infinity))
        {
            // Handle hit logic
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            if (destroyOnHit == true && physicsHit == false)
            {

                // Destroy the hit object
                Debug.Log("Destroyed: " + hit.collider.gameObject.name);
                beatMapLevelManager.NoteHit(hit.collider.gameObject);

            }
            else if (destroyOnHit == false && physicsHit == true)
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Debug.Log("PhysicsHit: " + hit.collider.gameObject.name);
                    Vector3 forceDirection = hit.point - handTransform.position;
                    rb.AddForce(forceDirection.normalized * forceMagnitude, ForceMode.Impulse);
                }
            }
        }

        if (handTransform == rightHandTransform && rightGunAudioSource != null)
        {
            // pick a random shoot sound from the list
            AudioClip shootSound = shootSounds[Random.Range(0, shootSounds.Length)];
            rightGunAudioSource.PlayOneShot(shootSound);
            
        }
        else if (handTransform == leftHandTransform && leftGunAudioSource != null)
        {
            AudioClip shootSound = shootSounds[Random.Range(0, shootSounds.Length)];
            leftGunAudioSource.PlayOneShot(shootSound);
        }

        // Trigger haptic feedback
        if (handTransform == rightHandTransform)
        {
            // For right hand
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (rightHandDevice != null)
            {
                HapticCapabilities hapticCapabilities;
                if (rightHandDevice.TryGetHapticCapabilities(out hapticCapabilities) && hapticCapabilities.supportsImpulse)
                {
                    rightHandDevice.SendHapticImpulse(0, 0.5f, 0.1f);
                }
            }
        }
        else if (handTransform == leftHandTransform)
        {
            // For left hand
            InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (leftHandDevice != null)
            {
                HapticCapabilities hapticCapabilities;
                if (leftHandDevice.TryGetHapticCapabilities(out hapticCapabilities) && hapticCapabilities.supportsImpulse)
                {
                    leftHandDevice.SendHapticImpulse(0, 0.5f, 0.1f);
                }
            }
        }

        // Instantiate the projectile prefab
        Instantiate(projectilePrefab, handTransform.position, shootDirectionRotation);

        // Debug simulation for testing
        Debug.DrawRay(handTransform.position, shootDirectionRotation * Vector3.forward, Color.red, debugLineDuration);
    }

    private void UpdateAimLine(Transform handTransform, Vector3 handRotationOffset)
    {
        // Calculate aim direction
        Quaternion aimDirection = handTransform.rotation * Quaternion.Euler(handRotationOffset);

        // Calculate end position of the aim line
        Vector3 endPosition = handTransform.position + aimDirection * Vector3.forward * aimLineLength;

        // Render the aim line as a debug draw line
        Debug.DrawRay(handTransform.position, aimDirection * Vector3.forward * aimLineLength, Color.yellow);
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(VRShootInputLaser))]
    public class ActivateTriggerButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            VRShootInputLaserV02 vrShootInput = (VRShootInputLaserV02)target;

            if (GUILayout.Button("Activate Right Trigger"))
            {
                vrShootInput.rightTriggerActivated = true;
            }

            if (GUILayout.Button("Activate Left Trigger"))
            {
                vrShootInput.leftTriggerActivated = true;
            }
        }
    }
}
#endif