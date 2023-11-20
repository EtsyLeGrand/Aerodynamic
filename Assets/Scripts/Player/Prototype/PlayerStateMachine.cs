using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject feet;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    [Header("Misc")]
    [LabelOverride("Grace time")]
    [SerializeField] private float invincibilityTimeWhenHurt = 1.0f;
    [SerializeField] private float torque = 1.0f;

    private PlayerState currentState;
    private Rigidbody playerBody;
    private SpriteRenderer handSpriteRenderer;
    private Animator anim;
    private GameObject grabbableObject;
    private Transform handInitialTransform;
    private float rollSpeed;
    private Coroutine sendVelocityToUI;
    private float currentInvincibilityTime = 0.0f;
    private bool isInvincible = false;
    

    public Action<float> onSendVelocityToUI;
    public Action onHurt;

    #region Get & Set
    public GameObject Hand { get => hand; set => hand = value; }
    public GameObject Feet { get => feet; set => feet = value; }
    public PlayerState CurrentState { get => currentState; set => currentState = value; }
    public Rigidbody PlayerBody { get => playerBody; set => playerBody = value; }
    public SpriteRenderer HandSpriteRenderer { get => handSpriteRenderer; set => handSpriteRenderer = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public GameObject GrabbableObject { get => grabbableObject; set => grabbableObject = value; }
    public Transform HandInitialTransform { get => handInitialTransform; }
    public GameObject Body { get => body; set => body = value; }
    public GameObject Head { get => head; set => head = value; }
    public float RollSpeed { get => rollSpeed; set => rollSpeed = value; }
    public float Torque { get => torque; set => torque = value; }
    #endregion

    private void Awake()
    {
        PlayerBody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        HandSpriteRenderer = Hand.GetComponent<SpriteRenderer>();

        handInitialTransform = hand.transform;
        playerBody.centerOfMass = Vector3.zero;
    }

    void Start()
    {
        CurrentState = new PlayerMain(this);
        sendVelocityToUI = StartCoroutine(SendVelocityToUI());
    }
    
    void Update()
    {
        CurrentState.Execute();
    }

    private void FixedUpdate()
    {
        CurrentState.FixedExecute();
    }

    private void OnCollisionEnter(Collision c)
    {
        //Rolling Logic
        if (c.gameObject.layer == LayerMask.NameToLayer("Ramp") && currentState.GetName() == "Roll")
        {
            Debug.Log("Speed set");
            rollSpeed = playerBody.velocity.magnitude;

            /*
             * Peut causer problème si le joueur rencontre une autre pente. Il serait peut-être
             * mieux de changer le physics material du joueur quand il roule pour lui mettre moins
             * de friction. Par contre, le boost dans une rampe est nécessaire.
            */

            StopCoroutine(DeleteSpeedWhenRollingStops());
        }

        if (c.relativeVelocity.magnitude >= 10.5f && currentState.GetName() != "Roll" && !isInvincible) // À mettre en Singleton (ou similaire)
        {
            isInvincible = true;
            anim.Play("HurtColor", 1);
            onHurt.Invoke(); // Parfois appelé 2 fois
            StartCoroutine(StartInvincibilityTimer());
        }
    }

    private void OnCollisionExit(Collision c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Ramp"))
        {
            /*
             * Cette coroutine peut être appelée plusieurs fois. Pour éviter ça, il
             * faudrait créer un autre collider (trigger) circulaire autour du joueur
             * pour ne pas reset la coroutine si elle est en contact avec cette hitbox.
             *                               TODO
             */
            StartCoroutine(DeleteSpeedWhenRollingStops());
        }
    }

    private IEnumerator DeleteSpeedWhenRollingStops()
    {
        yield return new WaitUntil(() => currentState.GetName() != "Roll");
        Debug.Log("Speed deleted");
        rollSpeed = 0.0f;
    }

    private IEnumerator SendVelocityToUI()
    {
        float displayTime = 0.05f; //For debugging purposes
        while (true)
        {
            onSendVelocityToUI.Invoke(playerBody.velocity.magnitude);
            yield return new WaitForSeconds(displayTime);
        }
    }

    private IEnumerator StartInvincibilityTimer()
    {
        isInvincible = true;
        while (currentInvincibilityTime < invincibilityTimeWhenHurt)
        {
            currentInvincibilityTime += Time.deltaTime;
            yield return null;
        }
        currentInvincibilityTime = 0.0f;
        isInvincible = false;
    }

    public PlayerState GetPlayerStateByName(string name)
    {
        switch (name)
        {
            case "Main": return new PlayerMain(this);
            case "Airborne": return new PlayerAirborne(this);
            case "Roll": return new PlayerRoll(this);
            case "OnPole": return new PlayerOnPole(this);
            default: Debug.Log("Error retrieving state"); break;
        }
        return new PlayerMain(this);
    }
}
