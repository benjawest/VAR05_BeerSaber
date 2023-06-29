using UnityEngine;
using UnityEngine.UI;

public class ScrollWheelInteraction : MonoBehaviour
{
    public Image[] images;
    public float rotationThreshold = 30f;
    public Transform rotationObject; // Reference to the object with the X-axis rotation

    public int selectedIndex = 0;
    public float accumulatedRotation = 0f;
    public float previousXRotation;

    public Color selectedColor = new (0.5f, 0.2f, 0.8f);
    public Color deselectedColor = new (0.5f, 0.2f, 0.8f);

    public UIEvents uiEvents;


    void Start()
    {
        previousXRotation = rotationObject.rotation.eulerAngles.x;
        SelectImage(selectedIndex, selectedColor, deselectedColor);
    }

    void Update()
    {
        // Get the current X-axis rotation
        float currentXRotation = rotationObject.rotation.eulerAngles.x;

        // Calculate the rotation delta
        float rotationDelta = currentXRotation - previousXRotation;

        // Adjust the rotation delta to account for wrapping around
        if (rotationDelta > 180f)
        {
            rotationDelta -= 360f;
        }
        else if (rotationDelta < -180f)
        {
            rotationDelta += 360f;
        }

        // Accumulate the rotation
        accumulatedRotation += rotationDelta;

        // Check if the accumulated rotation exceeds a threshold
        if (Mathf.Abs(accumulatedRotation) >= rotationThreshold)
        {
            // Calculate the number of steps to move
            int steps = Mathf.FloorToInt(accumulatedRotation / rotationThreshold);

            // Update the selected image based on the steps
            if (steps > 0)
            {
                for (int i = 0; i < steps; i++)
                {
                    SelectNextImage();
                }
            }
            else if (steps < 0)
            {
                for (int i = 0; i > steps; i--)
                {
                    SelectPreviousImage();
                }
            }

            // Update the accumulated rotation
            accumulatedRotation -= steps * rotationThreshold;
        }

        // Store the current X-axis rotation as the previous rotation for the next frame
        previousXRotation = currentXRotation;

        uiEvents.SetSelectedIndex(selectedIndex);
    }

    void SelectImage(int index, Color selectedColor, Color deselectedColor)
    {
        // Reset all images to unselected color
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = deselectedColor;
        }

        // Set the selected image to the custom color
        images[index].color = selectedColor;
        selectedIndex = index;
    }

    void SelectNextImage()
    {
        int nextIndex = (selectedIndex + 1) % images.Length;
        SelectImage(nextIndex, selectedColor, deselectedColor);
    }

    void SelectPreviousImage()
    {
        int previousIndex = (selectedIndex - 1 + images.Length) % images.Length;
        SelectImage(previousIndex, selectedColor, deselectedColor);
    }
}


