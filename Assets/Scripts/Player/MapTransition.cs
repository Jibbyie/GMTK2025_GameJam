using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{

    [SerializeField] private PolygonCollider2D mapBoundary;
    private CinemachineConfiner2D confiner;
    [SerializeField] private Direction direction;
    [SerializeField] private float posOffset = 2f;

    [Header("FMOD Settings")]
    [Tooltip("The level value to send to FMOD (0 for Level 1, 1 for Level 2, etc.)")]
    [SerializeField] private int levelIndex;

    enum Direction { Up, Down, Left, Right }

    private void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            UpdatePlayerPosition(collision.gameObject);
            // Call the function in the GameMusicManager to change the music parameter.
            GameMusicManager.Instance.SetLevelParameter(levelIndex);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += posOffset;
                break;
            case Direction.Down:
                newPos.y -= posOffset;
                break;
            case Direction.Left:
                newPos.x -= posOffset;
                break;
            case Direction.Right:
                newPos.x += posOffset;
                break;
        }
    }

}
