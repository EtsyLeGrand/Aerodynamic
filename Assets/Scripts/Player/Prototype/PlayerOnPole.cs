using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Changer le swing pour y mettre de l'accélération. En temps normal, il faudrait juste pouvoir faire un tour 
    au bout de 4 swings. À faire plus tard.
 */

public class PlayerOnPole : PlayerState
{
    private GameObject grabbableObject;
    private Rigidbody playerBody;
    private Rigidbody handBody;
    private HingeJoint hingeJoint;
    private FixedJoint fixedJoint;
    private GameObject hand;
    private Vector3 handDefaultPosition;
    private float initialSwingForce = 3.0f;
    
    public PlayerOnPole(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        Debug.Log("State set to PlayerGrabbed");
        grabbableObject = stateMachine.GrabbableObject;
        playerBody = stateMachine.PlayerBody;
        hand = stateMachine.Hand;
        Init();
    }

    private void Init()
    {
        fixedJoint = hand.AddComponent<FixedJoint>(); // Mettre le joint sur le parent le plus haut
        fixedJoint.connectedBody = playerBody;

        handDefaultPosition = hand.transform.localPosition;
        hand.transform.position = grabbableObject.transform.position;
        
        handBody = hand.GetComponent<Rigidbody>();
        handBody.useGravity = false;
        handBody.constraints = RigidbodyConstraints.FreezePositionZ;

        hingeJoint = hand.gameObject.AddComponent<HingeJoint>(); // Mettre le joint sur la main (ou le point de pivot)
        hingeJoint.connectedBody = grabbableObject.GetComponent<Rigidbody>();
        hingeJoint.axis = Vector3.forward;
        hingeJoint.anchor = Vector3.zero;

        //playerBody.GetComponent<CustomGravity>().gravityScale = 10f;
    }

    public override void Execute()
    {
        float swingForce = (60 * Time.deltaTime) * ((1 * playerBody.velocity.magnitude) + initialSwingForce);
        if (Input.GetKey("a"))
        {
            playerBody.AddForce(-swingForce * playerBody.transform.right, ForceMode.Acceleration);
        }
        if (Input.GetKey("d"))
        {
            playerBody.AddForce(swingForce * playerBody.transform.right, ForceMode.Acceleration);
        }
        if (!Input.GetMouseButton(0))
        {
            ExitState(new PlayerAirborne(stateMachine));
        }
    }

    public override void FixedExecute()
    {

    }

    public override void ExitState(PlayerState exitTo)
    {
        Debug.Log("Released bar");
        Object.Destroy(hingeJoint);
        Object.Destroy(fixedJoint);
        Object.Destroy(handBody);

        hand.transform.localPosition = handDefaultPosition;
        hand.GetComponent<PlayerHand>().IsStateLocked = false;

        playerBody.GetComponent<CustomGravity>().gravityScale = 1f;

        stateMachine.CurrentState = exitTo;
    }

    public override string GetName()
    {
        return "OnPole";
    }
}
