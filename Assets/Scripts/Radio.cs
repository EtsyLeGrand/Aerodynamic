using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private AudioSource audioSource;
    private KeyCode key = KeyCode.E;
    private bool isActive = false;
    private bool isPlayerPresent = false;
    private string activateMessage = "turn on radio";
    private string deactivateMessage = "turn off radio";

    public event Action<KeyCode, string> onPlayerInRangeOfRadio;
    public event Action onPlayerOutOfRangeOfRadio;
    public event Action<AudioSource> onMusicPlay;
    public event Action onMusicStop;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Stickman") && !isPlayerPresent)
        {
            isPlayerPresent = true;
            if (!isActive)
            {
                onPlayerInRangeOfRadio.Invoke(key, activateMessage);
            }
            else
            {
                onPlayerInRangeOfRadio.Invoke(key, deactivateMessage);
            }
            
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Stickman") && isPlayerPresent)
        {
            isPlayerPresent = false;
            onPlayerOutOfRangeOfRadio.Invoke();
        }
    }

    private void Update()
    {
        if (isPlayerPresent && Input.GetKeyDown(key))
        {
            if (!isActive)
            {
                audioSource.Play();
                onMusicPlay.Invoke(audioSource);
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
                onPlayerInRangeOfRadio.Invoke(key, deactivateMessage);
            }
            else
            {
                audioSource.Stop();
                onMusicStop.Invoke();
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                onPlayerInRangeOfRadio.Invoke(key, activateMessage);
            }

            isActive = !isActive;
        }
    }
}
