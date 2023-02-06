using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMap : MonoBehaviour
{
    [SerializeField] private GameObject TriggerHideMap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x > transform.position.x)
            {
                TriggerHideMap.SetActive(false);
            }
            else if (other.transform.position.x < transform.position.x)
            {
                TriggerHideMap.SetActive(true);
            }
        }
    }
}
