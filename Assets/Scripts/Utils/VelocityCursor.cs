using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCursor : MonoBehaviour
{
    [SerializeField] private Rigidbody torsoBody;

    [SerializeField] private float redMax = 25f;

    [SerializeField] private float clamp = 40.0f;

    [SerializeField] private float timeBeforeUpdate = 0.1f;
    private float timer;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBeforeUpdate)
        {
            timer = 0;
            UpdateCursorPosition();
        }
    }

    void UpdateCursorPosition()
    {
        Vector3 vel = torsoBody.velocity;
        float xPos, yPos;

        xPos = vel.x * clamp / redMax;
        yPos = vel.y * clamp / redMax;

        float hyp = Vector3.Distance(Vector3.zero, new Vector3(xPos, yPos));

        rect.localPosition = Vector3.ClampMagnitude(new Vector3(
            Mathf.Clamp(xPos, -clamp, clamp),
            Mathf.Clamp(yPos, -clamp, clamp)),
            clamp);

    }

}
