using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour, ILootable
{
    public PlayerData PlayerData;
    public Rigidbody2D RB2D;
    public FixedJoint2D FixedJoint;

    public float Damage = 0;
    public float invulnTimer = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        invulnTimer = Mathf.Clamp01(invulnTimer - Time.deltaTime);

        if ((invulnTimer == 0) && (gameObject.layer == 0))
        {
            gameObject.layer = 6;
        }
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
        if (FixedJoint.enabled == true)
        {
            FixedJoint.connectedBody = null;
            FixedJoint.enabled = false;
            PlayerData.RemoveConnectedObject(this.gameObject);
        }        
        gameObject.layer = 0;
        invulnTimer = 1.0f;
    }

    public void Deposit()
    {
        Disconnect();
        Destroy(gameObject);
    }

    public void Explode()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invulnTimer > 0)
            return;

        Debug.Log($"LOOT IMPACT {collision.relativeVelocity}");
        if (collision.relativeVelocity.magnitude > 2.0f)
        {
            Damage += 0.4f;
            invulnTimer = 1.0f;
            Disconnect();

            if (Damage > 1f)
            {
                Explode();
            }
        }
    }
}
