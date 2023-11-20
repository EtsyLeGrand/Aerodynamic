using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : PlayerState
{
    public PlayerRoll(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        Debug.Log("State set to PlayerRoll");
        Init();
    }

    private void Init()
    {
        stateMachine.Feet.SetActive(false);
        stateMachine.Hand.SetActive(false);
        stateMachine.Head.SetActive(false);

        stateMachine.GetComponent<CustomGravity>().gravityScale = 4;
        stateMachine.Body.transform.localScale = Vector3.one;
        stateMachine.Body.GetComponent<SphereCollider>().enabled = true;
        stateMachine.Body.GetComponent<BoxCollider>().enabled = false;

        stateMachine.Anim.SetBool("Roll", true);
    }

    public override void Execute()
    {
        if (stateMachine.RollSpeed != 0.0f)
        {
            stateMachine.PlayerBody.velocity = stateMachine.PlayerBody.velocity.normalized * stateMachine.RollSpeed;
        }

        if (!Input.GetKey("s"))
        {
            ExitState(new PlayerMain(stateMachine));
        }
    }

    public override void FixedExecute()
    {

    }

    public override void ExitState(PlayerState exitTo)
    {
        /*
         * Known Bug: Quand le joueur se relève, il passe à travers le sol. Le bug sera certainement réglé lors
         * de mon implémentation des animations
        */

        stateMachine.Feet.SetActive(true);
        stateMachine.Hand.SetActive(true);
        stateMachine.Head.SetActive(true);

        stateMachine.GetComponent<CustomGravity>().gravityScale = 1;
        stateMachine.Body.transform.localScale = new Vector3(1, 2, 1);
        stateMachine.Body.GetComponent<SphereCollider>().enabled = false;
        stateMachine.Body.GetComponent<BoxCollider>().enabled = true;

        stateMachine.Anim.SetBool("Roll", false);

        stateMachine.CurrentState = exitTo;
    }

    public override string GetName()
    {
        return "Roll";
    }
}
