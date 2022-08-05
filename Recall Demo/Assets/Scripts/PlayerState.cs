using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;   

    public IdleState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("idle");
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
    }

    public void OnUpdate()
    {
        if (parameter.isGroud)
        {
            if (keyboardState.pressedRight || keyboardState.pressedLeft)
            {
                playerFSM.Transition(stateType.run);
            }
            else if (keyboardState.pressedJump)
                playerFSM.Transition(stateType.jumpUp);
            else if (keyboardState.pressedAttack)
                playerFSM.Transition(stateType.attack_1);
        }
        else
            playerFSM.Transition(stateType.jumpDown);

    }

    public void OnExit()
    {

    }
}

public class RunState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public RunState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("run");
    }

    public void OnUpdate()
    {
        if (parameter.isGroud)
        { 
            if (keyboardState.pressedJump)
                playerFSM.Transition(stateType.jumpUp);
            else if (keyboardState.pressedAttack)           
                playerFSM.Transition(stateType.attack_1);
            else if (keyboardState.pressedRight)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(1, 1, 1);
                }
            else if (keyboardState.pressedLeft)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(-1, 1, 1);
                }          
            else
            {
                playerFSM.Transition(stateType.idle);
            }
        }
        else
        {
            playerFSM.Transition(stateType.jumpDown);
        }

    }

    public void OnExit()
    {

    }
}

public class JumpUpState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public JumpUpState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.jumpTimer = 0;
        parameter.playerAnimator.Play("jumpUp");
        parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerJumpSpeed);
    }

    public void OnUpdate()
    {
        playerFSM.PlayerGravity.GravitySimulate(playerFSM);
        parameter.jumpTimer += Time.deltaTime;

        if (parameter.jumpTimer<0.2f&&keyboardState.pressedJump)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerJumpSpeed);
        }
        

        if (keyboardState.pressedRight)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
            playerFSM.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (keyboardState.pressedLeft)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
            playerFSM.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (parameter.playerRigidbody.velocity.y<20)
        {
            playerFSM.Transition(stateType.jumpTop);
        }
        //else if (parameter.playerRigidbody.velocity.y<0)
        //{
        //    playerFSM.Transition(stateType.jumpDown);
        //}
    }

    public void OnExit()
    {
        parameter.jumpTimer = 0;
    }
}

public class JumpDownState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public JumpDownState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("jumpDown");
    }

    public void OnUpdate()
    {
        playerFSM.PlayerGravity.GravitySimulate(playerFSM);
        if (!parameter.isGroud)
        {
            if (keyboardState.pressedRight)
            {
                parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                playerFSM.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (keyboardState.pressedLeft)
            {
                parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                playerFSM.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
            playerFSM.Transition(stateType.idle);
    }

    public void OnExit()
    {

    }
}

public class JumpTopState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public JumpTopState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("jumpTop");
    }

    public void OnUpdate()
    {
        playerFSM.PlayerGravity.GravitySimulate(playerFSM);
        if (!parameter.isGroud)
        {
            if (parameter.playerRigidbody.velocity.y >= 0)
            {
                if (keyboardState.pressedRight)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (keyboardState.pressedLeft)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
        else
            playerFSM.Transition(stateType.idle);
    }

    public void OnExit()
    {

    }
}

public class Attack_1_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public Attack_1_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("attack_1");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
    }

    public void OnUpdate()
    {
        if (parameter.acceptNextOrder && keyboardState.pressedAttack)
            parameter.goAttack_2 = true;       

        if (parameter.currentAnimEnd)
        {
            if (parameter.isGroud)
            {
                if (parameter.goAttack_2)                
                    playerFSM.Transition(stateType.attack_2);                 
                else if (keyboardState.pressedRight || keyboardState.pressedLeft)
                    playerFSM.Transition(stateType.run);
                else
                    playerFSM.Transition(stateType.idle);
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
    }

    public void OnExit()
    {
        parameter.acceptNextOrder = false;
        parameter.goAttack_2 = false;
    }
}

public class Attack_2_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public Attack_2_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("attack_2");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
    }

    public void OnUpdate()
    {
        if (parameter.acceptNextOrder && keyboardState.pressedAttack)
            parameter.goAttack_3 = true;

        if (parameter.currentAnimEnd)
        {
            if (parameter.isGroud)
            {
                if (parameter.goAttack_3)
                    playerFSM.Transition(stateType.attack_3);
                else if (keyboardState.pressedRight || keyboardState.pressedLeft)
                    playerFSM.Transition(stateType.run);
                else
                    playerFSM.Transition(stateType.idle);
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
    }

    public void OnExit()
    {
        parameter.acceptNextOrder = false;
        parameter.goAttack_3 = false;
    }
}

public class Attack_3_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public Attack_3_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("attack_3");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
    }

    public void OnUpdate()
    {
        if (parameter.currentAnimEnd)
        {
            if (parameter.isGroud)
            {
                if (keyboardState.pressedRight || keyboardState.pressedLeft)
                    playerFSM.Transition(stateType.run);
                else
                    playerFSM.Transition(stateType.idle);
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
    }

    public void OnExit()
    {

    }
}