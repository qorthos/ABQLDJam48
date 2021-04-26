using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCheckPointController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("VICTORY!");

            var gyroController = collision.gameObject.GetComponent<GyroController>();
            gyroController.Victory();

        }
    }

}
