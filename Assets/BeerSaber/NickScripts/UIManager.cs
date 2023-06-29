using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public UnityEvent onSelectButtonHit;
    public UnityEvent onBackButtonHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // Customize this with the appropriate tag for your bullet
        {
            GameObject collidedObject = collision.gameObject;

            if (collidedObject.CompareTag("SelectButton")) // Customize this with the appropriate tag for your select button
            {
                onSelectButtonHit?.Invoke();
            }
            else if (collidedObject.CompareTag("BackButton")) // Customize this with the appropriate tag for your back button
            {
                onBackButtonHit?.Invoke();
            }
        }
    }
}