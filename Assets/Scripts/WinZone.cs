using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Stickman"))
        {
            EventManager.TriggerEvent("PlayerWin", new Dictionary<string, object>());
        }
    }
}
