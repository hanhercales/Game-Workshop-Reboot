using System;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed;

    [SerializeField] private float xLimit;
    
    private Vector3 originalPosition;
    private ParallaxCamera parallaxCamera;

    private void Start()
    {
        originalPosition = transform.localPosition;
        
        parallaxCamera = FindObjectOfType<ParallaxCamera>();
        if (parallaxCamera != null)
        {
            parallaxCamera.onCameraTranslate += Move;
        }
    }

    private void Move(float deltaMovement)
    {
        Vector3 newPosition = transform.position;
        newPosition.x += deltaMovement * speed;
        transform.position = newPosition;

        if (Mathf.Abs(transform.localPosition.x) > xLimit)
        {
            speed *= -1;
        }
    }
}
