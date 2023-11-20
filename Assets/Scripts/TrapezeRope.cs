using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapezeRope : MonoBehaviour
{
    [SerializeField] private GameObject trapezeBase;
    [SerializeField] private GameObject trapezeGrab;
    [SerializeField] private float zOffset;

    private void FixedUpdate()
    {
        transform.SetPositionAndRotation(.5f * (trapezeBase.transform.position + trapezeGrab.transform.position) + new Vector3(0, 0, zOffset), trapezeGrab.transform.rotation);
    }
}
