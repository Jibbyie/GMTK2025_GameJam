using UnityEngine;

public class SyncPositionToUI : MonoBehaviour
{
    [SerializeField] private RectTransform targetUIElement;

    void Update()
    {
        if (targetUIElement != null)
        {
            Vector3 screenPosition = targetUIElement.position;

            screenPosition.z = 10f;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            transform.position = worldPosition;
        }
    }
}