using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private PlayerStateMachine sm;
    private Rigidbody rb;
    private bool isStateLocked = false;

    public bool IsStateLocked { set => isStateLocked = value; }

    private void Awake()
    {
        GameObject rootChar = transform.root.gameObject;
        sm = rootChar.GetComponent<PlayerStateMachine>();
        rb = rootChar.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Pole") && Input.GetMouseButton(0) && !isStateLocked)
        {
            IsStateLocked = true; // POSER QUESTION CAR NON OPTIMAL, MAIS SANS ÇA LE TRIGGER EST CALLED 2 FOIS
            sm.GrabbableObject = c.gameObject;
            GameObject grabbableObject = c.gameObject;
            Debug.Log("Grabbed " + grabbableObject.name);
            GetComponent<SpriteRenderer>().color = Color.green;
            sm.CurrentState = new PlayerOnPole(sm.GetComponent<PlayerStateMachine>());
        }
    }
}
