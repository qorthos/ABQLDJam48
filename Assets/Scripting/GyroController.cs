using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour
{
    public PlayerData PlayerData;
    public Rigidbody2D RB2D;
    public Animator Animator;

    Vector2 inputAxes;
    public Vector2 Force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // direct input stuff:
        inputAxes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Rotate(new Vector3(0, 0, -inputAxes.x * Time.deltaTime * 30f));

        SetThrottle();
        ConsumeFuel();
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

    private void FixedUpdate()
    {
        RB2D.AddForce(Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * PlayerData.Throttle / 100f * 4f);
    }
}
