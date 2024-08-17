using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    private Rigidbody2D myRigidBody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle; ;
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();    
        myRigidBody = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {
        CheckDistance();
    }


    void CheckDistance()
    {
        if (Vector3.Distance( target.position, 
            transform.position) <= chaseRadius
            && Vector3.Distance(target.position,transform.position)>attackRadius)
        {
            if (currentState == EnemyState.idle||currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidBody.MovePosition(temp);
                


                ChangeState(EnemyState.walk);
                animator.SetBool("wakeUp", true);
            }
        }
        else if(Vector3.Distance(target.position,
            transform.position) > chaseRadius)
        {
            animator.SetBool("wakeUp", false);
        }
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("moveX", setVector.x);
        animator.SetFloat("moveY", setVector.y);
    }

    private void changeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }else if(direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }


        }else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }


        }



    }

    private void ChangeState(EnemyState newState)
    {
        if(currentState!= newState)
        {
            currentState = newState;
        }
    }
}
