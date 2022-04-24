﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField]
        private PlayerCar carTarget;
        [SerializeField]
        private Transform carCamera;
        
        [Header("Car controls")]
        [SerializeField]
        private float turnStrength;
        [SerializeField]
        private float accelerationStrength;
        [SerializeField]
        private float liftUpStrength = 3f;

        [Header("3rd camera settings")]
        [SerializeField]
        private float camDistance = 2f;
        [SerializeField]
        private float camHeight = 0.8f;
        [SerializeField]
        private float sensitivity = 1f;
        [Header("Dump settings")]
        [SerializeField, Tooltip("how fast camera moves to desired position")]
        private float dampening = 10f;

        private float _angleX, _angleY;
        private Quaternion _desiredRotation;
        private Transform _car;

        private void Start()
        {
            //initialize start angles
            Vector3 angles;
            angles = carCamera.eulerAngles;
            _angleX = angles.x;
            _angleY = angles.y;
            _desiredRotation = carCamera.rotation;

            _car = carTarget.transform;
        }

        private void LateUpdate()
        {
            //handle camera rotation
            HandleRotation();
            MoveToDesiredPos();

            ControlCar();
        }

        private void ControlCar()
        {
            Vector3 angles;
            Vector3 vCamera, vCar;

            //how to calc Car.TurnRate
            //calculate signed angle between two "forward" vectors from top down view
            //camera and car
            angles = carCamera.eulerAngles;
            vCamera = Quaternion.Euler(0f, angles.y, 0f) * Vector3.forward;

            angles = _car.eulerAngles;
            vCar = Quaternion.Euler(0f, angles.y, 0f) * Vector3.forward;

            carTarget.TurnRate = Vector3.SignedAngle(vCar, vCamera, Vector3.up) * turnStrength;

            //apply acceleration
            carTarget.AccelerationForward = Input.GetAxis("Vertical") * accelerationStrength;
            carTarget.AccelerationStrafe = Input.GetAxis("Horizontal") * accelerationStrength;

            if (Input.GetKey(KeyCode.E))
                carTarget.AccelerationLift = liftUpStrength;
            else if (Input.GetKey(KeyCode.Q))
                carTarget.AccelerationLift = -liftUpStrength;
            else
                carTarget.AccelerationLift = 0f;
        }

        private void HandleRotation()
        {
            _angleX -= Input.GetAxis("Mouse Y") * sensitivity;
            _angleY += Input.GetAxis("Mouse X") * sensitivity;

            _angleX = Mathf.Clamp(_angleX, -80f, 80f);

            _desiredRotation = Quaternion.Euler(_angleX, _angleY, 0);
        }

        private void MoveToDesiredPos()
        {
            //smoothly move camera to desired rotation
            carCamera.rotation = Quaternion.Lerp(carCamera.rotation, _desiredRotation, Time.deltaTime * dampening);

            //update camera local position
            carCamera.position = _car.position + Vector3.up * camHeight - carCamera.forward * camDistance;
        }
    }
}