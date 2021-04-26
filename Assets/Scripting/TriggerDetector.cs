using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    public Rb2dUnityEvent OnTriggerEntered;
    public bool DoOnce = true;

    bool isTurnedOn = false;
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if ((timer > 0.5f) && (isTurnedOn == false))
        {
            isTurnedOn = true;
            gameObject.layer = 8;
        }

        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        OnTriggerEntered.Invoke(collision.attachedRigidbody);

        if (DoOnce)
        {
            Destroy(this);
        }

    }
}
