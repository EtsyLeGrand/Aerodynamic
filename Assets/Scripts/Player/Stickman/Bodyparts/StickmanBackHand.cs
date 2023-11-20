using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanBackHand : MonoBehaviour
{
    private GameObject itemInRange = null;

    public GameObject ItemInRange { get => itemInRange; }

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
}
