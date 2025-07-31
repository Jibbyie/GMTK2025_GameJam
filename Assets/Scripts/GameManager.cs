using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            Instantiate(enemyPrefab, worldPos, Quaternion.identity);
        }
    }
}
