using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanOnPole : StickmanState
{
    HingeJoint hingeJoint;
    GameObject grabbedObject;

    private Vector3 handDefaultPosition;

    public StickmanOnPole(StickmanBase stateMachine, GameObject grabbedObject, GameObject mainHand) : base(stateMachine)
    {
        Debug.Log("State: Stickman OnPole");
        stateMachine.StateName = StickmanBase.StickmanStateName.OnPole;
        this.grabbedObject = grabbedObject;
        Init();
    }

    private void Init()
    {
        Vector3 dist = 
            grabbedObject.transform.position 
            - stateMachine.Bodyparts[0].transform.position 
            - (3 * stateMachine.Bodyparts[0].transform.up);
        stateMachine.transform.position += dist;

        hingeJoint = stateMachine.gameObject.AddComponent<HingeJoint>();
        hingeJoint.enablePreprocessing = false;
        hingeJoint.connectedBody = grabbedObject.GetComponent<Rigidbody>();
        hingeJoint.axis = Vector3.forward;

        hingeJoint.anchor = stateMachine.transform.InverseTransformPoint(grabbedObject.transform.position);

        Dictionary<string, object> args = new Dictionary<string, object>
        {
            { "tracked", grabbedObject }
        };
        EventManager.TriggerEvent("StateOnPole", args);
    }

    public override void Execute()
    {
        if (!Input.GetMouseButton(0))
        {
            ExitState(new StickmanAirborne(stateMachine));
        }
    }

    public override void ExitState(StickmanState exitTo)
    {
        AudioManager.PlaySound(3);
        Object.Destroy(hingeJoint);
        stateMachine.State = exitTo;
    }

    public override void FixedExecute()
    {
        Vector3 torque = -(.5f - stateMachine.mousePercent) * stateMachine.SpinSpeed * Time.deltaTime * 60 * stateMachine.XFlipped * Vector3.forward;
        stateMachine.MainBody.AddTorque(torque);
    }

    public override string GetName()
    {
        return "StickmanOnPole";
    }
}
