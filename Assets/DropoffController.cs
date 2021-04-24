using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var iLootable = collision.gameObject.GetComponent<ILootable>();

        if (iLootable != null)
        {
            Debug.Log("om nom nom");
            iLootable.Deposit();
        }
    }
}
