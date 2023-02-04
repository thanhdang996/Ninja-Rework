using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapGround : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(OnDespawn), 0.5f);
        }
    }

    private void OnInit()
    {
        gameObject.SetActive(true);
    }

    private void OnDespawn()
    {
        gameObject.SetActive(false);
        Invoke(nameof(OnInit), 2f);
    }
}
