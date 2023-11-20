using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

// À UTILISER POUR PLIER LE PERSONNAGE PENDANT LE ROLL

public class TorsoSpline : MonoBehaviour
{
    SpriteShapeController shape;
    [SerializeField] int splineLeftIndex = 1;
    int splineRightIndex = 5;
    private Vector3 defaultLeftPosition = new Vector3(-1.5f, 0);
    private Vector3 defaultRightPosition = new Vector3(1.5f, 0);

    private Vector3 endLeftPosition = new Vector3(-6f, 0);
    private Vector3 endRightPosition = new Vector3(-3f, 0);

    public Vector3 splineVal;
    public float tangentVal;
    //[SerializeField] int splineRightIndex;
    private void Awake()
    {
        shape = GetComponent<SpriteShapeController>();
        defaultLeftPosition = shape.spline.GetPosition(splineLeftIndex);
        defaultRightPosition = shape.spline.GetPosition(splineRightIndex);
        //Debug.Log(defaultLeftPosition + " " + defaultRightPosition);
    }
    void Start()
    {
        shape.spline.SetPosition(splineLeftIndex, endLeftPosition);
        shape.spline.SetLeftTangent(splineLeftIndex, 4 * Vector3.down);
        shape.spline.SetRightTangent(splineLeftIndex, 4 * Vector3.up);
        shape.spline.SetTangentMode(splineLeftIndex, ShapeTangentMode.Continuous);

        shape.spline.SetPosition(splineRightIndex, endRightPosition);
        shape.spline.SetLeftTangent(splineRightIndex, -4 * Vector3.down);
        shape.spline.SetRightTangent(splineRightIndex, -4 * Vector3.up);
        shape.spline.SetTangentMode(splineRightIndex, ShapeTangentMode.Continuous);
        //shape.spline.SetRightTangent(1, 2 * Vector3.one);
    }

    // Update is called once per frame
    void Update()
    {
        //shape.spline.SetPosition()
    }
}
