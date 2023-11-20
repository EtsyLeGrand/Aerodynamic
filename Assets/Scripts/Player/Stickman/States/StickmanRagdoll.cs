using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanRagdoll : StickmanState
{
    bool isStanding = false;
    float timeInState = 0.0f;
    
    public StickmanRagdoll(StickmanBase stateMachine) : base(stateMachine)
    {
        Debug.Log("State: Stickman Ragdoll");
        stateMachine.StateName = StickmanBase.StickmanStateName.Ragdoll;
        Init();
    }

    private void Init()
    {
        stateMachine.DisableSprings();
        stateMachine.GetComponent<Ragdoll>().enabled = true;
    }

    public override void Execute()
    {
        timeInState += Time.deltaTime;
        // CHIFFRE MAGIQUE
        if (stateMachine.MainBody.velocity.magnitude < stateMachine.MaxRespawnVelocity 
            && !isStanding 
            && timeInState >= stateMachine.RequiredTimeInRagdollState)
        {
            isStanding = true;
            EventManager.TriggerEvent("StandUp", new Dictionary<string, object>());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ExitState(new StickmanMain(stateMachine));
        }
    }

    public override void ExitState(StickmanState exitTo)
    {
        stateMachine.EnableSprings();
        stateMachine.State = exitTo;
    }

    public override void FixedExecute()
    {
        
    }

    public override string GetName()
    {
        return "StickmanRagdoll";
    }
}
