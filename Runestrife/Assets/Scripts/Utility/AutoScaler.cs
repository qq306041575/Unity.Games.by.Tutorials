using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaler : MonoBehaviour
{
    public float scaleSpeed;

    Vector3 scale;

    void Awake()
    {
        scale = transform.localScale;
    }

    void Update()
    {
        scale += Vector3.one * scaleSpeed * Time.deltaTime;
        if (scale.x < 0 || scale.y < 0 || scale.z < 0)
        {
            return;
        }
        transform.localScale = scale;
    }
}
