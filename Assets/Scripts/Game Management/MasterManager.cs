using UnityEngine;
using UnityEngine.Audio;

public class MasterManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    public static double TimeInSeconds { get; private set; }
    public static double Fps { get; private set; }
    public static double Ping { get; private set; } // UNSET

    private void Awake()
    {
        KeepPersistentStatus();
    }

    private void Start()
    {
        Init();
        KeepPersistentStatus();
    }

    private void Update()
    {
        Fps = 1 / Time.unscaledDeltaTime;
        TimeInSeconds += Time.unscaledDeltaTime;
    }

    private void KeepPersistentStatus()
    {
        int gameStatusCount = FindObjectsOfType<MasterManager>().Length;
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Init()
    {
        TimeInSeconds = 0d;
        Fps = 0d;
        Ping = 0d;
    }
}
