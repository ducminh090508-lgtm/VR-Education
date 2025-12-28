using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalMotion : MonoBehaviour
{
   
    public float amplitude = 1.0f;
    public float frequency = 0.2f;
    
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
    }
    
    void Update()
    {

        float yOffset = amplitude * Mathf.Sin(Time.time * frequency * 2 * Mathf.PI);
        
        // Apply the offset to the original starting position's Y component.
        Vector3 currentPosition = startPosition;
        currentPosition.y += yOffset;
        
        // Update the water object's position.
        transform.position = currentPosition;
    }
}
