using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class CrystalController : MonoBehaviour, ILootable
{
    public GameEventChannel GameEventChannel;
    public PlayerData PlayerData;
    public Rigidbody2D RB2D;
    public FixedJoint2D FixedJoint;
    public SpriteRenderer Sprite;
    public GameObject Reward;

    public float Damage = 0;
    public float invulnTimer = 0;

    public AudioClip DepositClip;
    public AudioClip ConnectedClip;

    bool isDepositing = false;
    public bool IsConnected => FixedJoint.connectedBody != null;

    public GameObject RewardPrefab { get => Reward; set => Reward = value; }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        invulnTimer = Mathf.Clamp01(invulnTimer - Time.deltaTime);

        if ((invulnTimer == 0) && (gameObject.layer == 9))
        {
            gameObject.layer = 6;
        }
    }

    public void Connect(Rigidbody2D rb)
    {
        if (invulnTimer > 0)
            return;

        FixedJoint.enabled = true;
        FixedJoint.connectedBody = rb;
        FixedJoint.autoConfigureConnectedAnchor = false;

        PlayerData.AddConnectedObject(this.gameObject);

        GameEventChannel.Broadcast(GameEventEnum.PlayLocalAudio, new AudioEventArgs()
        {
            AudioClip = ConnectedClip,
            Position = transform.position,
        });
    }

    public void Disconnect()
    {
        if (FixedJoint.enabled == true)
        {
            FixedJoint.connectedBody = null;
            FixedJoint.enabled = false;
            PlayerData.RemoveConnectedObject(this.gameObject);
        }        
        gameObject.layer = 9;
        invulnTimer = 1.0f;
    }

    public void Deposit()
    {
        Disconnect();

        if (isDepositing)
            return;

        GameEventChannel.Broadcast(GameEventEnum.PlayLocalAudio, new AudioEventArgs()
        {
            AudioClip = DepositClip,
            Position = transform.position,
        });

        isDepositing = true;

        PlayerData.CrystalsCollected += 1;
        var destroyTween = Tween.LocalScale(transform, new Vector3(0.01f, 0.01f, 0.01f), 0.3f, 0, Tween.EaseOut, completeCallback: () => Destroy(gameObject));
    }

    public void Explode()
    {
        Destroy(gameObject);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (invulnTimer > 0)
    //        return;

    //    Debug.Log($"LOOT IMPACT {collision.relativeVelocity}");
    //    if (collision.relativeVelocity.magnitude > 2.0f)
    //    {
    //        invulnTimer = 1.0f;
    //        Explode();
    //    }
    //}
}
