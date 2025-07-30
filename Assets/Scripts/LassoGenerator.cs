using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LassoGenerator : MonoBehaviour
{
    public GameObject lassoPrefab;

    Lasso activeLasso;
    bool loopClosed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLasso = Instantiate(lassoPrefab);
            activeLasso = newLasso.GetComponent<Lasso>();
        }

        if (Input.GetMouseButtonUp(0))
        {

            DetectLoop();
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

    void DetectLoop()
    {
        if (Vector2.Distance(activeLasso.GetPoints().First(), activeLasso.GetPoints().Last()) < .25f
            && activeLasso.GetPoints().Count > 15)
        {
            loopClosed = true;
            if (loopClosed)
            {
                Debug.Log("Closed loop detected!");

                DetectableObject[] objList = FindObjectsByType<DetectableObject>(FindObjectsSortMode.None);
                foreach (DetectableObject obj in objList)
                {
                    Vector2 position = obj.transform.position;

                    // Ray cast check
                    if (IsPointInPolygon(position, activeLasso.GetPoints()))
                    {
                        obj.OnDetected();
                    }
                }
                Destroy(activeLasso.gameObject, 1f);
                loopClosed = false;
            }
        }
        else
        {
            Debug.Log("Not a closed loop - destroying immediately");
            Destroy(activeLasso.gameObject);   
        }
    }

    // Point-in-polygon algorithm using ray casting
    bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        int intersectionCount = 0;

        // Loop through all edges of the polygon
        for (int i = 0; i < polygon.Count; i++)
        {
            Vector2 currentPoint = polygon[i];
            Vector2 nextPoint = polygon[(i + 1) % polygon.Count]; // Wrap around to first point

            // Check if horizontal ray from point intersects with this edge
            if (DoesRayIntersectEdge(point, currentPoint, nextPoint))
            {
                intersectionCount++;
            }
        }

        // Odd number of intersections = inside
        return intersectionCount % 2 == 1;
    }

    bool DoesRayIntersectEdge(Vector2 rayStart, Vector2 edgeStart, Vector2 edgeEnd)
    {
        // Ray goes horizontally to the right from rayStart
        // Check if the edge crosses this horizontal line

        // Edge must cross the horizontal line at rayStart.y
        if ((edgeStart.y > rayStart.y) == (edgeEnd.y > rayStart.y))
            return false; // Edge doesn't cross the horizontal line

        // Calculate where the edge intersects the horizontal line
        float intersectionX = edgeStart.x + (rayStart.y - edgeStart.y) / (edgeEnd.y - edgeStart.y) * (edgeEnd.x - edgeStart.x);

        // Intersection must be to the right of the ray start point
        return intersectionX > rayStart.x;
    }
}