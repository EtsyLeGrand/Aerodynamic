using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private const float TEXT_SIZE_Y = 120f;
    private const int FONT_SIZE = 80;

    [SerializeField] private KeyCode debugtoggle;
    [SerializeField] private StickmanBase debugTarget;

    private readonly List<DebugCommand> commands = new List<DebugCommand>();
    private bool isConsoleOpened = false;
    private string input;

    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(gameObject);
#endif

        DebugCommand killCamera = new DebugCommand("KILLCAMERA", () =>
        {
            Destroy(Camera.main.gameObject);
        });

        DebugCommand stateMain = new DebugCommand("SETSTATE MAIN", () =>
        {
            debugTarget.State.ExitState(new StickmanMain(debugTarget));
        });

        DebugCommand stateAirborne = new DebugCommand("SETSTATE AIR", () =>
        {
            debugTarget.StartCoroutine(debugTarget.IncreaseSpringForceWithTime());
            debugTarget.transform.root.GetComponent<Ragdoll>().enabled = true;
            debugTarget.State.ExitState(new StickmanAirborne(debugTarget));
        });

        DebugCommand stateRagdoll = new DebugCommand("SETSTATE RAGDOLL", () =>
        {
            debugTarget.State.ExitState(new StickmanRagdoll(debugTarget));
        });

        DebugCommand godmodeOn = new DebugCommand("GODMODE ON", () =>
        {
            TorsoCollision torso = debugTarget.Bodyparts[0].GetComponent<TorsoCollision>();
            torso.HitVelocityTolerance = float.MaxValue; 
            foreach (GameObject bodypart in debugTarget.Bodyparts)
            {
                bodypart.TryGetComponent(out PartCollision pc);
                if (pc != null)
                {
                    pc.HitVelocityTolerance = float.MaxValue;
                }
            }
        });

        DebugCommand godmodeOff = new DebugCommand("GODMODE OFF", () =>
        {
            TorsoCollision torso = debugTarget.Bodyparts[0].GetComponent<TorsoCollision>();
            torso.HitVelocityTolerance = torso.HitVelocityToleranceDefault;
            foreach (GameObject bodypart in debugTarget.Bodyparts)
            {
                bodypart.TryGetComponent(out PartCollision pc);
                if (pc != null)
                {
                    pc.HitVelocityTolerance = pc.HitVelocityToleranceDefault;
                }
            }
        });

        commands.Add(killCamera);
        commands.Add(stateMain);
        commands.Add(stateAirborne);
        commands.Add(stateRagdoll);
        commands.Add(godmodeOn);
        commands.Add(godmodeOff);
    }

    private void Update()
    {
        if (Input.GetKeyDown(debugtoggle))
        {
            ToggleConsole();
        }
    }

    private void ToggleConsole()
    {
        isConsoleOpened = !isConsoleOpened;
        if (isConsoleOpened)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
        }
    }

    private void ProcessCommand()
    {
        foreach (DebugCommand command in commands)
        {
            if (command.CommandID.ToUpper() == input.ToUpper())
            {
                command.Callback?.Invoke();
                input = null;
                break;
            }
        }
    }

    private void OnGUI()
    {
        if (!isConsoleOpened) return;
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return 
            && e.type == EventType.KeyDown 
            && !string.IsNullOrEmpty(input))
        {
            ProcessCommand();
        }
        else if (e.keyCode == debugtoggle && e.type == EventType.KeyDown)
        {
            ToggleConsole();
        }

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, TEXT_SIZE_Y), string.Empty);
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.skin.textField.fontSize = FONT_SIZE;
        GUI.SetNextControlName("InputField");

        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, TEXT_SIZE_Y), input);
        GUI.FocusControl("InputField");
    }
}
