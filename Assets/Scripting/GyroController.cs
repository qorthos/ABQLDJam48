using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class GyroController : MonoBehaviour
{
    public PlayerData PlayerData;
    public Rigidbody2D RB2D;
    public Animator Animator;
    public GameObject GyroGraphics;

    public bool IsFacingRight = true;
    TweenBase flipTween;

    float invulnCountdown;
    public float releaseCountdown;

    Vector2 inputAxes;
    public Vector2 Force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        // timers;
        invulnCountdown = Mathf.Clamp01(invulnCountdown - Time.deltaTime);
        releaseCountdown = Mathf.Clamp(releaseCountdown - Time.deltaTime, 0, 0.2f);


        SetThrottle();
        ConsumeFuel();
    }

    private void HandleInput()
    {
        // direct input stuff:
        inputAxes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Rotate(new Vector3(0, 0, -inputAxes.x * Time.deltaTime * 30f));
        if (Input.GetKey(KeyCode.Space) && flipTween == null)
        {
            if (IsFacingRight)
            {
                flipTween = Tween.LocalScale(GyroGraphics.transform, new Vector3(-1, 1, 1), 0.5f, 0f, Tween.EaseInOutBack, completeCallback: () => flipTween = null);
                IsFacingRight = false;
            }
            else
            {
                flipTween = Tween.LocalScale(GyroGraphics.transform, new Vector3(+1, 1, 1), 0.5f, 0f, Tween.EaseInOutBack, completeCallback: () => flipTween = null);
                IsFacingRight = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && (releaseCountdown < 0.001f))
        {
            GameObject poppedGameObject;
            if (PlayerData.TryPopConnectedObject(out poppedGameObject))
            {
                poppedGameObject.GetComponent<ILootable>().Disconnect();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    private void SetThrottle()
    {
        if (Mathf.Abs(inputAxes.y) < 0.01f)
        {
            var throttleDelta = PlayerData.Throttle - PlayerData.RestingThrottle;
            PlayerData.Throttle += -Mathf.Sign(throttleDelta) * Time.deltaTime * 100f;
        }
        else
        {
            PlayerData.Throttle += inputAxes.y * Time.deltaTime * 100f;
        }

        PlayerData.Throttle = Mathf.Clamp(PlayerData.Throttle, PlayerData.MinThrottle, PlayerData.MaxThrottle);

        var throttlePct = (PlayerData.Throttle - PlayerData.MinThrottle) / (PlayerData.MaxThrottle - PlayerData.MinThrottle);
        Animator.SetFloat("ThrottleSpeed", throttlePct * 2.0f + 1.0f);

        if (throttlePct > 0.95f)
        {
            Animator.SetBool("FullThrottle", true);
        }
        else
        {
            Animator.SetBool("FullThrottle", false);
        }
    }

    private void ConsumeFuel()
    {
        PlayerData.Fuel -= (PlayerData.Throttle / 100f) * Time.deltaTime; // 60 units of fuel per second when hovering

        Force = transform.up * 100f * PlayerData.Throttle / 100f;
    }

    private void TakeDamage()
    {
        if (invulnCountdown > 0)
            return;

        Animator.SetTrigger("Damaged");
        invulnCountdown = 1f;

        PlayerData.Damage = Mathf.Clamp(PlayerData.Damage + 0.32f, 0, 1);
        if (PlayerData.Damage > 0.999f)
        {
            Debug.Log("Gameover man");
        }
    }

    private void FixedUpdate()
    {
        RB2D.AddForce(Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * PlayerData.Throttle / 100f * 4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"IT IS {collision.relativeVelocity} BONG OCLOCK: ");

        if (collision.relativeVelocity.magnitude > 1f)
        {
            TakeDamage();
        }
    }
}
