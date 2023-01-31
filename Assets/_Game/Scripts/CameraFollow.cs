using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed = 5f;

    private void FixedUpdate()
    {
        Vector3 playerPosition = playerTransform.position;
        transform.position = Vector3.Lerp(transform.position, playerPosition + offset, speed * Time.fixedDeltaTime);
    }


    // C2
    // private void LateUpdate()
    // {
    //     Vector3 playerPosition = playerTransform.position;
    //     transform.position = playerPosition + offset;
    // }
}
