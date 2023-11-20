using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollTorso : MonoBehaviour
{
    private Ragdoll mainRagdoll;
    void Awake()
    {
        mainRagdoll = transform.parent.GetComponent<Ragdoll>();
    }

    public void SetRagdoll()
    {
        mainRagdoll.enabled = true;
    }

    public void UnsetRagdoll()
    {
        mainRagdoll.enabled = false;
    }
}
