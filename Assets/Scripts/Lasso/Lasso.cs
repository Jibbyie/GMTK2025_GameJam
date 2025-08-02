using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lasso : MonoBehaviour
{
    public LineRenderer lineRenderer;

    List<Vector2> points;
    [SerializeField] private int maxPointCount;

    // Time-based point collection
    [SerializeField] private float pointCollectionInterval = 0.008f; // ~60fps equivalent
    private float lastPointTime;
    private float totalDrawTime;
    private float minDrawDistance;
    [SerializeField] private float drawSensitivity = 0.005f;

    void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        points = new List<Vector2>();
        maxPointCount = 120;
        lastPointTime = 0f;
        totalDrawTime = 0f;

        // Calculate world space distances on start
        // Lasso drawing logic scales with player's screen size
        minDrawDistance = Camera.main.orthographicSize * drawSensitivity; // 20% of camera height
    }

    public void UpdateLine(Vector2 position)
    {
        if (points.Count == 0) // Change from null check to count check
        {
            SetPoint(position);
            lastPointTime = Time.time;
            return;
        }

        // Track total drawing time
        totalDrawTime = Time.time - lastPointTime + totalDrawTime;

        // Time-based collection with distance fallback
        bool shouldAddPoint = false;

        // Primary: Time-based collection with movement requirement
        if (Time.time - lastPointTime >= pointCollectionInterval &&
            Vector2.Distance(points.Last(), position) > minDrawDistance)  // movement threshold 
        {
            shouldAddPoint = true;
        }

        // Secondary: Distance-based collection for fine details
        if (Vector2.Distance(points.Last(), position) > minDrawDistance && !shouldAddPoint)
        {
            shouldAddPoint = true;
        }

        if (shouldAddPoint && points.Count < maxPointCount)
        {
            SetPoint(position);
            lastPointTime = Time.time;
        }
    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    public List<Vector2> GetPoints()
    {
        return points;
    }

    // Get adaptive minimum points based on draw time
    public int GetAdaptiveMinimumPoints()
    {
        // At least 10 points, but scale with draw time (20 points per second)
        return Mathf.Max(10, Mathf.RoundToInt(totalDrawTime * 20f));
    }

    public float GetTotalDrawTime()
    {
        return totalDrawTime;
    }
}