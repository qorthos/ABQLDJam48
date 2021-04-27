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

    private void OnTriggerStay2D(Collider2D collision)
    {
        var iLootable = collision.gameObject.GetComponent<ILootable>();

        if (iLootable != null)
        {
            if (iLootable.IsConnected == true)
                return;

            if (iLootable.IsDepositing == true)
                return;

            Debug.Log("om nom nom");

            var newGO = Instantiate(iLootable.RewardPrefab);
            newGO.transform.position = transform.position;
            newGO.transform.position += new Vector3(0, 1, 0);
            newGO.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-8, 8), Random.Range(1, 4)));


            iLootable.Deposit();

        }
    }
}
