using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLeanTween : MonoBehaviour
{
    public GameObject platform;
    public float speed = 5; // khi dung leantween chi set dc speed o trang thai ban dau
    private void Start()
    {
        LeanTween.moveLocalY(platform, 8, speed).setEaseInOutCubic().setLoopPingPong();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
