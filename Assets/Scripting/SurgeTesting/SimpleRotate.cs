using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;


public class SimpleRotate : MonoBehaviour
{
    public void Rotate()
    {
        Tween.Rotate(transform, new Vector3(0, 0, 360), Space.World, 1f, 0f, Tween.EaseInOutBack);
    }
}
