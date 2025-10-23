using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AudioClip sound;

    private Animator animator;
    private AudioSource audioSource;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Transform parentTransform;

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        parentTransform = transform.parent;

        audioSource = GetComponentInParent<AudioSource>();
        audioSource.clip = sound;
        audioSource.volume = 0.01f;
    }
    
    public void Initialize(Vector2 target)
    {
        this.targetPosition = target;
    }

    void Update()
    {
        if (isMoving)
        {
            parentTransform.position = Vector2.MoveTowards(
                parentTransform.position, 
                targetPosition, 
                moveSpeed * Time.deltaTime
            );
            
            if (Vector2.Distance(parentTransform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                animator.SetTrigger("StartHiding");
            }
        }
    }
    
    public void OnEmergeComplete()
    {
        animator.Play("SawIdle"); 
        
        if (!audioSource.isPlaying)
            audioSource.Play();
        
        isMoving = true;
    }
    
    public void OnHideComplete()
    {
        Destroy(parentTransform.gameObject);
    }
}
