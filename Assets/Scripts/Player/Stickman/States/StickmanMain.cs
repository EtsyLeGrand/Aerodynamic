using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanMain : StickmanState
{
    private const float MIN_ANIM_SPEED = .2f;
    private const float IDLE_ANIM_TRESHOLD = .01f; //% du mouvement ou l'animation reste sur Idle

    private bool isJumping = false;

    private bool isRolling = false;

    private CapsuleCollider mainCollider;
    private Rigidbody mainBody;
    private Animator anim;
    public StickmanMain(StickmanBase stateMachine) : base(stateMachine)
    {
        Debug.Log("State: Stickman Main");
        stateMachine.StateName = StickmanBase.StickmanStateName.Main;
        mainBody = stateMachine.MainBody;
        mainCollider = stateMachine.MainCollider;
        anim = stateMachine.Anim;
        stateMachine.AnimationManager.SetJumpEvent(TransitionToJumpState);
        stateMachine.AnimationManager.SetRollEvent(TransitionToRollState);
        Init();
    }

    private void Init()
    {
        anim.enabled = true;

        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                anim.SetBool(param.name, false);
            }
        }

        anim.Play("Idle");
        stateMachine.gameObject.GetComponent<Ragdoll>().enabled = false;
        stateMachine.XFlipped = 1;

        Dictionary<string, object> args = new Dictionary<string, object>
        {
            { "tracked", stateMachine.gameObject }
        };
        EventManager.TriggerEvent("StateMain", args);
    }

    public override void Execute()
    {
        KeyActions();
        Run();
    }

    private void Run()
    {
        if (!isJumping && !isRolling)
        {
            float hAxis = Input.GetAxis("Horizontal");
            mainBody.AddForce(Vector3.right * hAxis * stateMachine.Speed * Time.deltaTime * 60, ForceMode.Force);
            
            // Temp - Tant que le bug de anim speed est pas fixed
            anim.SetBool("Run", hAxis != 0);

            #region Max Speed
            if (mainBody.velocity.x > stateMachine.MaxSpeed)
            {
                mainBody.velocity = new Vector3(stateMachine.MaxSpeed, mainBody.velocity.y);
            }
            else if (mainBody.velocity.x < -stateMachine.MaxSpeed)
            {
                mainBody.velocity = new Vector3(-stateMachine.MaxSpeed, mainBody.velocity.y);
            }
            #endregion

            #region X Flipping
            
            if (mainBody.velocity.x >= 0.2f)
            {
                stateMachine.Stickman.transform.localRotation = Quaternion.Euler(0, 0, 0);
                stateMachine.XFlipped = 1;
            }
            else if ((mainBody.velocity.x <= -0.2f))
            {
                stateMachine.Stickman.transform.localRotation = Quaternion.Euler(0, 180, 0);
                stateMachine.XFlipped = -1;
            }

            #endregion

            #region Animation & Anim speed
            /*
            float speedPercent = Mathf.Abs(mainBody.velocity.x) / maxSpeed;

            if (speedPercent <= IDLE_ANIM_TRESHOLD)
            {
                anim.speed = 1; // Bug ici, le speed devrait être 1 au idle, mais pas dans la transition.
                anim.SetBool("Run", false);
            }
            else if (speedPercent > IDLE_ANIM_TRESHOLD)
            {
                if (speedPercent < MIN_ANIM_SPEED)
                {
                    speedPercent = MIN_ANIM_SPEED;
                }

                anim.speed = speedPercent;
                anim.SetBool("Run", true);
            }
            */
            #endregion
        }
    }

    private void KeyActions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartJump();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StartRoll();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartRagdoll(); // CHEAT
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            // CHEAT
            ParticlePool.SpawnParticles(stateMachine.ImpactParticle, stateMachine.gameObject.transform.position);
        }
    }

    private void StartRagdoll()
    {
        ExitState(new StickmanRagdoll(stateMachine));
    }

    private void StartJump()
    {
        isJumping = true;
        anim.SetBool("Jump", true);
    }

    private void StartRoll()
    {
        isRolling = true;
        anim.SetBool("Roll", true);
    }

    private void TransitionToJumpState() // Premier call dans l'anim jump
    {
        stateMachine.StartCoroutine(stateMachine.IncreaseSpringForceWithTime());
        mainBody.AddForce((Vector3.up * stateMachine.JumpForce)
            + stateMachine.JumpDirectionFromMousePosition.Evaluate(stateMachine.mousePercent) * (stateMachine.Speed / 10 * stateMachine.XFlipped * Vector3.right),
            ForceMode.Impulse);

        ExitState(new StickmanAirborne(stateMachine));
    }

    private void TransitionToRollState()
    {
        ExitState(new StickmanRoll(stateMachine));
    }

    public override void ExitState(StickmanState exitTo)
    {
        stateMachine.State = exitTo;
    }

    public override void FixedExecute()
    {
    }

    public override string GetName()
    {
        return "StickmanMain";
    }
}
