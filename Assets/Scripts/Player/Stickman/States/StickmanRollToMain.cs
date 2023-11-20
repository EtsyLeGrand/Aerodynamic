using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanRollToMain : StickmanState
{
    private float currentTime = 0f;
    private float standingTime = 1f;

    private float initialRot;

    private float mainColliderSize = 25f;
    private float mainColliderSizeTarget = 52.5f;
    public StickmanRollToMain(StickmanBase stateMachine) : base(stateMachine)
    {
        Debug.Log("State: Stickman Roll to Main");
        stateMachine.StateName = StickmanBase.StickmanStateName.RollingToMain;
        stateMachine.MainBody.constraints ^= RigidbodyConstraints.FreezeRotationZ;

        initialRot = stateMachine.gameObject.transform.rotation.eulerAngles.z;
    }

    public override void Execute()
    {
        currentTime += Time.deltaTime;
        stateMachine.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(initialRot, 0, currentTime / standingTime));
        stateMachine.MainCollider.height = Mathf.Lerp(mainColliderSize, mainColliderSizeTarget, currentTime / standingTime);
        // Ça devrait être moins long de se lever quand la rotation est plus près de 0
        if (currentTime >= standingTime)
        {
            ExitState(new StickmanMain(stateMachine));
        }
    }

    public override void ExitState(StickmanState exitTo)
    {
        stateMachine.State = exitTo;
    }

    public override void FixedExecute()
    {
        
    }

    public override string GetName()
    {
        return "StickmanRoll";
    }
}
