using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private IceStone iceStonePrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnIceStone());
    }

    private IEnumerator SpawnIceStone()
    {
        while (true)
        {
            Vector3 pos = transform.position;
            pos.x = Random.Range(-2f, 2f);
            Instantiate(iceStonePrefab, pos, Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            pos.x = Random.Range(-2f, 2f);
            Instantiate(iceStonePrefab, pos, Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }
}
