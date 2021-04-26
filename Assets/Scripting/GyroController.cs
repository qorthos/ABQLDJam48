using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class GyroController : MonoBehaviour
{
    public GameEventChannel GameEventChannel;
    public PlayerData PlayerData;
    public Rigidbody2D RB2D;
    public Animator Animator;
    public GameObject GyroGraphics;
    public ParticleSystem ExhaustParticleSystem;
    public PointEffector2D MagnetEffector;


    public AudioSource GyroAudio;
    public AudioClip BumpClip;
    public AudioClip PowerDownClip;

    public bool IsFacingRight = true;
    TweenBase flipTween;

    bool isDead = false;

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
        if (isDead == true)
            return;

        // direct input stuff:
        inputAxes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Rotate(new Vector3(0, 0, -inputAxes.x * Time.deltaTime * 30f));
        if (Input.GetKey(KeyCode.Space) && flipTween == null)
        {
            if (IsFacingRight)
            {
                flipTween = Tween.LocalScale(GyroGraphics.transform, new Vector3(-1, 1, 1), 0.5f, 0f, Tween.EaseInOutBack, completeCallback: () => flipTween = null);
                IsFacingRight = false;
                Tween.LocalScale(ExhaustParticleSystem.transform, new Vector3(-1, 1, 1), 0.5f, 0f, Tween.EaseInOutBack, completeCallback: () => flipTween = null);
            }
            else
            {
                flipTween = Tween.LocalScale(GyroGraphics.transform, new Vector3(+1, 1, 1), 0.5f, 0f, Tween.EaseInOutBack, completeCallback: () => flipTween = null);
                IsFacingRight = true;
                Tween.LocalScale(ExhaustParticleSystem.transform, new Vector3(+1, 1, 1), 0.5f, 0f, Tween.EaseInOutBack, completeCallback: () => flipTween = null);
            }
        }

        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("F");
            // turn off magnet
            MagnetEffector.enabled = false;

            if (releaseCountdown < 0.001f)
            {
                releaseCountdown = 0.2f;
                
                GameObject poppedGameObject;
                if (PlayerData.TryPopConnectedObject(out poppedGameObject))
                {
                    poppedGameObject.GetComponent<ILootable>().Disconnect();
                }
                Debug.Log(poppedGameObject);
            }
        }
        else
        {
            // turn on magnet
            MagnetEffector.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    private void SetThrottle()
    {
        if (isDead == true)
        {
            return;
        }

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

        var emission = ExhaustParticleSystem.emission;
        var rot = ExhaustParticleSystem.emission.rateOverTime;
        rot.constant = 3f + 3f * throttlePct;
        emission.rateOverTime = rot;

        GyroAudio.pitch = 1.5f * PlayerData.Throttle / 100f;
        GyroAudio.volume = 0.1f + PlayerData.Throttle / 100f / 3.0f;
    }

    private void ConsumeFuel()
    {
        if (isDead)
            return;

        PlayerData.Fuel -= (PlayerData.Throttle / 100f) * Time.deltaTime * 0.75f; // 45 units of fuel per second when hovering

        Force = transform.up * 100f * PlayerData.Throttle / 100f;

        if (PlayerData.Fuel <= 0f)
        {
            isDead = true;
            Debug.Log("Gameover man");
            PlayerData.Throttle = 0;

            var sceneTransitioner = FindObjectOfType<SceneTransitioner>();
            sceneTransitioner.GameOver();

            GameEventChannel.Broadcast(GameEventEnum.PlayLocalAudio, new AudioEventArgs()
            {
                AudioClip = PowerDownClip,
                Position = transform.position,
            });

        }

    }

    private void TakeDamage()
    {
        if (invulnCountdown > 0)
            return;

        if (isDead)
            return;

        Animator.SetTrigger("Damaged");
        invulnCountdown = 1f;

        PlayerData.Damage = Mathf.Clamp(PlayerData.Damage + 0.24f, 0, 1);
        if (PlayerData.Damage >= 1f)
        {
            isDead = true;
            Debug.Log("Gameover man");
            PlayerData.Throttle = 0;

            var sceneTransitioner = FindObjectOfType<SceneTransitioner>();
            sceneTransitioner.GameOver();

            GameEventChannel.Broadcast(GameEventEnum.PlayLocalAudio, new AudioEventArgs()
            {
                AudioClip = PowerDownClip,
                Position = transform.position,
            });

        }
        else
        {
            GameEventChannel.Broadcast(GameEventEnum.PlayLocalAudio, new AudioEventArgs()
            {
                AudioClip = BumpClip,
                Position = transform.position,
            });
        }

        
    }

    public void Victory()
    {
        isDead = true; //lol
        //RB2D.simulated = false;
        RB2D.velocity = Vector2.zero;
        RB2D.drag = 1000f;
        RB2D.angularDrag = 1000f;
        FindObjectOfType<SceneTransitioner>().FinishGame();
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
