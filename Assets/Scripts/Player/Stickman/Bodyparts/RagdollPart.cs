using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RagdollPart : MonoBehaviour
{
    private StickmanBase stateMachine;
    private Rigidbody body;
    private HingeJoint joint;

    private SpriteRenderer partRenderer;
    private Color partCurrentColor;


    [Tooltip("Positions to aim for when Stickman is flipping. [flip back, flip neutral, flip front]")]
    [SerializeField] private SpringTargetPositions jumpFlipSpringPositions;
    [SerializeField] private SpringTargetPositions poleSpringPositions;
    public Rigidbody Body { get => body; set => body = value; }

    void Awake()
    {
        stateMachine = transform.root.GetComponent<StickmanBase>();
        body = GetComponent<Rigidbody>();
        if (!TryGetComponent(out joint) && !gameObject.name.Contains("torso"))
        {
            Debug.LogWarning(gameObject.name + " doesn't have a joint.");
        }
    }

    private void OnEnable()
    {
        body.isKinematic = false;
    }

    private void OnDisable()
    {
        body.isKinematic = true;
    }

    private void Update()
    {
        // Logique des states d'animation avec le ScriptableObject de Spring Positions
        if (stateMachine.StateName == StickmanBase.StickmanStateName.Airborne && joint != null)
        {
            int arrLen = jumpFlipSpringPositions.AirbornePositions.Count;
            float percentInArr = stateMachine.mousePercent * (arrLen - 1);

            int startIndex, endIndex;

            startIndex = Mathf.FloorToInt(percentInArr);
            endIndex = Mathf.CeilToInt(percentInArr);

            float lerpPos = percentInArr - startIndex;

            float value = Mathf.Lerp(jumpFlipSpringPositions.AirbornePositions[startIndex], jumpFlipSpringPositions.AirbornePositions[endIndex], lerpPos);

            joint.spring = new JointSpring
            {
                spring = stateMachine.CurrentSpringForce,
                targetPosition = value
            };
        }

        else if ((stateMachine.StateName == StickmanBase.StickmanStateName.OnPole || stateMachine.StateName == StickmanBase.StickmanStateName.OnTrapeze) && joint != null)
        {
            int arrLen = poleSpringPositions.AirbornePositions.Count;
            float percentInArr = stateMachine.mousePercent * (arrLen - 1);

            int startIndex, endIndex;

            startIndex = Mathf.FloorToInt(percentInArr);
            endIndex = Mathf.CeilToInt(percentInArr);

            float lerpPos = percentInArr - startIndex;

            float value = Mathf.Lerp(poleSpringPositions.AirbornePositions[startIndex], poleSpringPositions.AirbornePositions[endIndex], lerpPos);

            joint.spring = new JointSpring
            {
                spring = stateMachine.CurrentSpringForce,
                targetPosition = value
            };
        }
    }
}
