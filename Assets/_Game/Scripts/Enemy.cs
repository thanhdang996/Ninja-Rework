using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float moveSpeed = 4f;
    private IState currentState;
    private bool isRight = true;

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());

    }

    protected override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }


    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = moveSpeed * transform.right;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {

    }

    public bool IsTargetInRange()
    {
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWall"))
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        isRight = !isRight;
        transform.rotation = Quaternion.Euler(0, isRight ? 0 : 180, 0);
    }
}