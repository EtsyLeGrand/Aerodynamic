using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    
    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract string GetName();
    public abstract void Execute();

    public abstract void FixedExecute();

    public abstract void ExitState(PlayerState exitTo);
}