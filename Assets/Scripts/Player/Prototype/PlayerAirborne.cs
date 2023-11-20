using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborne : PlayerState
{
    private PlayerFeet feetScript;
    private SpriteRenderer handSpriteRenderer;
    private Rigidbody playerBody;
    private float torque;

    public PlayerAirborne(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        Debug.Log("State set to PlayerAirborne");
        Init();
    }

    private void Init()
    {
        handSpriteRenderer = stateMachine.HandSpriteRenderer;
        handSpriteRenderer.color = Color.red;
        playerBody = stateMachine.PlayerBody;
        feetScript = stateMachine.Feet.GetComponent<PlayerFeet>();

        torque = stateMachine.Torque;
    }

    public override void Execute()
    {
        Grab();
        Dangle();

        if (feetScript.OnGround)
        {
            ExitState(new PlayerMain(stateMachine));
        }
    }

    public override void FixedExecute()
    {

    }

    public override void ExitState(PlayerState exitTo)
    {
        stateMachine.CurrentState = exitTo;
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

    private void Dangle()
    {
        playerBody.AddTorque(60 * Time.deltaTime * Vector3.forward * Input.GetAxis("Horizontal") * -torque);
    }

    public override string GetName()
    {
        return "Airborne";
    }
}
