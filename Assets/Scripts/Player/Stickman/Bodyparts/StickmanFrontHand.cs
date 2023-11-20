using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanFrontHand : MonoBehaviour // Main hand
{
    private GameObject itemInRange = null;
    [SerializeField] private StickmanBackHand backHand;
    StickmanBase stateMachine;

    private void Awake()
    {
        stateMachine = transform.root.GetComponent<StickmanBase>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Pole")
            || c.gameObject.layer == LayerMask.NameToLayer("Trapeze"))
        {
            itemInRange = c.gameObject;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Pole")
            || c.gameObject.layer == LayerMask.NameToLayer("Trapeze"))
        {
            itemInRange = null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && (itemInRange != null || backHand.ItemInRange != null)
            && stateMachine.StateName == StickmanBase.StickmanStateName.Airborne)
        {
            GameObject sentItem;
            if (itemInRange == null) sentItem = backHand.ItemInRange;
            else sentItem = itemInRange;

            AudioManager.PlaySound(2);

            if (sentItem.layer == LayerMask.NameToLayer("Pole"))
            {
                stateMachine.State.ExitState(new StickmanOnPole(stateMachine, sentItem, gameObject));
            }
            else
            {
                stateMachine.State.ExitState(new StickmanOnTrapeze(stateMachine, sentItem, gameObject));
            }
        }
    }
}
