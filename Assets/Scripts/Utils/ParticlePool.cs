using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    private static ParticlePool pool;

    public static ParticlePool instance
    {
        get
        {
            if (!pool)
            {
                pool = FindObjectOfType(typeof(ParticlePool)) as ParticlePool;
                
                if (!pool)
                {
                    Debug.LogError("There needs to be one active ParticlePool script on a GameObject in your scene.");
                }
            }

            return pool;
        }
    }

    public static void SpawnParticles(GameObject prefab, Vector3 position)
    {
        if (!prefab.TryGetComponent(out ParticleSystem ps))
        {
            Debug.LogWarning("Didn't find a ParticleSystem on sent prefab.");
            return;
        }

        GameObject objToUse = GetUsableObject(prefab);

        objToUse.transform.position = position;
        objToUse.GetComponent<ParticleSystem>().Play();
    }

    private static GameObject GetUsableObject(GameObject prefab)
    {
        foreach(Transform child in instance.transform)
        {
            Debug.Log(child.gameObject.name);
            if (!child.gameObject.activeInHierarchy && 
                child.gameObject.name == prefab.name &&
                !child.gameObject.GetComponent<ParticleSystem>().isPlaying)
            {
                child.gameObject.SetActive(true);
                return child.gameObject;
            }
        }

        GameObject temp = Instantiate(prefab, instance.transform);
        temp.name = prefab.name;
        return temp;
    }
}