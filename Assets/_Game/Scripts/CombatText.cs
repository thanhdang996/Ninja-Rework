using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] private Text combatText;


    public void OnInit(float damage)
    {
        SetCombatText(damage);
        Invoke(nameof(OnDespawn), 1f);
    }

    private void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void SetCombatText(float damage)
    {
        combatText.text = damage.ToString();
    }
}
