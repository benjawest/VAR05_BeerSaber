using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuRaycaster : MonoBehaviour
{
    public float raycastDistance = 10f;
    public LayerMask raycastLayer;
    public Color lineColor = Color.white;
    public float lineThickness = 0.02f;
    public Vector3 forwardVector = Vector3.forward;
    public InputActionReference triggerAction;
    public bool isTriggerPressed = false;
    public float triggerActionValue;
    public bool showLineRenderer = true;

    private LineRenderer lineRenderer;

    void Awake()
    {
        triggerAction.action.Enable();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = showLineRenderer; // Set the initial state of the line renderer
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material.color = lineColor;
    }

    void Update()
    {
        triggerActionValue = triggerAction.action.ReadValue<float>();
        isTriggerPressed = triggerActionValue > 0.5f;

        Vector3 localForwardVector = transform.TransformDirection(forwardVector);
        Ray ray = new Ray(transform.position, localForwardVector);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit, raycastDistance, raycastLayer);

        lineRenderer.enabled = showLineRenderer;

        lineRenderer.positionCount = 2; // Set the position count to 2

        lineRenderer.SetPosition(0, transform.position);

        if (hitSomething)
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.gameObject.GetComponent<Button>())
            {
                EventSystem.current.SetSelectedGameObject(hit.collider.gameObject);

                if (isTriggerPressed)
                {
                    hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + localForwardVector * raycastDistance);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }


    public void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;
    }
}
