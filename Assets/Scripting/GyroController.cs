using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour
{
    public PlayerData PlayerData;
    public Rigidbody2D RB2D;
    
    Vector2 inputAxes;
    public Vector2 Force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputAxes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.Rotate(new Vector3(0, 0, -inputAxes.x * Time.deltaTime * 30f));

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
