using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour
{
    public Rigidbody2D RB2D;
    Vector2 inputAxes;

    public float MinThrottle;
    public float MaxThrottle;
    public float RestingThrottle;
    public float Throttle;
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
            Throttle = RestingThrottle;
        }
        else
        {
            Throttle += inputAxes.y;
        }
        

        Throttle = Mathf.Clamp(Throttle, MinThrottle, MaxThrottle);


        Force = transform.up * 100f * Throttle / 100f;

    }

    private void FixedUpdate()
    {
        RB2D.AddForce(Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * Throttle / 100f * 4f);
    }
}
