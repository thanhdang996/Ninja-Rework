using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDetector : MonoBehaviour
{
    [SerializeField] private Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rope"))
        {
            player.AllowClimp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Rope"))
        {
            player.AllowClimp = false;
        }
    }
}
