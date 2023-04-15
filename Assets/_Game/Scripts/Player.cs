using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private BoxCollider2D triggerHideMap;
    public GameObject shield;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    // [SerializeField] private float distanceToGround = 1.01f;
    [SerializeField] private LayerMask groundLayer;
    private float horizontal;
    private float dirY;

    private bool isGround;
    private bool isJumping;
    private bool isAttack;
    private bool isDead;
    public bool IsShield { get; set; }
    public bool AllowClimp { get; set; }

    private int coin;
    private Vector3 savePoint = Vector3.zero;
    [SerializeField] private Transform hidePointMap;
    [SerializeField] private int numberThrow;

    Coroutine resetSpeed = null;



    protected override void OnInit()
    {
        hp = 100;
        base.OnInit();
        transform.position = savePoint;
        isDead = false;
        isAttack = false;
        ChangeAnim("idle");
        StopAllCoroutines();
        StartCoroutine(HealHpPerSecond());
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
        rb.velocity = Vector2.zero;
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
            else if (Input.GetKeyDown(KeyCode.V) && !isAttack && numberThrow == 3)
            {
                numberThrow = 0;
                ThrowSpecial();
                // Debug.LogError("throw");
                return;
            }
            else if (Input.GetKeyDown(KeyCode.V) && !isAttack)
            {
                numberThrow++;
                ThrowNormal();
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
            if (AllowClimp)
            {
                //dirY = Input.GetAxisRaw("Vertical") * speed;
                if (dirY == 0)
                {
                    ChangeAnim("climb_idle");
                }
                if (dirY != 0)
                {
                    ChangeAnim("climb");
                }
            }
            else if (rb.velocity.y > 0)
            {
                ChangeAnim("jump");
            }
            else if (rb.velocity.y < 0)
            {
                isJumping = false;
                ChangeAnim("fall");
            }
        }


    }

    private void FixedUpdate()
    {
        if (AllowClimp)
        {
            rb.isKinematic = true;
            rb.velocity = new Vector2(rb.velocity.x, dirY * speed);
        }
        else
        {
            rb.isKinematic = false;
        }
    }
    private void HandleMovingAndRotate()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            transform.rotation = Quaternion.Euler(0, horizontal > 0 ? 0 : 180, 0);
        }
        rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);
        // print("move");
    }



    private bool IsGround()
    {
        // if (isJumping) return false;
        // Debug.DrawLine(transform.position, transform.position + (distanceToGround * Vector3.down), Color.red);
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);
        // return hit.collider != null;
        // de 0.99 cho do bi trung vao capsule collider
        if (isJumping) return false;
        RaycastHit2D hit2 = Physics2D.BoxCast(_collider.bounds.center, Vector2.one * 0.99f, 0, Vector2.down, _collider.bounds.extents.y - 0.48f, groundLayer);
        return hit2.collider != null;
    }

    public void Jump()
    {
        if (!enabled) return;
        if (!isGround) return;
        if (isAttack) return;
        ChangeAnim("jump");
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }


    public void Attack()
    {
        if (!enabled) return;
        if (!isGround) return;
        if (isAttack) return;
        ChangeAnim("attack");
        isAttack = true;
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
        Invoke(nameof(ResetAttack), 0.5f);
    }


    public void ThrowNormal()
    {
        if (!enabled) return;
        if (!isGround) return;
        if (isAttack) return;
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }
    private void ThrowSpecial()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position + Vector3.up * 0.5f, throwPoint.rotation);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
        Instantiate(kunaiPrefab, throwPoint.position + Vector3.down * 0.5f, throwPoint.rotation);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    public void SetUp(float dirY)
    {
        this.dirY = dirY;
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
        savePoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coin++;
            if (coin == 7)
            {
                // transform.position = hidePointMap.position;
                triggerHideMap.isTrigger = true;
            }
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
        else if (other.CompareTag("BoosterSpeed"))
        {
            Destroy(other.gameObject);
            speed = 10f;
            if (resetSpeed != null)
                StopCoroutine(resetSpeed);
            resetSpeed = StartCoroutine(ResetSpeed());
        }
    }

    private IEnumerator HealHpPerSecond()
    {
        while (!IsDead)
        {
            if (hp < 100)
            {
                hp += 3f;
                healthBar.SetHp(hp);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(10f);
        print(2);
        speed = 5f;
    }

    public override void OnHit(float damage)
    {
        if (IsShield) return;
        base.OnHit(damage);
    }
}
