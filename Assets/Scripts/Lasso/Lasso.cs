using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lasso : MonoBehaviour
{
    public LineRenderer lineRenderer;

    List<Vector2> points;
    [SerializeField] private int maxPointCount;

    void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return;
        }

        // Only add a new point if the mouse has moved a significant distance
        if (Vector2.Distance(points.Last(), position) > .1f)
        {
            if(points.Count < maxPointCount)
            {
                SetPoint(position);
            }
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
}
