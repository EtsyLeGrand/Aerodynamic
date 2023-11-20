using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounceModifier = 5.0f;
    [SerializeField] private float resetTimer = 0.5f;

    private float torsoVelocityAdjustment = 10.0f;
    private bool isActivated = true;

    private void OnCollisionEnter(Collision c)
    {

        /*
         * À faire: Ajouter un minimum de vélocité et d'angle pour activer un rebond
         */

        if (isActivated && c.gameObject.layer == LayerMask.NameToLayer("Stickman"))
        {
            StickmanBase stickman = c.gameObject.transform.root.GetComponent<StickmanBase>();

            if (stickman.StateName == StickmanBase.StickmanStateName.Airborne)
            {
                Debug.Log("Main : " + stickman.MainBody.velocity + "\nTorso : " + stickman.TorsoBody.velocity);
                stickman.TorsoBody.velocity = Vector3.Reflect(torsoVelocityAdjustment * bounceModifier * stickman.TorsoBody.velocity, transform.up);
            }
            else
            {
                stickman.MainBody.velocity = Vector3.Reflect(bounceModifier * stickman.MainBody.velocity, transform.up);
            }
            
            isActivated = false;
            StartCoroutine(ReEnable());
        }
    }

    private IEnumerator ReEnable()
    {
        float timer = 0f;
        while (timer < resetTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        isActivated = true;
    }
}
