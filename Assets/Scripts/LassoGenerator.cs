using UnityEngine;

public class LassoGenerator : MonoBehaviour
{
    public GameObject lassoPrefab;

    Lasso activeLasso;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLasso = Instantiate(lassoPrefab);

            activeLasso = newLasso.GetComponent<Lasso>();
        }

        if(Input.GetMouseButtonUp(0))
        {
            activeLasso = null;
        }

        if (activeLasso != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; 
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            activeLasso.UpdateLine(worldPos);
        }
    }
}
