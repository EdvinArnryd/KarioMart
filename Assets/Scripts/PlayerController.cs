using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Car settings")]
    public float accelerationFactor = 12f;
    public float turnFactor = 3.5f;
    public float driftFactor = 0.95f;
    public float maxSpeed = 15f;

    private float accelerationInput = 0f;
    private float steeringInput = 0f;
    private float rotationAngle = 0f;
    private float velocityVsUp = 0f;

    private Rigidbody carRigidBody;

    private float speedBoost = 1f;
    private float boostTimer = 0f;
    private bool boosting = false;

    private void Awake()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Drive();
        
        KillOrthogonalVelocity();
        
        Steer();
    }

    private void Drive()
    {
        velocityVsUp = Vector3.Dot(transform.forward, carRigidBody.velocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0)
        {
            return;
        }

        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        if (boosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= 2)
            {
                speedBoost = 1f;
                boostTimer = 0;
                boosting = false;
            }
        }
        
        var carRunVector = transform.forward * accelerationInput * accelerationFactor * speedBoost;
        
        carRigidBody.AddForce(carRunVector, ForceMode.Force);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SpeedBoost")
        {
            boosting = true;
            speedBoost = 1.5f;
        }
    }

    private void Steer()
    {
        var minSpeedBeforeAllowTurningFactor = (carRigidBody.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
        
        rotationAngle += steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;
        
        carRigidBody.MoveRotation(Quaternion.Euler(0, rotationAngle, 0));
    }

    private void KillOrthogonalVelocity()
    {
        var forwardVelocity = transform.forward * Vector3.Dot(carRigidBody.velocity, transform.forward);
        var rightVelocity = transform.right * Vector3.Dot(carRigidBody.velocity, transform.right);

        carRigidBody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }
    

    public void SetInputVector(Vector3 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.z;
    }
}
