using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lasso : MonoBehaviour
{
    public LineRenderer lineRenderer;

    List<Vector2> points;
    [SerializeField] private int maxPointCount;

    // Time-based point collection
    [SerializeField] private float pointCollectionInterval = 0.016f; // ~60fps equivalent
    private float lastPointTime;
    private float totalDrawTime;

    void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lastPointTime = 0f;
        totalDrawTime = 0f;
    }

    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            lastPointTime = Time.time;
            return;
        }

        // Track total drawing time
        totalDrawTime = Time.time - lastPointTime + totalDrawTime;

        // Time-based collection with distance fallback
        bool shouldAddPoint = false;

        // Primary: Time-based collection
        if (Time.time - lastPointTime >= pointCollectionInterval)
        {
            shouldAddPoint = true;
        }

        // Secondary: Distance-based collection (reduced threshold)
        if (Vector2.Distance(points.Last(), position) > 0.02f && !shouldAddPoint)
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
        // At least 20 points, but scale with draw time (30 points per second)
        return Mathf.Max(20, Mathf.RoundToInt(totalDrawTime * 30f));
    }

    public float GetTotalDrawTime()
    {
        return totalDrawTime;
    }
}