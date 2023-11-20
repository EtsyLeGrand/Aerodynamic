using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool onGround;

    public bool OnGround { get => onGround; }

    private void Awake()
    {
        onGround = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            spriteRenderer.color = Color.magenta;
            onGround = true;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            spriteRenderer.color = Color.white;
            onGround = false;
        }
    }
}
