using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    private float maxHp;
    private float hp;

    private float speed = 5f;

    // private Transform target;
    // [SerializeField] private Vector3 offset;


    public void OnInit(float maxHp)
    {
        this.maxHp = maxHp;
        this.hp = this.maxHp;
        healthBarFill.fillAmount = 1;

        // this.target = target;
    }

    private void Update()
    {
        healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, hp / maxHp, speed * Time.deltaTime);
        // transform.position = target.position + offset;
    }

    public void SetHp(float hp)
    {
        this.hp = hp;
    }
}
