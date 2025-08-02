using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using FMOD;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private PlayerLogic playerLogic;
    [SerializeField] private ShieldDetectable shield;
    [SerializeField] private TMP_Text playerHealth;
    [SerializeField] private TMP_Text shieldCooldown;

    [SerializeField] private bool isGamePaused;
    [SerializeField] private GameObject paused;
    private bool audioResumed = false;

    private void Awake()
    {
        playerLogic = FindFirstObjectByType<PlayerLogic>();
        shield = FindFirstObjectByType<ShieldDetectable>();

        playerHealth = GameObject.Find("PlayerHealth").GetComponent<TMP_Text>();
        shieldCooldown = GameObject.Find("ShieldCooldown").GetComponent<TMP_Text>();

        isGamePaused = false;
    }
    void Start()
    {
        GameMusicManager.Instance.SetLevelParameter(0);
    }

    private void Update()
    {
        // This block handles browser autoplay policies by resuming FMOD
        // on the first key press or mouse click.
        if (!audioResumed)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                // Get the high-level Studio System from the RuntimeManager
                FMOD.Studio.System studioSystem = FMODUnity.RuntimeManager.StudioSystem;

                // Get the low-level Core System from the Studio System
                studioSystem.getCoreSystem(out FMOD.System coreSystem);

                // Resume the mixer on the Core System. This is what un-suspends audio in a browser.
                coreSystem.mixerResume();

                audioResumed = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            isGamePaused = !isGamePaused;
            HandlePauseState();

        }
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
            SceneManager.LoadScene("MainLevel");
        }
    }

    private void HandlePauseState()
    {
        if(isGamePaused)
        {
            paused.SetActive(true);
            Time.timeScale = 0f;
            GameMusicManager.Instance.SetPauseState(true);
        }
        else
        {
            paused.SetActive(false);
            Time.timeScale = 1f;
            GameMusicManager.Instance.SetPauseState(false);
        }
    }

}
