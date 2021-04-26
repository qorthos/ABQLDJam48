using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRendering : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public GameObject AnchorA;
    public GameObject AnchorB;


    private void Start()
    {
        AdjustLines();
    }

    private void Update()
    {
        AdjustLines();
    }

    private void AdjustLines()
    {
        LineRenderer.SetPositions(new Vector3[2]
        {
            AnchorA.transform.position,
            AnchorB.transform.position
        });
    }
}
