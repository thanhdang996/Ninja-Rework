using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 4f;
    private IState currentState;
    private bool isRight = true;

    // nen de la Character vi ko phai tham chieu toi player trong truong hop muon tao prefab cho enemy
    // neu tao prefab thi cac thuoc tinh ko dc tham chieu toi cac object khac, chi tham chieu trong cung 1 gameoject thoi
    // vi khi de thanh prefab no se missing cac tham chieu ben ngoai
    private Character target;
    public Character Target => target;

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
        DeActiveAttack();
        ChangeState(new IdleState());

    }

    protected override void OnDespawn()
    {
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        rb.velocity = Vector2.zero;
        ChangeState(null);
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

    public void SetTarget(Character target)
    {
        this.target = target;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if (target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
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
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }


    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) < attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWall"))
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = Quaternion.Euler(0, isRight ? 0 : 180, 0);
        // Moving();
    }
}