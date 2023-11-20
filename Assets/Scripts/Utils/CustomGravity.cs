using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;

    Rigidbody body;

    void OnEnable()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        body.AddForce(gravity, ForceMode.Acceleration);
    }
}