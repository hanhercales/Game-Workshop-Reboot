using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public float backgroundWidth;
    
    private Transform cameraTransform;
    
    public InfiniteBackground preBackground;
    public InfiniteBackground nextBackground;
    
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        MoveRight();
        MoveLeft();
    }

    private void MoveRight()
    {
        float cameraRightEdge = cameraTransform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
        if (transform.position.x + backgroundWidth < cameraRightEdge)
        {
            transform.position = new Vector3(nextBackground.transform.position.x + backgroundWidth,
                transform.position.y, transform.position.z);
        }
    }

    private void MoveLeft()
    {
        float cameraLeftEdge = cameraTransform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
        if (transform.position.x - backgroundWidth > cameraLeftEdge)
        {
            transform.position = new Vector3(preBackground.transform.position.x - backgroundWidth,
                transform.position.y, transform.position.z);
        }
    }
}
