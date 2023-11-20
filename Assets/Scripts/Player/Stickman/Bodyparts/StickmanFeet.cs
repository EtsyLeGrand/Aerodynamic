using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanFeet : MonoBehaviour
{
    private StickmanBase stateMachine;
    [SerializeField] private bool isInverted;
    private void Awake()
    {
        stateMachine = transform.root.GetComponent<StickmanBase>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Floor") && 
            stateMachine.StateName == StickmanBase.StickmanStateName.Airborne && 
            stateMachine.GetVelocity().y <= 0f &&
            (stateMachine.transform.rotation.eulerAngles.z <= 45f || stateMachine.transform.rotation.eulerAngles.z >= 315f))
        {
            Debug.Log("STAND");
            stateMachine.State.ExitState(new StickmanAirborneToMain(stateMachine));
        }
    }
}
