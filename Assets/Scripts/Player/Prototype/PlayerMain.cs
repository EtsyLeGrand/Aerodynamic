using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : PlayerState
{
    private float speed = 5.0f;
    private float jumpForce = 6.0f;
    private GameObject hand;

    private SpriteRenderer handSpriteRenderer;
    
    private Rigidbody playerBody;

    public PlayerMain(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        Debug.Log("State set to PlayerMain");
        playerBody = stateMachine.PlayerBody;
        handSpriteRenderer = stateMachine.HandSpriteRenderer;
    }

    private void Move()
    {
        float hAxis = Input.GetAxis("Horizontal");
        playerBody.velocity = new Vector3(hAxis * speed, playerBody.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpImpulse = playerBody.velocity.y + jumpForce;
            if (jumpImpulse > jumpForce) jumpImpulse = jumpForce;
            playerBody.velocity = new Vector3(playerBody.velocity.x, jumpImpulse);
        }
    }

    private void Grab()
    {
        if (Input.GetMouseButton(0))
        {
            handSpriteRenderer.color = Color.yellow;
        }
        else
        {
            handSpriteRenderer.color = Color.red;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown("S"))
        {
            ExitState(new PlayerRoll(stateMachine));
        }
    }

    public override void Execute()
    {
        Move();
        Jump();
        Grab();
        Crouch();
    }

    public override void FixedExecute()
    {

    }

    public override void ExitState(PlayerState exitTo)
    {
        stateMachine.CurrentState = exitTo;
    }

    public override string GetName()
    {
        return "Main";
    }
}
