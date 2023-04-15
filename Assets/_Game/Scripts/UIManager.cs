using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Ins {get; private set;}

    [SerializeField] private GameObject winGO;

    private void Awake()
    {
        if(Ins == null) Ins = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowWinGO()
    {
        winGO.SetActive(true);
    }
}
