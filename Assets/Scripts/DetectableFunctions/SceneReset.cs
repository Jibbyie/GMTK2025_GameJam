using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReset : DetectableObject
{
    public override void OnDetected()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
