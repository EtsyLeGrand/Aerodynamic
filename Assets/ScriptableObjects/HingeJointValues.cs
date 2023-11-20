using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
[System.Serializable]
public class HingeJointValues : ScriptableObject
{
    public enum BodypartType
    {
        Head,
        Shoulder,
        Thigh,
        Wrist,
        Calf
    }
    public BodypartType type;
    public Vector3 anchor;
    public float minLimit;
    public float maxLimit;
}
