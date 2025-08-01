using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private PlayerLogic playerLogic;
    [SerializeField] private ShieldDetectable shield;
    [SerializeField] private TMP_Text playerHealth;
    [SerializeField] private TMP_Text shieldCooldown;

    private void Awake()
    {
        playerLogic = FindFirstObjectByType<PlayerLogic>();
        shield = FindFirstObjectByType<ShieldDetectable>();

        playerHealth = GameObject.Find("PlayerHealth").GetComponent<TMP_Text>();
        shieldCooldown = GameObject.Find("ShieldCooldown").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        SpawnEnemy();
        ReloadScene();
        playerHealth.text = "Health: " + playerLogic.GetHealth().ToString("F0") + "/100";

        // Update shield text based on its status
        if (shield.IsShieldActive())
        {
            // If active, show the countdown
            shieldCooldown.text = "Shield Cooldown: " + shield.GetRemainingCooldown().ToString("F1") + "s";
        }
        else
        {
            // If not active, show it's ready
            shieldCooldown.text = "Shield: Ready";
        }
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
