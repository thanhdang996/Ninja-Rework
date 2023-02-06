using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(CoolDownShied(other));
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator CoolDownShied(Collider2D other)
    {
        other.GetComponent<Player>().shield.SetActive(true);
        other.GetComponent<Player>().IsShield = true;
        // other.tag = "ShieldPlayer";
        yield return new WaitForSeconds(5);
        // other.tag = "Player";
        other.GetComponent<Player>().shield.SetActive(false);
        other.GetComponent<Player>().IsShield = false;
        Destroy(gameObject);
    }
}
