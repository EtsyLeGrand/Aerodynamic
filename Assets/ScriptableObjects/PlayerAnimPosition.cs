using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class PlayerAnimPosition : ScriptableObject
{
    [System.Serializable]
    public struct PositionRotation
    {
        public Vector3 position;
        public float rotation;
    }
    public PositionRotation jumpTransform;
}
