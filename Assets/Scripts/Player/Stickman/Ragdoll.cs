using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script à placer sur le root GameObject
public class Ragdoll : MonoBehaviour
{
    [SerializeField] private RagdollPart[] bodyparts;
    [SerializeField] private Animator anim;
    private Rigidbody mainBody;
    private CapsuleCollider rootCollider;
    private FixedJoint joint;

    private Vector3 reappliedVelocity;

    //Temporaire - sert juste pour MovementTest, tester le ragdoll de parts
    public RagdollPart[] Bodyparts { get => bodyparts; set => bodyparts = value; }

    private void Awake() //Sera called même si le component est disabled
    {
        rootCollider = GetComponent<CapsuleCollider>();
        mainBody = GetComponent<Rigidbody>();
        DisableAllBodypartsRagdoll();
    }

    private void OnEnable()
    {
        Debug.Log("Ragdoll mode enabled");
        EnableAllBodypartsRagdoll(); // Kinematic
        
        DisableAnimator();
        rootCollider.enabled = false;
        joint = rootCollider.gameObject.AddComponent<FixedJoint>();
        joint.enablePreprocessing = false;
        joint.connectedBody = bodyparts[0].Body;
        if ((mainBody.constraints & RigidbodyConstraints.FreezeRotationZ) == RigidbodyConstraints.FreezeRotationZ) // Check if set
        {
            mainBody.constraints ^= RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnDisable()
    {
        Debug.Log("Ragdoll disabled");
        Destroy(joint);
        rootCollider.enabled = true;
        EnableAnimator();
        DisableAllBodypartsRagdoll();
        mainBody.constraints ^= RigidbodyConstraints.FreezeRotationZ;
    }

    #region Kinematic
    private void EnableAllBodypartsRagdoll()
    {
        foreach (RagdollPart bodypart in bodyparts)
        {
            bodypart.enabled = true;
        }
    }

    private void DisableAllBodypartsRagdoll()
    {
        foreach (RagdollPart bodypart in bodyparts)
        {
            bodypart.enabled = false;
        }
    }
    #endregion

    #region Animator Setup
    private void EnableAnimator()
    {
        anim.enabled = true;
    }

    private void DisableAnimator()
    {
        anim.enabled = false;
    }

    #endregion
}