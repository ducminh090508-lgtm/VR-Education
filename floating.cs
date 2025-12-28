using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floating : MonoBehaviour
{
    public Rigidbody rb; 
    public float submergeDepth = 1f; 
    public float waterDrag = 1f; 
    public float floaterCount = 1f; 
    public float waterAngularDrag = 0.5f; 
    public float displacementAmount = 3f; 

    public void FixedUpdate ()
    {
        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        
        if (transform.position.y < 0f)
        {
            float displacementMultiplier = Mathf.Clamp01 (-transform.position.y / submergeDepth) * displacementAmount;
            rb.AddForceAtPosition (new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce (displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange); 
            rb.AddTorque (displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

        }
    }

}
