using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanBase : MonoBehaviour
{
    public enum StickmanStateName
    {
        Main,
        Airborne,
        AirborneToMain,
        Rolling,
        RollingToMain,
        Ragdoll,
        OnPole,
        OnTrapeze,
    }

    #region Variables
    private StickmanState state;
    [SerializeField] private StickmanStateName stateName;
    [SerializeField] private StickmanAnimationManager animationManager;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] bodyparts;
    private Rigidbody[] bodypartBodies;

    [LabelOverride("BP Spring Val")]
    [Tooltip("The value of the spring in every bodyparts' hinge joint")]
    [SerializeField] private float bodypartSpringValue = 1500f;
    [Tooltip("The maximum amount of movement the mouse is allowed to make in one frame")]
    [SerializeField] private float maxMouseXMove = 0.025f; // Max amount of movement you can make in a frame while moving the mouse
                                                           
    [Tooltip("The maximum rigidity of the springs")]
    [SerializeField] private float flipSpeed = 1500.0f;
    [Tooltip("Controls the speed at which the springs get rigid when the player is jumping")]
    [SerializeField] private float springTimeIncrement = 50.0f;
    private float currentSpringForce = 0;

    [SerializeField] private float speed = 500.0f;
    [SerializeField] private float maxSpeed = 20.0f;
    [SerializeField] private float jumpForce = 35.0f;

    [LabelOverride("Jump Dir Interpolation")]
    [Tooltip("When jumping:\n0: All momentum is vertical. 1: All momentum is horizontal")]
    [SerializeField] private AnimationCurve jumpDirectionFromMousePosition;

    private GameObject stickman;
    private Rigidbody mainBody;
    private Rigidbody torsoBody;
    private CapsuleCollider mainCollider;

    [SerializeField] private float hurtFlashingTime;
    [SerializeField] private GameObject impactParticle;

    [SerializeField] private GameObject respawnPrefab;
    [SerializeField] private float respawnTime;
    [SerializeField] private AnimationCurve prefabSizeAccel;
    [Tooltip("The minimum time the Stickman has to stay without moving before standing up")]
    [SerializeField] private float idleBeforeRespawn;
    [Tooltip("The minimum time the Stickman has to stay in this state before calling the Stand Up function")]
    [LabelOverride("Req. time in state")]
    [SerializeField] private float requiredTimeInRagdollState;
    [SerializeField] private float maxRespawnVelocity;

    [SerializeField] private float spinSpeed = 400.0f;


    private sbyte xFlipped = 1;

    public float mousePercent;
    public float axisPercent;
    #endregion

    #region Get & Set
    public StickmanState State { get => state; set => state = value; }
    public Rigidbody MainBody { get => mainBody; set => mainBody = value; }
    public Animator Anim { get => animator; set => animator = value; }
    public GameObject Stickman { get => stickman; set => stickman = value; }
    public StickmanAnimationManager AnimationManager { get => animationManager; set => animationManager = value; }
    public Rigidbody TorsoBody { get => torsoBody; set => torsoBody = value; }
    public CapsuleCollider MainCollider { get => mainCollider; set => mainCollider = value; }
    public GameObject[] Bodyparts { get => bodyparts; set => bodyparts = value; }
    public StickmanStateName StateName { get => stateName; set => stateName = value; }
    public Rigidbody[] BodypartBodies { get => bodypartBodies; set => bodypartBodies = value; }
    public sbyte XFlipped { get => xFlipped; set => xFlipped = value; }
    public float Speed { get => speed; set => speed = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public float FlipSpeed { get => flipSpeed; set => flipSpeed = value; }
    public float CurrentSpringForce { get => currentSpringForce; set => currentSpringForce = value; }
    public GameObject ImpactParticle { get => impactParticle; }
    public AnimationCurve JumpDirectionFromMousePosition { get => jumpDirectionFromMousePosition; }
    public float HurtFlashingTime { get => hurtFlashingTime; }
    public float MaxRespawnVelocity { get => maxRespawnVelocity; }
    public float RequiredTimeInRagdollState { get => requiredTimeInRagdollState; }
    public float SpinSpeed { get => spinSpeed; set => spinSpeed = value; }
    #endregion

    private void Awake()
    {
        //Init
        mousePercent = Input.mousePosition.x / Screen.width;

        stickman = gameObject;
        mainBody = GetComponentInChildren<Rigidbody>();
        mainCollider = GetComponent<CapsuleCollider>();
        torsoBody = bodyparts[0].GetComponent<Rigidbody>();

        bodypartBodies = new Rigidbody[bodyparts.Length];

        for (int i = 0; i < bodyparts.Length; i++)
        {
            bodypartBodies[i] = bodyparts[i].GetComponent<Rigidbody>();
        }

        AssignGlobalEvents();

        state = new StickmanMain(this);
    }

    private void Update()
    {
        axisPercent = Input.GetAxis("Horizontal");
        CalculateMousePercent();
        state?.Execute();
    }

    private void FixedUpdate()
    {
        state?.FixedExecute();
    }

    private void CalculateMousePercent() // Easing des mouvements
    {
        float actualMousePercent = Input.mousePosition.x / Screen.width;

        #region Inverting if necessary
        if (xFlipped != 1) //Inverser le input si le personnage face vers la gauche
        {
            actualMousePercent = 1f - actualMousePercent;
        }
        #endregion

        #region Compute mouse percent with max speed
        float computedMousePercent;
        if (actualMousePercent >= mousePercent + maxMouseXMove)
        {
            computedMousePercent = mousePercent + maxMouseXMove;
        }
        else if (actualMousePercent <= mousePercent - maxMouseXMove)
        {
            computedMousePercent = mousePercent - maxMouseXMove;
        }
        else
        {
            computedMousePercent = actualMousePercent;
        }
        #endregion

        #region Limits
        if (computedMousePercent < 0)
        {
            computedMousePercent = 0;
        }
        else if (computedMousePercent > 1)
        {
            computedMousePercent = 1;
        }
        #endregion

        mousePercent = computedMousePercent;
    }

    public Vector3 GetVelocity()
    {
        return mainBody.velocity;
    }

    public IEnumerator IncreaseSpringForceWithTime() // Pour éviter un snapping trop intense en entrant le jump mode
    {
        float t = 0;
        currentSpringForce = 0;
        while (bodypartSpringValue != currentSpringForce)
        {
            currentSpringForce += springTimeIncrement;
            if (currentSpringForce > bodypartSpringValue)
            {
                currentSpringForce = bodypartSpringValue;
            }
            t += Time.deltaTime;
            yield return null;
        }
    }

    public void DisableSprings()
    {
        foreach (GameObject jointObject in bodyparts)
        {
            if (jointObject.TryGetComponent(out HingeJoint joint))
            {
                joint.useSpring = false;
            }
        }
    }

    public void EnableSprings()
    {
        StartCoroutine(IncreaseSpringForceWithTime());
        foreach (GameObject jointObject in bodyparts)
        {
            if (jointObject.TryGetComponent(out HingeJoint joint))
            {
                joint.useSpring = true;
            }
        }
    }

    private void AssignGlobalEvents()
    {
        EventManager.StartListening("SpawnParticle", OnSpawnParticles);
        EventManager.StartListening("StandUp", StandUp);
    }

    private void OnSpawnParticles(Dictionary<string, object> args)
    {
        ParticlePool.SpawnParticles(ImpactParticle, (Vector3)args["Position"]);
    }

    private void StandUp(Dictionary<string, object> args)
    {
        StartCoroutine(StandUp());
    }

    private IEnumerator StandUp()
    {
        float timer = 0.0f;
        while (timer < idleBeforeRespawn)
        {
            timer += Time.deltaTime;
            yield return null;
            if (mainBody.velocity.magnitude >= maxRespawnVelocity) 
                timer = 0.0f;
        }


        GameObject respawnObj = Instantiate(respawnPrefab);
        Vector3 targetScale = respawnObj.transform.localScale;

        respawnObj.transform.localScale = Vector3.one;
        respawnObj.transform.position = transform.position + Vector3.up * (mainCollider.radius * 2);

        timer = 0.0f;
        while (timer < respawnTime)
        {
            timer += Time.deltaTime;
            respawnObj.transform.localScale = Vector3.Lerp(Vector3.one, targetScale, prefabSizeAccel.Evaluate(timer / respawnTime));
            yield return null;
        }

        timer = 0.0f;

        EventManager.TriggerEvent("PlayerFail", new Dictionary<string, object>());

        state.ExitState(new StickmanMain(this));
        transform.position = respawnObj.transform.position;
        transform.rotation = Quaternion.identity;

        while (timer < (respawnTime / 2))
        {
            timer += Time.deltaTime;
            respawnObj.transform.localScale = Vector3.Lerp(targetScale, Vector3.one, prefabSizeAccel.Evaluate(timer / (respawnTime / 2)));
            yield return null;
        }

        Destroy(respawnObj);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("SpawnParticle", OnSpawnParticles);
        EventManager.StopListening("StandUp", StandUp);
    }
}