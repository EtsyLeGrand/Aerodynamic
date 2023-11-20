using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanRoll : StickmanState
{
    private float rollSpeed = 0.5f;
    public StickmanRoll(StickmanBase stateMachine) : base(stateMachine)
    {
        Debug.Log("State: Stickman Roll");
        stateMachine.StateName = StickmanBase.StickmanStateName.Rolling;
        stateMachine.MainBody.constraints ^= RigidbodyConstraints.FreezeRotationZ;
    }

    public override void Execute()
    {
        if(!Input.GetKey(KeyCode.S))
        {
            ExitState(new StickmanRollToMain(stateMachine));
        }
    }

    public override void ExitState(StickmanState exitTo)
    {
        stateMachine.State = exitTo;
    }

    public override void FixedExecute()
    {
        stateMachine.MainBody.velocity += (Vector3.right * stateMachine.axisPercent * rollSpeed);
    }

    public override string GetName()
    {
        return "StickmanRoll";
    }
}
