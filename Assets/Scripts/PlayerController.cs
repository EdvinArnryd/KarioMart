using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 90f;

    public float accelerationFactor = 30f;
    public float turnFactor = 3.5f;

    private float accelerationInput = 0;
    private float steeringInput = 0;

    private float rotationAngle = 0f;

    private Rigidbody carRigidBody;

    void Awake()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        // var horizontalInput = Input.GetAxis("Horizontal");
        // var verticalInput = Input.GetAxis("Vertical");
        //
        // var movement = new Vector3(0.0f, 0.0f, verticalInput) * moveSpeed * Time.deltaTime;
        // transform.Translate(movement);
        //
        // var rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
        // transform.Rotate(Vector3.up, rotationAmount);
    }

    private void FixedUpdate()
    {
        CarRun();
        
        CarSteer();
    }

    private void CarRun()
    {
        var carRunVector = transform.forward * accelerationInput * accelerationFactor;
        
        carRigidBody.AddForce(carRunVector, ForceMode.Force);
        
    }

    private void CarSteer()
    {
        rotationAngle += steeringInput * turnFactor;
        
        carRigidBody.MoveRotation(Quaternion.Euler(0, rotationAngle, 0));
    }
    

    public void SetInputVector(Vector3 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.z;
    }
}
