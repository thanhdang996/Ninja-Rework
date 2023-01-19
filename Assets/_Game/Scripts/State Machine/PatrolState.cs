using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private float timer;
    private float randomTime;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer < randomTime)
        {
            // phai de moving o On Execute vi khi changeDirection se update Moving
            // (vi khi changedirection se change rotate va velocity co transform.right nen thay doi velocity)
            enemy.Moving();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
