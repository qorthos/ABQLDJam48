using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    public PlayerData PlayerData;
    public Rigidbody2D RB2D;
    public FixedJoint2D FixedJoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect(Rigidbody2D rb)
    {
        FixedJoint.enabled = true;
        FixedJoint.connectedBody = rb;
        FixedJoint.autoConfigureConnectedAnchor = false;

        PlayerData.AddConnectedObject(this.gameObject);
    }

    public void Disconnect()
    {

    }
}
