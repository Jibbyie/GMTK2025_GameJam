using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private PlayerLogic playerLogic;
    [SerializeField] private ShieldDetectable shield;
    [SerializeField] private TMP_Text playerHealth;
    [SerializeField] private Image shieldIcon;

    [SerializeField] private bool isGamePaused;
    [SerializeField] private GameObject paused;

    private Coroutine shieldFadeCoroutine;
    private bool isShieldReadyState = true;

    private void Awake()
    {
        playerLogic = FindFirstObjectByType<PlayerLogic>();
        shield = FindFirstObjectByType<ShieldDetectable>();

        playerHealth = GameObject.Find("PlayerHealth").GetComponent<TMP_Text>();
        shieldIcon = GameObject.Find("ShieldIcon").GetComponent<Image>();

        isGamePaused = false;
    }
    void Start()
    {
        GameMusicManager.Instance.SetLevelParameter(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isGamePaused = !isGamePaused;
            HandlePauseState();

        }
        //SpawnEnemy();
        ReloadScene();
        playerHealth.text = playerLogic.GetHealth().ToString("F0");

        if (shield.IsShieldActive())
        {
            if (isShieldReadyState)
            {
                isShieldReadyState = false;
                StartFade(0.4f, 0.5f);
            }
        }
        else
        {
            if (!isShieldReadyState)
            {
                isShieldReadyState = true;
                StartFade(1f, 0.5f);
            }
        }
    }

    private void StartFade(float targetAlpha, float duration)
    {
        if (shieldFadeCoroutine != null)
        {
            StopCoroutine(shieldFadeCoroutine);
        }
        shieldFadeCoroutine = StartCoroutine(FadeShieldIcon(targetAlpha, duration));
    }

    private IEnumerator FadeShieldIcon(float targetAlpha, float duration)
    {
        Color currentColor = shieldIcon.color;
        float startAlpha = currentColor.a;
        float elapsedTimer = 0f;

        while (elapsedTimer < duration)
        {
            elapsedTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTimer / duration);
            currentColor.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
            shieldIcon.color = currentColor;
            yield return null;
        }

        currentColor.a = targetAlpha;
        shieldIcon.color = currentColor;
    }


    //private void SpawnEnemy()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Vector3 mousePos = Input.mousePosition;
    //        mousePos.z = 10;

    //        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
    //        worldPos.z = 0;

    //        Instantiate(enemyPrefab, worldPos, Quaternion.identity);
    //    }
    //}

    private void ReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void HandlePauseState()
    {
        if (isGamePaused)
        {
            //paused.SetActive(true);
            Time.timeScale = 0f;
            GameMusicManager.Instance.SetPauseState(true);
        }
        else
        {
            //paused.SetActive(false);
            Time.timeScale = 1f;
            GameMusicManager.Instance.SetPauseState(false);
        }
    }
}