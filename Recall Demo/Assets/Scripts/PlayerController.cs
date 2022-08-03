using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum stateType
{
    idle, run, jumpUp, jumpDown, jumpTop, attack_1, attack_2, attack_3
}
public class PlayerController : MonoBehaviour
{
    public KeyBoardDetection PlayerKeyboardStates = new KeyBoardDetection();  //角色按键状态
    public PlayerParameter playerParameter = new PlayerParameter();  //角色参数
    private Gravity PlayerGravity = new Gravity();  //重力模拟
    public bool slow;
    public float slowSpeed=0.1f;

    public Dictionary<stateType, IState> playerStates = new Dictionary<stateType, IState>();  //角色状态
    public IState currentState;  //角色当前状态

    void Awake()
    {        
        playerParameter.playerRigidbody = this.GetComponent<Rigidbody2D>();
        playerParameter.playerAnimator = this.GetComponent<Animator>();

        playerStates.Add(stateType.idle, new IdleState(this));
        playerStates.Add(stateType.run, new RunState(this));
        playerStates.Add(stateType.jumpUp, new JumpUpState(this));
        playerStates.Add(stateType.jumpDown, new JumpDownState(this));
        playerStates.Add(stateType.jumpTop, new JumpTopState(this));
        playerStates.Add(stateType.attack_1, new Attack_1_State(this));
        playerStates.Add(stateType.attack_2, new Attack_2_State(this));
        playerStates.Add(stateType.attack_3, new Attack_3_State(this));

        Transition(stateType.jumpDown);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerGravity.GravitySimulate(this);
        PlayerKeyboardStates.KeyDown();

        currentState.OnUpdate();

        if (slow)
        {
            Time.timeScale = slowSpeed;
        }
    }

    //void FixedUpdate()
    //{
    //    //playerParameter.playerRigidbody.velocity = new Vector2(playerParameter.playerRigidbody.velocity.x, PlayerGravity.GravitySimulate(playerParameter.playerRigidbody.velocity).y);
    //    //PlayerKeyboardStates.KeyDown();

    //    //currentState.OnUpdate();
    //    ////PlayerMove();
    //    ////PlayerJump();

    //}

    public void Transition(stateType stateType)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = playerStates[stateType];
        currentState.OnEnter();
    }

    public void CurrentAnimEnd()
    {
        this.playerParameter.currentAnimEnd = true;

    }

    public void CanAcceptNextOrder()
    {
        this.playerParameter.acceptNextOrder = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Ground")
            playerParameter.isGroud = true;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
            playerParameter.isGroud = false;
    }

}
public class KeyBoardDetection   //按键检测
{
    public bool pressedUp = false;  //是否按下上键
    public bool pressedDown = false;  //是否按下下键
    public bool pressedLeft = false;   //是否按下左键
    public bool pressedRight = false;  //是否按下右键
    public bool pressedJump = false;  //是否按下跳跃键
    public bool pressedAttack = false;  //是否按下攻击键
    public bool pressedRush = false;  //是否按下冲刺键
    public bool pressedTimeBack = false;  //是否按下时间回退
    public PlayerParameter playerParameter = new PlayerParameter();

    public void KeyDown()  //检测键盘按键
    {
        if (Input.GetKey(Const.Control[0]))
            pressedUp = true;
        else
            pressedUp = false;

        if (Input.GetKey(Const.Control[1]))
            pressedDown = true;
        else
            pressedDown = false;

        if (Input.GetKey(Const.Control[2]))
            pressedLeft = true;
        else
            pressedLeft = false;

        if (Input.GetKey(Const.Control[3]))
            pressedRight = true;
        else
            pressedRight = false;

        if (Input.GetKey(Const.Control[4]))       
            pressedJump = true;
        else
            pressedJump = false;

        if (Input.GetKeyDown(Const.Control[5]))
            pressedAttack = true;
        else
            pressedAttack = false;

        if (Input.GetKeyDown(Const.Control[5]))
        {
            pressedRush = true;
        }


        if (Input.GetKey(Const.Control[6]))
        {
            pressedTimeBack = true;
        }
        else
        {
            pressedTimeBack = false;
        }

    }

}

public class PlayerParameter  //角色属性参数
{
    public bool isGroud = false;
    public float playerMoveSpeed = 20;
    public float playerJumpSpeed = 40;
    public Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public float jumpTimer;  //跳跃计时器
    public bool currentAnimEnd;  //当前动画是否结束
    public bool acceptNextOrder;  //是否可预输入下一个指令
    public bool goAttack_2;
    public bool goAttack_3;
}

public class Gravity  //重力模拟
{
    float acceleration = -0.8f;  //重力加速度  每固定帧
    float maxDropSpeed = 30;  //最大下落速度
    Vector2 velocity;
    public void GravitySimulate(PlayerController playerController)
    {
        //    if (playerController.GetType() == typeof(JumpUpState))
        //    {
        //        if (playerController.playerParameter.playerRigidbody.velocity.y > -1 * maxDropSpeed && playerController.playerParameter.jumpTimer > 0.02f)
        //            playerController.playerParameter.playerRigidbody.AddForce(new Vector2(0, -70));
        //    }else
        //        playerController.playerParameter.playerRigidbody.AddForce(new Vector2(0, -70));

        if (playerController.GetType() == typeof(JumpUpState))
        {
            if (playerController.playerParameter.playerRigidbody.velocity.y > -1 * maxDropSpeed && playerController.playerParameter.jumpTimer > 0.02f)
                playerController.playerParameter.playerRigidbody.velocity = new Vector2(playerController.playerParameter.playerRigidbody.velocity.x, playerController.playerParameter.playerRigidbody.velocity.y+acceleration);
        }
        else
            playerController.playerParameter.playerRigidbody.velocity = new Vector2(playerController.playerParameter.playerRigidbody.velocity.x, playerController.playerParameter.playerRigidbody.velocity.y + acceleration);
    }
}