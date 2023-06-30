using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public UnityEvent onSelectButtonHit;
    public UnityEvent onBackButtonHit;

    [SerializeField] private Collider selectButtonCollider;
    [SerializeField] private Collider backButtonCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit something");

            if (selectButtonCollider.bounds.Intersects(other.bounds))
            {
                Debug.Log("Bullet hit select");
                onSelectButtonHit?.Invoke();
            }
            else if (backButtonCollider.bounds.Intersects(other.bounds))
            {
                Debug.Log("Bullet hit back");
                onBackButtonHit?.Invoke();
            }
        }
    }
}