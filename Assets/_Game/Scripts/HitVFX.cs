using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVFX : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}
