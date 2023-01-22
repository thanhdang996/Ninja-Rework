using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private Animator anim;
    [SerializeField] private CombatText combatTextPrefab;
    private string currentAnimName;
    protected float hp;
    public bool IsDead => hp <= 0;

    private void Start()
    {
        OnInit();
    }


    protected virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(hp);
        enabled = true;
    }

    protected virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        enabled = false;
        Invoke(nameof(OnDespawn), 2f);
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(animName);
        }
    }

    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            if (IsDead)
            {
                hp = 0;
                OnDeath();
            }
        }
        Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        healthBar.SetHp(hp);
    }
}
