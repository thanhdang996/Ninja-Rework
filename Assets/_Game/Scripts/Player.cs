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
                // ????? d??ng n??y ????? velocity lu??n kh??c 0, ????? rigidbody ko r??i v??o trang th??i sleep ( khi velocity = 0) th?? s??? ko  ch???y h??m Ontrigger ??c 
                // rb.velocity = new Vector2(rb.velocity.x, -1);
                // ho???c c?? th??? thay ?????i sleeping mode trong rigidbody l?? never sleep, th?? khi velocity = 0 , rigid v???n ??? state Awake v?? ch???y dc h??m trigger

                ChangeAnim("idle");
                // print("idle");
            }
        }
        else
        {
            if (AllowClimp)
            {
                dirY = Input.GetAxisRaw("Vertical") * speed;
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
            rb.velocity = new Vector2(rb.velocity.x, dirY);
        }
        else
        {
            rb.isKinematic = false;
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
        // if (isJumping) return false;
        // Debug.DrawLine(transform.position, transform.position + (distanceToGround * Vector3.down), Color.red);
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);
        // return hit.collider != null;
        // de 0.99 cho do bi trung vao capsule collider
        if (isJumping) return false;
        RaycastHit2D hit2 = Physics2D.BoxCast(_collider.bounds.center, Vector2.one * 0.99f, 0, Vector2.down, _collider.bounds.extents.y - 0.48f, groundLayer);
        return hit2.collider != null;
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


    private void ThrowNormal()
    {
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
        speed = 5f;
    }

    public override void OnHit(float damage)
    {
        if (IsShield) return;
        base.OnHit(damage);
    }
}
