using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float distanceToGround = 1.01f;
    [SerializeField] private LayerMask groundLayer;
    private float horizontal;

    private bool isGround;
    private bool isJumping;
    private bool isAttack;
    private bool isDead;

    private int coin;
    [SerializeField] private Transform savePoint;



    protected override void OnInit()
    {
        base.OnInit();
        transform.position = savePoint.position;
        isDead = false;
        isAttack = false;
        ChangeAnim("idle");

        DeActiveAttack();
    }

    protected override void OnDespawn()
    {
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        isDead = true;
    }

    private void Update()
    {
        // neu dead hoac dang attack thi ko update van toc va rotate player
        if (isDead) return;
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        HandleMovingAndRotate();

        isGround = IsGround();
        if (isGround)
        {

            if (isJumping) return;  //them dieu o day, vi co nhung frame no van chay dieu kien phia duoi if (horizontal != 0) hoac else 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                // Debug.LogError("jump");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.C) && !isAttack)
            {
                Attack();
                // Debug.LogError("attack");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.V) && !isAttack)
            {
                Throw();
                // Debug.LogError("throw");
                return;
            }


            if (horizontal != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // ngan velocity Y khi dang run bi so am rat nho
                ChangeAnim("run");
            }
            else
            {
                // để dòng này để velocity luôn khác 0, để rigidbody ko rơi vào trang thái sleep ( khi velocity = 0) thì sẽ ko  chạy hàm Ontrigger đc 
                // rb.velocity = new Vector2(rb.velocity.x, -1);
                // hoặc có thể thay đổi sleeping mode trong rigidbody là never sleep, thì khi velocity = 0 , rigid vẫn ở state Awake và chạy dc hàm trigger

                ChangeAnim("idle");
                // print("idle");
            }
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                isJumping = false;
                ChangeAnim("fall");
            }
        }
    }

    private void HandleMovingAndRotate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            transform.rotation = Quaternion.Euler(0, horizontal > 0 ? 0 : 180, 0);
        }
        rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);
        // print("move");
    }

    private bool IsGround()
    {
        if (isJumping) return false;
        Debug.DrawLine(transform.position, transform.position + (distanceToGround * Vector3.down), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);
        return hit.collider != null;
    }

    private void Jump()
    {
        ChangeAnim("jump");
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }


    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
        Invoke(nameof(ResetAttack), 0.5f);
    }


    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    private void ResetAttack()
    {
        isAttack = false;
    }

    private void Dead()
    {
        ChangeAnim("die");
        isDead = true;
        Invoke(nameof(OnInit), 1f);
    }



    private void SavePoint()
    {
        savePoint.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coin++;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("DeadZone"))
        {
            Dead();
        }
        else if (other.CompareTag("SavePoint"))
        {
            SavePoint();
        }
    }
}
