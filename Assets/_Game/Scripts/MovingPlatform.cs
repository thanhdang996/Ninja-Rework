using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform aPoint, bPoint;
    [SerializeField] private float speed = 5f;
    private Vector3 target;


    private void Start()
    {
        transform.position = aPoint.position;
        target = bPoint.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, bPoint.position) < 0.1f)
        {
            target = aPoint.position;
        }
        else if (Vector2.Distance(transform.position, aPoint.position) < 0.1f)
        {
            target = bPoint.position;
        }
    }

    // when disable script still run collision enter and ontrigger
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         collision.transform.SetParent(transform);

    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         collision.transform.SetParent(null);
    //     }
    // }
}
