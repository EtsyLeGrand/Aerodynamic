using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAirborne : StickmanState
{
    private readonly Rigidbody mainBody;

    public StickmanAirborne(StickmanBase stateMachine) : base(stateMachine)
    {
        Debug.Log("State: Stickman Airborne");
        stateMachine.StateName = StickmanBase.StickmanStateName.Airborne;
        mainBody = stateMachine.MainBody;
        Init();
    }

    private void Init()
    {
        AudioManager.PlaySound(4);
        stateMachine.MainCollider.enabled = false;

        Dictionary<string, object> args = new Dictionary<string, object>
        {
            { "tracked", stateMachine.gameObject }
        };
        EventManager.TriggerEvent("StateAirborne", args);
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ExitState(new StickmanRagdoll(stateMachine));
        }
    }

    public override void ExitState(StickmanState exitTo)
    {
        AudioManager.StopSound(4);
        stateMachine.State = exitTo;
    }

    public override void FixedExecute()
    {
        Vector3 torque = (.5f - stateMachine.mousePercent) * stateMachine.FlipSpeed * Time.deltaTime * 60 * stateMachine.XFlipped * Vector3.forward;

        mainBody.AddTorque(torque);
    }

    public override string GetName()
    {
        return "StickmanAirborne";
    }


}
