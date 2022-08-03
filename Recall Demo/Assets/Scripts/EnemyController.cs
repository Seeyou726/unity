using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyStateType
{
    idle, patrol, walk_back, attack_1, attack_2, attack_3
}
public class EnemyController : MonoBehaviour
{
    public EnemyParameter enemyParameter = new EnemyParameter();  //敌人参数
    private EnemyGravity enemyGravity = new EnemyGravity();  //重力模拟

    public Dictionary<enemyStateType, IState> enemyStates = new Dictionary<enemyStateType, IState>();  //敌人状态
    public IState enemyCurrentState;  //敌人当前状态

    private void Awake()
    {
        enemyParameter.enemyRigidbody = this.GetComponent<Rigidbody2D>();
        enemyParameter.enemyAnimator = this.GetComponent<Animator>();

        enemyStates.Add(enemyStateType.idle, new EnemyIdleState(this));
        enemyStates.Add(enemyStateType.patrol, new EnemyPatrolState(this));
        enemyStates.Add(enemyStateType.walk_back, new EnemyWalkBackState(this));
        enemyStates.Add(enemyStateType.attack_1, new EnemyAttack_1_State(this));
        enemyStates.Add(enemyStateType.attack_2, new EnemyAttack_2_State(this));
        enemyStates.Add(enemyStateType.attack_3, new EnemyAttack_3_State(this));

        Transition(enemyStateType.idle);
    }
    // Update is called once per frame
    void Update()
    {
        enemyGravity.GravitySimulate(this);
    }

    public void Transition(enemyStateType enemyStateType)
    {
        if (enemyCurrentState != null)
            enemyCurrentState.OnExit();
        enemyCurrentState = enemyStates[enemyStateType];
        enemyCurrentState.OnEnter();
    }

    public void CurrentAnimEnd()
    {
        this.enemyParameter.currentAnimEnd = true;

    }
}

public class EnemyParameter  //角色属性参数
{
    public bool isGroud = false;
    public float enemyMoveSpeed = 20;
    public float enemyJumpSpeed = 40;
    public Rigidbody2D enemyRigidbody;
    public Animator enemyAnimator;
    public float jumpTimer;  //跳跃计时器
    public bool currentAnimEnd;  //当前动画是否结束
    public bool acceptNextOrder;  //是否可预输入下一个指令
    public bool goAttack_2;
    public bool goAttack_3;
}

public class EnemyGravity  //重力模拟
{
    float acceleration = -0.8f;  //重力加速度  每固定帧
    float maxDropSpeed = 30;  //最大下落速度
    Vector2 velocity;
    public void GravitySimulate(EnemyController enemyController)
    {
        if (enemyController.GetType() == typeof(JumpUpState))
        {
            if (enemyController.enemyParameter.enemyRigidbody.velocity.y > -1 * maxDropSpeed && enemyController.enemyParameter.jumpTimer > 0.02f)
                enemyController.enemyParameter.enemyRigidbody.velocity = new Vector2(enemyController.enemyParameter.enemyRigidbody.velocity.x, enemyController.enemyParameter.enemyRigidbody.velocity.y + acceleration);
        }
        else
            enemyController.enemyParameter.enemyRigidbody.velocity = new Vector2(enemyController.enemyParameter.enemyRigidbody.velocity.x, enemyController.enemyParameter.enemyRigidbody.velocity.y + acceleration);
    }
}