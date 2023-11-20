using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] Vector3 baseOffset;
    [SerializeField] Vector3 offsetOnPole;
    [SerializeField] Vector3 offsetOnTrapeze;

    [SerializeField] float damping;
    [SerializeField] AnimationCurve dampingCurve;

    private CinemachineCameraOffset cameraOffset;

    private Coroutine dampCoroutine;

    private void Awake()
    {
        EventManager.StartListening("StateMain", OnStateMain);
        EventManager.StartListening("StateOnPole", OnStateOnPole);
        EventManager.StartListening("StateOnTrapeze", OnStateOnTrapeze);
        EventManager.StartListening("StateAirborne", OnStateAirborne);
    }

    private void Start()
    {
        cameraOffset = GetComponent<CinemachineCameraOffset>();
    }

    private void OnStateMain(Dictionary<string, object> args)
    {
        if (args.TryGetValue("tracked", out object target))
        {
            Debug.Log("Stickman as Target!");
            if (cameraOffset == null)
            {
                cameraOffset = GetComponent<CinemachineCameraOffset>();
            }
            cameraOffset.VirtualCamera.Follow = ((GameObject)target).transform;

        }

        if (dampCoroutine != null)
        {
            StopCoroutine(dampCoroutine);
        }

        if (cameraOffset.m_Offset != baseOffset)
        {
            dampCoroutine = StartCoroutine(Damp(baseOffset));
        }
    }

    private void OnStateOnPole(Dictionary<string, object> args)
    {
        if (args.TryGetValue("tracked", out object target))
        {
            Debug.Log("Pole as Target!");
            cameraOffset.VirtualCamera.Follow = ((GameObject)target).transform;
        }

        if (dampCoroutine != null) 
        {
            StopCoroutine(dampCoroutine);
        }

        if (cameraOffset.m_Offset != offsetOnPole)
        {
            dampCoroutine = StartCoroutine(Damp(offsetOnPole));
        }
    }

    private void OnStateOnTrapeze(Dictionary<string, object> args)
    {
        if (args.TryGetValue("tracked", out object target))
        {
            Debug.Log("Trapeze as Target!");
            cameraOffset.VirtualCamera.Follow = ((GameObject)target).transform;
        }

        if (dampCoroutine != null)
        {
            StopCoroutine(dampCoroutine);
        }

        if (cameraOffset.m_Offset != offsetOnTrapeze)
        { 
            dampCoroutine = StartCoroutine(Damp(offsetOnTrapeze));
        }
    }

    private void OnStateAirborne(Dictionary<string, object> args)
    {
        OnStateMain(args);
    }

    private IEnumerator Damp(Vector3 dampPos)
    {
        Debug.Log("Routine go!");
        float dampIncrement = 1 / damping;
        float dampProgress = 0;
        Vector3 startPos = cameraOffset.m_Offset;
        Vector3 deltaOffset = dampPos - startPos;

        while (dampProgress != 1)
        {
            dampProgress += dampIncrement;

            if (dampProgress > 1) dampProgress = 1;

            cameraOffset.m_Offset = startPos + (deltaOffset * dampingCurve.Evaluate(dampProgress));
            yield return null;
        }
        Debug.Log("Routine stopped!");
    }

    private void OnDestroy()
    {
        EventManager.StopListening("StateMain", OnStateMain);
        EventManager.StopListening("StateOnPole", OnStateOnPole);
        EventManager.StopListening("StateOnTrapeze", OnStateOnTrapeze);
        EventManager.StopListening("StateAirborne", OnStateAirborne);
    }
}
