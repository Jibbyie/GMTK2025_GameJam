using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LassoGenerator : MonoBehaviour
{
    [Header("Lasso Values")]
    [SerializeField] private float closedLoopValue = 2f; // Slightly more forgiving
    [SerializeField] private int fallbackMinimumPoints = 20; // Lower fallback minimum
    [SerializeField] private float lassoLifeTime = 0.75f;
    [SerializeField] private float drawSensitivity = 0.01f; // 1% of camera height
    private float minDrawDistance;
    private bool canLasso;
    [SerializeField] private Animator playerAnimator;

    public GameObject lassoPrefab;

    Lasso activeLasso;
    bool loopClosed;

    bool ignoreInput;

    // Defensive programming
    private void Awake()
    {
        // Destroy any persisting lassos on scene reload 
        Lasso[] persistingLassos = FindObjectsByType<Lasso>(FindObjectsSortMode.None);
        foreach (Lasso existingLasso in persistingLassos)
        {
            Destroy(existingLasso.gameObject);
        }
        // Set activeLasso to null
        activeLasso = null;

        // Check for a phantom press on game start - input validation
        if (Input.GetMouseButton(0))
        {
            ignoreInput = true;
        }

        // Calculate world space distances on start
        // Lasso drawing logic scales with player's screen size
        minDrawDistance = Camera.main.orthographicSize * drawSensitivity;
        closedLoopValue = Camera.main.orthographicSize * 0.2f; // 20% of camera height
    }

    private void Update()
    {
        // If we let go of the phantom press
        if (Input.GetMouseButtonUp(0) && ignoreInput)
        {
            ignoreInput = false;
        }

        // Dont run update until phantom press is resolved
        if (ignoreInput) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Destroy any pre existing lassos (double check)
            if (activeLasso != null)
            {
                Destroy(activeLasso.gameObject);
            }

            GameObject newLassoObject = Instantiate(lassoPrefab);
            activeLasso = newLassoObject.GetComponent<Lasso>();

            canLasso = true;
            playerAnimator.SetBool("isLasso", canLasso);
        }

        if (Input.GetMouseButtonUp(0))
        {
            canLasso = false;
            playerAnimator.SetBool("isLasso", canLasso);

            DetectLoop();

            activeLasso = null;
        }

        if (activeLasso != null)
        {
            Vector3 mousePositionScreen = Input.mousePosition;
            mousePositionScreen.z = 10f;
            Vector2 mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePositionScreen);

            // Only update line if mouse has moved significantly or count is 0
            if (activeLasso.GetPoints().Count == 0 ||
                Vector2.Distance(activeLasso.GetPoints().Last(), mousePositionWorld) > minDrawDistance)
            {
                activeLasso.UpdateLine(mousePositionWorld);
            }
        }
    }

    void DetectLoop()
    {
        // Get adaptive minimum points based on draw time
        int adaptiveMinimum = activeLasso.GetAdaptiveMinimumPoints();

        var intersectionLoopPoints = DetectSelfIntersection(activeLasso.GetPoints(), adaptiveMinimum);
        if (intersectionLoopPoints != null)
        {
            ObjectDetected(intersectionLoopPoints);
        }
        // If the first and last points are close to each other
        // And there are enough points in the line (use adaptive or fallback minimum)
        else if (Vector2.Distance(activeLasso.GetPoints().First(), activeLasso.GetPoints().Last()) < closedLoopValue
            && activeLasso.GetPoints().Count > Mathf.Min(adaptiveMinimum, fallbackMinimumPoints))
        {
            loopClosed = true;
            if (loopClosed) // if we detect a closed loop
            {
                ObjectDetected(activeLasso.GetPoints());
            }
        }
        else
        {
            Debug.Log($"Not a closed loop - Points: {activeLasso.GetPoints().Count}, Required: {Mathf.Min(adaptiveMinimum, fallbackMinimumPoints)}, Distance: {Vector2.Distance(activeLasso.GetPoints().First(), activeLasso.GetPoints().Last())}");
            Destroy(activeLasso.gameObject);
        }
    }

    void ObjectDetected(List<Vector2> polygonPoints)
    {
        Debug.Log("Closed loop detected!");

        // Find every object with a detectable tag
        DetectableObject[] detectableObjects = FindObjectsByType<DetectableObject>(FindObjectsSortMode.None);
        foreach (DetectableObject detectableObject in detectableObjects)
        {
            // Pass in the detectable objects position
            Vector2 objectPosition = detectableObject.transform.position;

            // Ray cast check - is this object inside the polygon?
            if (IsPointInPolygon(objectPosition, polygonPoints))
            {
                // Call the virtual function on this detected object
                // i.e., boost the player's speed
                detectableObject.OnDetected();
            }
        }
        Destroy(activeLasso.gameObject, lassoLifeTime);
        loopClosed = false;
    }

    bool DoLinesIntersect(Vector2 firstLineStart, Vector2 firstLineEnd, Vector2 secondLineStart, Vector2 secondLineEnd)
    {
        float denominator = (firstLineStart.x - firstLineEnd.x) * (secondLineStart.y - secondLineEnd.y) -
                           (firstLineStart.y - firstLineEnd.y) * (secondLineStart.x - secondLineEnd.x);

        if (Mathf.Abs(denominator) < 0.0001f) return false; // Lines are parallel

        float firstLineParameter = ((firstLineStart.x - secondLineStart.x) * (secondLineStart.y - secondLineEnd.y) -
                                   (firstLineStart.y - secondLineStart.y) * (secondLineStart.x - secondLineEnd.x)) / denominator;
        float secondLineParameter = -((firstLineStart.x - firstLineEnd.x) * (firstLineStart.y - secondLineStart.y) -
                                     (firstLineStart.y - firstLineEnd.y) * (firstLineStart.x - secondLineStart.x)) / denominator;

        // Both parameters must be between 0 and 1 for intersection to occur on both line segments
        return firstLineParameter >= 0 && firstLineParameter <= 1 && secondLineParameter >= 0 && secondLineParameter <= 1;
    }

    // https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
    List<Vector2> DetectSelfIntersection(List<Vector2> lassoPoints, int minimumPointsRequired)
    {
        if (lassoPoints != null)
        {
            // Check each line segment against every non-adjacent segment for intersections
            for (int firstSegmentIndex = 0; firstSegmentIndex < lassoPoints.Count - 1; firstSegmentIndex++)
            {
                for (int secondSegmentIndex = firstSegmentIndex + 2; secondSegmentIndex < lassoPoints.Count - 1; secondSegmentIndex++) // Skip adjacent segments
                {
                    // Skip checking first segment against last segment (they share endpoints)
                    if (secondSegmentIndex == lassoPoints.Count - 2 && firstSegmentIndex == 0) continue;

                    if (DoLinesIntersect(lassoPoints[firstSegmentIndex], lassoPoints[firstSegmentIndex + 1],
                                        lassoPoints[secondSegmentIndex], lassoPoints[secondSegmentIndex + 1]))
                    {
                        // Found intersection between segments - extract the loop portion
                        List<Vector2> extractedLoopPoints = lassoPoints.GetRange(firstSegmentIndex + 1,
                                                                                (secondSegmentIndex + 1) - (firstSegmentIndex + 1));
                        if (extractedLoopPoints.Count > minimumPointsRequired)
                        {
                            return extractedLoopPoints;
                        }
                    }
                }
            }
        }
        return null;
    }

    // Point-in-polygon algorithm using ray casting
    bool IsPointInPolygon(Vector2 testPoint, List<Vector2> polygonVertices)
    {
        int rayIntersectionCount = 0;

        // Cast a horizontal ray from test point and count intersections with polygon edges
        for (int vertexIndex = 0; vertexIndex < polygonVertices.Count; vertexIndex++)
        {
            Vector2 currentVertex = polygonVertices[vertexIndex];
            Vector2 nextVertex = polygonVertices[(vertexIndex + 1) % polygonVertices.Count]; // Wrap around to first point

            // Check if horizontal ray from test point intersects with this edge
            if (DoesRayIntersectEdge(testPoint, currentVertex, nextVertex))
            {
                rayIntersectionCount++;
            }
        }

        // Odd number of intersections = point is inside polygon
        return rayIntersectionCount % 2 == 1;
    }

    bool DoesRayIntersectEdge(Vector2 rayStartPoint, Vector2 edgeStartPoint, Vector2 edgeEndPoint)
    {
        // Ray goes horizontally to the right from rayStartPoint
        // Check if the edge crosses this horizontal line

        // Edge must cross the horizontal line at rayStartPoint.y
        if ((edgeStartPoint.y > rayStartPoint.y) == (edgeEndPoint.y > rayStartPoint.y))
            return false; // Edge doesn't cross the horizontal line

        // Calculate where the edge intersects the horizontal line
        float intersectionX = edgeStartPoint.x + (rayStartPoint.y - edgeStartPoint.y) /
                             (edgeEndPoint.y - edgeStartPoint.y) * (edgeEndPoint.x - edgeStartPoint.x);

        // Intersection must be to the right of the ray start point
        return intersectionX > rayStartPoint.x;
    }
}