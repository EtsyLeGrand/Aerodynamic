using UnityEngine;
public class NoteScript : MonoBehaviour
{
#if UNITY_EDITOR
    [TextArea]
    [Tooltip("This component doesn't do anything. It serves only to write notes in the inspector")]
    [SerializeField] private string notes;
#endif
}