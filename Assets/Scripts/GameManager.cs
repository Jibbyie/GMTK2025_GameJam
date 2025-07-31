using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private PlayerLogic playerLogic;
    [SerializeField] TMP_Text playerHealth;

    private void Awake()
    {
        playerLogic = FindFirstObjectByType<PlayerLogic>();
    }

    private void Update()
    {
        SpawnEnemy();
        ReloadScene();
        playerHealth.text = "Health: " + playerLogic.GetHealth().ToString("F0") + "/100";
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

    private void ReloadScene()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
