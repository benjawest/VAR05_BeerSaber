using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public int selectedIndex;

    public void OnSelectButtonHit()
    {
        Debug.Log("Select button hit!");
        // Perform action based on the selected index
        switch (selectedIndex)
        {
            case 0:
                // Handle action for index 0
                Debug.Log("Selected index 0");
                break;
            case 1:
                // Handle action for index 1
                Debug.Log("Selected index 1");
                break;
            case 2:
                // Handle action for index 2
                Debug.Log("Selected index 2");
                break;
            // Add more cases for additional indices as needed
            default:
                // Handle default case
                Debug.Log("Invalid selected index");
                break;
        }
    }

    public void OnBackButtonHit()
    {
        // Handle back button hit event
        Debug.Log("Back button hit!");
    }

    public void SetSelectedIndex(int index)
    {
        selectedIndex = index;
    }
}