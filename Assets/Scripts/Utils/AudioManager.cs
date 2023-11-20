using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager audioManager;

    [SerializeField] private List<GameObject> audioCategories;

    public static AudioManager instance
    {
        get
        {
            if (!audioManager)
            {
                audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;

                if (!audioManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    audioManager.Init();
                }
            }
            return audioManager;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(instance);
    }

    void Init()
    {

    }

    public static void PlaySound(int cat)
    {
        AudioSource[] sources = instance.audioCategories[cat].GetComponents<AudioSource>();

        sources[Random.Range(0, sources.Length)].Play();
    }

    public static void StopSound(int cat)
    {
        AudioSource[] sources = instance.audioCategories[cat].GetComponents<AudioSource>();

        foreach (AudioSource src in sources)
        {
            src.Stop();
        }
    }

    public static void StopAllSound()
    {
        foreach (GameObject obj in instance.audioCategories)
        {
            AudioSource[] sources = obj.GetComponents<AudioSource>();

            foreach (AudioSource src in sources)
            {
                src.Stop();
            }
        }
    }
}