using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class DwarfController : MonoBehaviour, ILootable
{
    public PlayerData PlayerData;
    public GameEventChannel GameEventChannel;
    public Rigidbody2D RB2D;
    public FixedJoint2D FixedJoint;
    public SpriteRenderer Sprite;
    public Animator Animator;
    public float invulnTimer = 0;
    public GameObject Reward;

    public bool IsConnected => FixedJoint.connectedBody != null;
    public GameObject RewardPrefab { get => Reward; set => Reward = value; }

    bool isDepositing = false;

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

        Animator.SetBool("IsConnected", true);
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

        Animator.SetBool("IsConnected", false);
    }

    public void Deposit()
    {
        if (isDepositing)
            return;

        isDepositing = true;
        Disconnect();
        PlayerData.DwarvesSaved += 1;

        if (PlayerData.DwarvesSaved == 1)
        {
            GameEventChannel.Broadcast(GameEventEnum.Dialogue, new DialogueEventArgs()
            {
                Msg = "Hey good job saving that dwarf! Looks like he gave you a repair kit too!"
            });
        }

        var destroyTween = Tween.LocalScale(transform, new Vector3(0.01f, 0.01f, 0.01f), 0.3f, 0, Tween.EaseOut, completeCallback: ()=>Destroy(gameObject));
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (invulnTimer > 0)
    //        return;

    //    Debug.Log($"DWARF IMPACT {collision.relativeVelocity}");
    //    if (collision.relativeVelocity.magnitude > 2.0f)
    //    {
    //        invulnTimer = 1.0f;
    //        Disconnect();
    //    }
    //}
}
