using UnityEngine;
using UnityEngine.UI;

public class ScrollWheelInteraction : MonoBehaviour
{
    public Image[] MainMenuimages;
    public Image[] SongMenuimages;
    public Image[] GunMenuimages;
    public Image[] Extrasimages;

    private Image[] canvasImages;

    public float rotationThreshold = 30f;
    public Transform rotationObject; // Reference to the object with the X-axis rotation


    public int canvasGroupIndex = 0;
    public int selectedIndex = 0;

    public float accumulatedRotation = 0f;
    public float previousXRotation;

    public Color selectedColor = new (0.5f, 0.2f, 0.8f);
    public Color deselectedColor = new (0.5f, 0.2f, 0.8f);

    public UIEvents uiEvents;


    void Start()
    {
        previousXRotation = rotationObject.rotation.eulerAngles.x;
        SelectImage(canvasGroupIndex, selectedIndex, selectedColor, deselectedColor);
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

    void SelectImage(int canvasIndex, int index, Color selectedColor, Color deselectedColor)
    {

        if (canvasIndex == 0)
        {
            canvasImages = MainMenuimages;
            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Set the selected image to the custom color
            canvasImages[index].color = selectedColor;
            selectedIndex = index;
        }
        if(canvasIndex == 1)
        {
            canvasImages = SongMenuimages;
            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Set the selected image to the custom color
            canvasImages[index].color = selectedColor;
            selectedIndex = index;
        }
        if (canvasIndex == 2)
        {
            canvasImages = GunMenuimages;
            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Set the selected image to the custom color
            canvasImages[index].color = selectedColor;
            selectedIndex = index;
        }
        if (canvasIndex == 3)
        {
            canvasImages = Extrasimages;
            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Reset all images to unselected color
            for (int i = 0; i < canvasImages.Length; i++)
            {
                canvasImages[i].color = deselectedColor;
            }

            // Set the selected image to the custom color
            canvasImages[index].color = selectedColor;
            selectedIndex = index;
        }

    }

    void SelectNextImage()
    {
        int nextIndex = (selectedIndex + 1) % canvasImages.Length;
        SelectImage(canvasGroupIndex, nextIndex, selectedColor, deselectedColor);
    }

    void SelectPreviousImage()
    {
        int previousIndex = (selectedIndex - 1 + canvasImages.Length) % canvasImages.Length;
        SelectImage(canvasGroupIndex, previousIndex, selectedColor, deselectedColor);
    }
}


