using UnityEditor;

[CustomEditor(typeof(StickmanBase))]
public class StickmanEditor : Editor
{
    #region Base
    SerializedProperty stateName;
    SerializedProperty animationManager;
    SerializedProperty animator;
    SerializedProperty bodyparts;
    SerializedProperty mousePercent;
    SerializedProperty axisPercent;
    SerializedProperty hurtFlashingTime;
    SerializedProperty impactParticle;
    #endregion

    #region Main
    // Main
    SerializedProperty speed;
    SerializedProperty maxSpeed;
    SerializedProperty jumpForce;
    #endregion

    #region Airborne
    // Airborne
    SerializedProperty bodypartSpringValue;
    SerializedProperty maxMouseXMove;
    SerializedProperty flipSpeed;
    SerializedProperty springTimeIncrement;
    #endregion

    #region Ragdoll
    SerializedProperty respawnPrefab;
    SerializedProperty respawnTime;
    SerializedProperty prefabSizeAccel;
    SerializedProperty idleBeforeRespawn;
    SerializedProperty requiredTimeInRagdollState;
    SerializedProperty maxRespawnVelocity;
    #endregion

    #region On Pole
    SerializedProperty spinSpeed;
    #endregion


    SerializedProperty jumpDirectionFromMousePosition;
    private static bool showBase = true, showMain = true, showRoll = true, showAirborne = true, showRagdoll = true, showOnPole = true;
    private static bool showBaseInputs = true;

    void OnEnable()
    {
        stateName = serializedObject.FindProperty("stateName");
        mousePercent = serializedObject.FindProperty("mousePercent");
        axisPercent = serializedObject.FindProperty("axisPercent");

        animationManager = serializedObject.FindProperty("animationManager");
        animator = serializedObject.FindProperty("animator");
        bodyparts = serializedObject.FindProperty("bodyparts");

        speed = serializedObject.FindProperty("speed");
        maxSpeed = serializedObject.FindProperty("maxSpeed");
        jumpForce = serializedObject.FindProperty("jumpForce");

        bodypartSpringValue = serializedObject.FindProperty("bodypartSpringValue");
        maxMouseXMove = serializedObject.FindProperty("maxMouseXMove");
        flipSpeed = serializedObject.FindProperty("flipSpeed");
        springTimeIncrement = serializedObject.FindProperty("springTimeIncrement");

        jumpDirectionFromMousePosition = serializedObject.FindProperty("jumpDirectionFromMousePosition");

        hurtFlashingTime = serializedObject.FindProperty("hurtFlashingTime");
        impactParticle = serializedObject.FindProperty("impactParticle");

        respawnPrefab = serializedObject.FindProperty("respawnPrefab");
        respawnTime = serializedObject.FindProperty("respawnTime");
        prefabSizeAccel = serializedObject.FindProperty("prefabSizeAccel");
        idleBeforeRespawn = serializedObject.FindProperty("idleBeforeRespawn");
        requiredTimeInRagdollState = serializedObject.FindProperty("requiredTimeInRagdollState");
        maxRespawnVelocity = serializedObject.FindProperty("maxRespawnVelocity");

        spinSpeed = serializedObject.FindProperty("spinSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showBase = EditorGUILayout.Foldout(showBase, "Base");
        if (showBase)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(bodyparts, includeChildren: true);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(stateName);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(animationManager);
            EditorGUILayout.PropertyField(animator);

            EditorGUILayout.PropertyField(hurtFlashingTime);
            EditorGUILayout.PropertyField(impactParticle);

            showBaseInputs = EditorGUILayout.Foldout(showBaseInputs, "Inputs");
            if (showBaseInputs)
            {
                EditorGUI.indentLevel++;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(mousePercent);
                EditorGUILayout.PropertyField(axisPercent);
                EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel--;
            }
            

            EditorGUI.indentLevel--;
        }

        showMain = EditorGUILayout.Foldout(showMain, "Main State");
        if (showMain)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(maxSpeed);
            EditorGUILayout.PropertyField(jumpForce);
            EditorGUILayout.PropertyField(jumpDirectionFromMousePosition);
            EditorGUI.indentLevel--;
        }

        showAirborne = EditorGUILayout.Foldout(showAirborne, "Airborne State");
        if (showAirborne)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(bodypartSpringValue);
            EditorGUILayout.PropertyField(springTimeIncrement);
            EditorGUILayout.PropertyField(maxMouseXMove);
            EditorGUILayout.PropertyField(flipSpeed);

            EditorGUI.indentLevel--;
        }

        showRagdoll = EditorGUILayout.Foldout(showRagdoll, "Ragdoll State");
        if (showRagdoll)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(respawnPrefab);
            EditorGUILayout.PropertyField(respawnTime);
            EditorGUILayout.PropertyField(prefabSizeAccel);
            EditorGUILayout.PropertyField(idleBeforeRespawn);
            EditorGUILayout.PropertyField(requiredTimeInRagdollState);
            EditorGUILayout.PropertyField(maxRespawnVelocity);

            EditorGUI.indentLevel--;
        }

        showOnPole = EditorGUILayout.Foldout(showOnPole, "On Pole State");
        if (showOnPole)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(spinSpeed);

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
