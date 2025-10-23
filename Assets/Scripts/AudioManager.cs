using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.volume = 0.05f;
        _audioSource.clip = backgroundMusic;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    
}