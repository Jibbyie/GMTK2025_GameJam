using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    [SerializeField] private PlayerLogic playerLogic;

    private void Awake()
    {
        playerLogic = FindFirstObjectByType<PlayerLogic>();
    }
    public void RetryStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RetryCheckpoint()
    {
        playerLogic.RespawnAndHeal();
    }

}
