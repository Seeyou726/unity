using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyIdleState:IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    private Vector2 playerPos;
    private float actTime;
    public EnemyIdleState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
        playerPos = parameter.player.transform.position;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("idle");
        parameter.enemyRigidbody.velocity = new Vector2(0, 0);
        parameter.actTimer = 0;
        actTime = Random.Range(1f, 4f);
    }

    public void OnUpdate()
    {
        parameter.actTimer += Time.deltaTime;
        if (Mathf.Abs(playerPos.x-enemyFSM.transform.position.x)<parameter.enemyAlertRange.x&& Mathf.Abs(playerPos.y - enemyFSM.transform.position.y) < parameter.enemyAlertRange.y)
        {
            enemyFSM.Transition(enemyStateType.chase);
        }
        else
        {
            if (parameter.actTimer > actTime)
                enemyFSM.Transition(enemyStateType.patrol);
        }

    }

    public void OnExit()
    {
        parameter.actTimer = 0;
    }
}

public class EnemyPatrolState : IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    private Vector2 playerPos;
    private int direciton;
    private float actTime;
    public EnemyPatrolState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
        playerPos = parameter.player.transform.position;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("walk");
        parameter.actTimer = 0;

        if (parameter.arriveBorder)
        {
            direciton= enemyFSM.transform.position.x - parameter.contactBorder.transform.position.x>0?1:-1;
        }
        else
        {
            direciton = Random.Range(0, 2)<0.5?-1:1;
        }
        parameter.arriveBorder = false;
        actTime = Random.Range(2f, 5f);
    }

    public void OnUpdate()
    {
        parameter.actTimer += Time.deltaTime;

        if (Mathf.Abs(playerPos.x - enemyFSM.transform.position.x) < parameter.enemyAlertRange.x && Mathf.Abs(playerPos.y - enemyFSM.transform.position.y) < parameter.enemyAlertRange.y)
        {
            enemyFSM.Transition(enemyStateType.chase);
        }
        else
        {
            if (Mathf.Abs(enemyFSM.transform.position.x-enemyFSM.bornPoint.transform.position.x)<30)
            {
                if (parameter.arriveBorder || parameter.actTimer > actTime)
                {
                    enemyFSM.Transition(enemyStateType.idle);
                }
                else
                {
                    enemyFSM.transform.localScale = new Vector3(direciton, 1, 1);
                    parameter.enemyRigidbody.velocity = new Vector2(parameter.enemyMoveSpeed * direciton, 0);
                }
            }
            else
            {
                enemyFSM.Transition(enemyStateType.backToBornPoint);
            }
        }
            
    }

    public void OnExit()
    {
        parameter.actTimer = 0;
    }
}

public class EnemyChaseState : IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    public EnemyChaseState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
    }
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        if (true)
        {

        }
    }

    public void OnExit()
    { }
}

public class EnemyWalkBackState : IState
{
    public EnemyWalkBackState(EnemyController enemyController)
    {
        
    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class EnemyAttack_1_State : IState
{
    public EnemyAttack_1_State(EnemyController enemyController)
    {

    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class EnemyAttack_2_State : IState
{
    public EnemyAttack_2_State(EnemyController enemyController)
    {

    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class EnemyAttack_3_State : IState
{
    public EnemyAttack_3_State(EnemyController enemyController)
    {

    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class BackToBornPointState : IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    private int direciton;
    public BackToBornPointState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
    }
    public void OnEnter()
    {
        direciton = enemyFSM.transform.position.x - enemyFSM.bornPoint.transform.position.x > 0 ? -1 : 1;
        parameter.enemyAnimator.Play("walk");
    }

    public void OnUpdate()
    {
        if (Mathf.Abs(enemyFSM.transform.position.x - enemyFSM.bornPoint.transform.position.x)>3)
        {
            enemyFSM.transform.localScale = new Vector3(direciton, 1, 1);
            parameter.enemyRigidbody.velocity = new Vector2(parameter.enemyMoveSpeed * direciton, 0);
        }
        else
        {
            enemyFSM.Transition(enemyStateType.idle);
        }
    }

    public void OnExit()
    { }
}

public class EnemyBeAttackedState : IState
{
    public EnemyBeAttackedState(EnemyController enemyController)
    {

    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}