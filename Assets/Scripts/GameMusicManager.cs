using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    // A static instance allows other scripts to access this manager easily.
    public static GameMusicManager Instance { get; private set; }

    // Create a variable in the Inspector to hold your main music event.
    [SerializeField] private FMODUnity.EventReference musicEvent;

    // This will hold the actual running instance of our music.
    private FMOD.Studio.EventInstance musicInstance;

    private void Awake()
    {
        // Set up the Singleton pattern.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes music persist between scenes.
        }
    }

    private void Start()
    {
        // Create an instance of the FMOD event and start it.
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    // --- Public Functions to Control Parameters ---

    public void SetLevelParameter(int levelValue)
    {
        // FMOD uses floats, so we pass the integer in.
        musicInstance.setParameterByName("Level", levelValue);
    }

    public void SetLowHealthParameter(bool isLow)
    {
        // Use a ternary operator to convert the boolean to 1f (on) or 0f (off).
        musicInstance.setParameterByName("LowHealth", isLow ? 1f : 0f);
    }

    public void SetPauseState(bool isPaused)
    {
        musicInstance.setParameterByName("Pause_State", isPaused ? 1f : 0f);
    }

    // --- Cleanup ---

    private void OnDestroy()
    {
        // Stop the music and release the memory when the manager is destroyed.
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}