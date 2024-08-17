using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    walk,
    attack,
    stagger,
    idle
}
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector3 change;

    private Rigidbody2D myRigidBody;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    public LayerMask interactablesLayer;

    public PlayerState currentState;

    private bool isInDialog = false;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        HandleUpdate();
    }

    public void HandleUpdate()
    {
        if (!isMoving && !isInDialog)
        {
            change = Vector2.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");

            Debug.Log("This is change.x" + change.x);
            Debug.Log("This is change.y" + change.y);



            if (change.x != 0 ) 
            {
                change.y = 0;
            }

            if (change != Vector3.zero)
            {
                animator.SetFloat("moveX", change.x);
                animator.SetFloat("moveY", change.y);

                var targetPos = transform.position;
                targetPos.x += change.x;
                targetPos.y += change.y;

                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }

            if (Input.GetButtonDown("attack") & currentState != PlayerState.attack)
            {
                StartCoroutine(AttackCo());
            }
        }
        animator.SetBool("isMoving", isMoving);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;

        yield return null;

        animator.SetBool("attacking", false);

        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

   void Interact()
{
    if (!isInDialog)
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactablesLayer);
        if (collider != null)
        {
            isInDialog = true;
            collider.GetComponent<Interactable>()?.Interact();
            isInDialog = false;
        }
    }
}




    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos,0.3f,solidObjectsLayer | interactablesLayer)!= null)
        {
            return false;
        }
        return true;
    }


}
