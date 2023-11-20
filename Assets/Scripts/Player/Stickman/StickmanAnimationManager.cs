using System;
using UnityEngine;

public class StickmanAnimationManager : MonoBehaviour
{
    private Action onEnterJumpState;
    private Action onEnterRollState;
    private Ragdoll ragdoll;
    private CapsuleCollider mainCollider;
    private bool colliderEditMode = false;

    private void Awake()
    {
        ragdoll = transform.parent.GetComponent<Ragdoll>();
        mainCollider = transform.parent.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (colliderEditMode)
        {
            Debug.Log("Updated height");
        }
    }

    public void EmptyEvent()
    {
        Debug.Log("Empty event called");
    }

    public void JumpEvent()
    {
        onEnterJumpState?.Invoke();
    }

    public void SetJumpEvent(Action a)
    {
        onEnterJumpState = a;
    }

    public void RollEvent()
    {
        onEnterRollState?.Invoke();
    }

    public void SetRollEvent(Action a)
    {
        onEnterRollState = a;
    }

    public void OnJumpAnimationEnd() // Dernier call dans l'anim jump
    {
        ragdoll.enabled = true;
    }

    public void EditMainColliderHeight(float height)
    {
        mainCollider.height = height;
    }

    public void EditMainColliderRadius(float radius)
    {
        mainCollider.radius = radius;
    }

    public void Step()
    {
        AudioManager.PlaySound(0);
    }
}