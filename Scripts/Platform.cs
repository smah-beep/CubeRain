using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float PositionByX { get; private set; }
    public float PositionByZ { get; private set; }

    private void Awake()
    {
        PositionByX = transform.localScale.x;
        PositionByZ = transform.localScale.z;
    }
}
