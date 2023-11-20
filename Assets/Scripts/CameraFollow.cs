using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float yOffset = 3.0f;
    [SerializeField] float zOffset = 3.0f;
    [SerializeField] GameObject focus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(focus.transform.position.x, focus.transform.position.y + yOffset, zOffset);
    }
}
