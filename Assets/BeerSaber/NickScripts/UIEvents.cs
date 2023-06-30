using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public int selectedIndex;
    public ScrollWheelInteraction scrollWheelInteraction;
    public GameObject[] menuGroups;

    public void OnSelectButtonHit()
    {
        Debug.Log("Select button hit!");
        // Perform action based on the selected index
        switch (selectedIndex)
        {
            case 0:
                // Handle action for index 0
                Debug.Log("Selected index 0");
                scrollWheelInteraction.canvasGroupIndex = 1;
                EnableMenuGroup(0); // Enable the menu group at index 0
                break;
            case 1:
                // Handle action for index 1
                Debug.Log("Selected index 1");
                scrollWheelInteraction.canvasGroupIndex = 2;
                EnableMenuGroup(1); // Enable the menu group at index 1
                break;
            case 2:
                // Handle action for index 2
                Debug.Log("Selected index 2");
                scrollWheelInteraction.canvasGroupIndex = 3;
                EnableMenuGroup(2); // Enable the menu group at index 2
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
        DisableAllMenuGroups();
        scrollWheelInteraction.canvasGroupIndex = 0;
        EnableMenuGroup(3);
    }
    private void EnableMenuGroup(int index)
    {
        // Disable all menu groups
        DisableAllMenuGroups();

        // Enable the specified menu group
        if (index >= 0 && index < menuGroups.Length)
        {
            menuGroups[index].SetActive(true);
        }
        else
        {
            Debug.Log("Invalid menu group index: " + index);
        }
    }

    private void DisableAllMenuGroups()
    {
        // Disable all menu groups
        foreach (GameObject menuGroup in menuGroups)
        {
            menuGroup.SetActive(false);
        }
    }

    public void SetSelectedIndex(int index)
    {
        selectedIndex = index;
    }
}