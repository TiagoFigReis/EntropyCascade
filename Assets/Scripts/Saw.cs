using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Animator animator;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Transform parentTransform;

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        parentTransform = transform.parent;
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
        
        isMoving = true;
    }
    
    public void OnHideComplete()
    {
        Destroy(parentTransform.gameObject);
    }
}
