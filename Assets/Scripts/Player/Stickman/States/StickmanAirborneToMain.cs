using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAirborneToMain : StickmanState
{
    private Rigidbody[] bodyParts;
    private float currentTime = 0f;
    private float standingTime = 1f;
    private float initialRot;
    private float targetVal;
    public StickmanAirborneToMain(StickmanBase stateMachine) : base(stateMachine)
    {
        Debug.Log("State Transition: Airborne To Main");
        stateMachine.StateName = StickmanBase.StickmanStateName.AirborneToMain;
        Debug.Log(stateMachine.StateName);
        bodyParts = stateMachine.BodypartBodies;

        initialRot = stateMachine.gameObject.transform.rotation.eulerAngles.z;

        if (initialRot <= 45f) //0 à 45
        {
            standingTime = (initialRot / 45f);
            targetVal = 0f;
        }
        else //315 à 360
        {
            standingTime = (initialRot - 315f) / 45f;
            targetVal = 360f;
        }

        Init();
    }

    private void Init()
    {
        foreach (Rigidbody bodypart in bodyParts)
        {
            bodypart.isKinematic = false;
        }
    }

    public override void Execute()
    {
        currentTime += Time.deltaTime;
        stateMachine.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(initialRot, targetVal, currentTime / standingTime));
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
        return "StickmanAirborneToMain";
    }
}
