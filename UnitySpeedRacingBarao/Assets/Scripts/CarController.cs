using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{    
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBrakeForce;
    private bool isBraking;
    private bool handbrakeActive = false;
    private bool headlightsOn = false;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float handbrakeTorque;

    [SerializeField] private Light leftHeadlight;
    [SerializeField] private Light rightHeadlight;

    private void FixedUpdate() 
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        RestartPosition();
    }

    private void RestartPosition()
    {
       if(Input.GetKey("r"))
       {
           Debug.Log("RestartPosition");
           transform.position = new Vector3(3f, transform.position.y, transform.position.z);
           transform.rotation = Quaternion.Euler(0f, 0f, 0f);
       }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.H)) handbrakeActive = !handbrakeActive;
        if (Input.GetKeyDown(KeyCode.L)) ToggleHeadlights();
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        
        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        frontLeftWheelCollider.brakeTorque = currentBrakeForce; 
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
        
        if (handbrakeActive)
        {
            rearLeftWheelCollider.brakeTorque = handbrakeTorque;
            rearRightWheelCollider.brakeTorque = handbrakeTorque;
        }
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
       UpdateSingleWheelCollider(frontLeftWheelCollider, frontLeftWheelTransform);
       UpdateSingleWheelCollider(frontRightWheelCollider, frontRightWheelTransform);
       UpdateSingleWheelCollider(rearRightWheelCollider, rearRightWheelTransform);
       UpdateSingleWheelCollider(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheelCollider(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos,  out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void ToggleHeadlights()
    {
        headlightsOn = !headlightsOn;
        leftHeadlight.enabled = headlightsOn;
        rightHeadlight.enabled = headlightsOn;
    }
}
