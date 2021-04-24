using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    public Rb2dUnityEvent OnTriggerEntered;
    public bool DoOnce = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEntered.Invoke(collision.attachedRigidbody);

        if (DoOnce)
        {
            Destroy(this);
        }

    }
}
