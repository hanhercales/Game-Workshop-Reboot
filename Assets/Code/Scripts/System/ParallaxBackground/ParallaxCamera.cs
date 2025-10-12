using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    
    private float oldPosition;

    private void Start()
    {
        oldPosition = transform.position.x;
    }

    private void Update()
    {
        if(transform.position.x != oldPosition)
        {
            float delta = oldPosition - transform.position.x;
            onCameraTranslate?.Invoke(delta);
            oldPosition = transform.position.x;
        }
    }
}
