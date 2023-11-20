using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollision : MonoBehaviour
{
    // RGB Mid - 27 62 190
    // RGB Pâle - 37 75 213
    // RGB Foncé 43 43 162

    private new SpriteRenderer renderer;
    private Color baseColor;
    private Color hurtTargetColor = Color.red;
    [SerializeField] private float hitVelocityTolerance;
    private float hitVelocityToleranceDefault;
    private float flashingTime;

    private bool isFlashing = false;

    StickmanBase stateMachine;

    public float HitVelocityTolerance { get => hitVelocityTolerance; set => hitVelocityTolerance = value; }
    public float HitVelocityToleranceDefault { get => hitVelocityToleranceDefault; set => hitVelocityToleranceDefault = value; }

    private void Awake()
    {
        stateMachine = transform.root.GetComponent<StickmanBase>();
        renderer = GetComponent<SpriteRenderer>();
        flashingTime = stateMachine.HurtFlashingTime;
        baseColor = renderer.color;

        hitVelocityToleranceDefault = hitVelocityTolerance;
    }

    // Requis pour le enabled
    private void Start() { }

    private void OnCollisionEnter(Collision c)
    {
        if (!enabled) return;
        if (c.relativeVelocity.magnitude > hitVelocityTolerance)
        {
            Debug.Log(gameObject.name + " hit for: " + c.relativeVelocity.magnitude);

            if (renderer.color == baseColor)
            {
                EventManager.TriggerEvent("SpawnParticle",
                new Dictionary<string, object> {
                    { "Position", c.GetContact(0).point } });
            }
            
            if (isFlashing)
            {
                StopCoroutine(FlashColor());
                renderer.color = baseColor;
            }

            StartCoroutine(FlashColor());

            if (stateMachine.StateName != StickmanBase.StickmanStateName.Ragdoll)
            {
                stateMachine.State.ExitState(new StickmanRagdoll(stateMachine));
            }
        }
    }

    private IEnumerator FlashColor()
    {
        isFlashing = true;
        AudioManager.PlaySound(1);
        renderer.color = hurtTargetColor;
        float t = 0;

        while (t < flashingTime)
        {
            t += Time.deltaTime;
            renderer.color = Color.Lerp(hurtTargetColor, baseColor, t / flashingTime);
            yield return null;
        }

        isFlashing = false;
    }
}
