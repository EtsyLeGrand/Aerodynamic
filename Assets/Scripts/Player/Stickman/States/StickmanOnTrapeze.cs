using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanOnTrapeze : StickmanState
{
    FixedJoint fixedJoint;
    GameObject grabbedObject;

    Rigidbody grabbedBody;

    public StickmanOnTrapeze(StickmanBase stateMachine, GameObject grabbedObject, GameObject mainHand) : base(stateMachine)
    {
        Debug.Log("State: Stickman OnTrapeze");
        stateMachine.StateName = StickmanBase.StickmanStateName.OnTrapeze;
        this.grabbedObject = grabbedObject;
        Init();
    }

    private void Init()
    {
        grabbedBody = grabbedObject.GetComponent<Rigidbody>();
        Vector3 rot = stateMachine.gameObject.transform.rotation.eulerAngles;

        stateMachine.gameObject.transform.rotation =
           Quaternion.Euler(new Vector3(rot.x, rot.y, grabbedObject.transform.rotation.y));

        Vector3 dist = 
            grabbedObject.transform.position 
            - stateMachine.Bodyparts[0].transform.position 
            - (3 * stateMachine.Bodyparts[0].transform.up);
        stateMachine.transform.position += dist;

        fixedJoint = stateMachine.gameObject.AddComponent<FixedJoint>();
        fixedJoint.enablePreprocessing = false;
        fixedJoint.connectedBody = grabbedObject.GetComponent<Rigidbody>();
        fixedJoint.axis = Vector3.forward;

        fixedJoint.anchor = stateMachine.transform.InverseTransformPoint(grabbedObject.transform.position);

        Dictionary<string, object> args = new Dictionary<string, object>
        {
            { "tracked", grabbedObject.transform.root.gameObject }
        };
        EventManager.TriggerEvent("StateOnTrapeze", args);
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
        Object.Destroy(fixedJoint);
        stateMachine.State = exitTo;
    }

    public override void FixedExecute()
    {
        float force = -(.5f - stateMachine.mousePercent) * stateMachine.SpinSpeed * Time.deltaTime * 60;
        Debug.Log(force);
        grabbedBody.AddForce(stateMachine.transform.right * force, ForceMode.Force);
    }

    public override string GetName()
    {
        return "StickmanOnPole";
    }
}
