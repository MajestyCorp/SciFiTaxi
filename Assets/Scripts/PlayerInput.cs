using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scifi.Input;
using UnityEngine.InputSystem;
using System;

namespace Scifi
{
    public class PlayerInput : MonoBehaviour
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

        private Quaternion _desiredRotation;
        private Transform _car;

        //new input system stuff
        private InputActions _inputActions;
        private Vector2 _movement, _look, _cameraAngles;
        private float _lift;

        private void Awake()
        {
            _inputActions = new InputActions();
        }

        private void OnEnable()
        {
            _inputActions.carControl.Enable();
        }

        private void Start()
        {
            //initialize start angles
            Vector3 angles;
            angles = carCamera.eulerAngles;
            _cameraAngles.x = angles.x;
            _cameraAngles.y = angles.y;
            _desiredRotation = carCamera.rotation;

            _car = carTarget.transform;
        }

        private void LateUpdate()
        {
            HandleRotation();
            MoveToDesiredPos();

            ControlCar();
        }

        private void ControlCar()
        {
            Vector3 vCamera, vCar;

            //how to calc Car.TurnRate
            //calculate signed angle between two "forward" vectors from top down view
            //forward camera vector and forward car vector
            vCamera = Quaternion.Euler(0f, carCamera.eulerAngles.y, 0f) * Vector3.forward;
            vCar = Quaternion.Euler(0f, _car.eulerAngles.y, 0f) * Vector3.forward;
            carTarget.TurnRate = Vector3.SignedAngle(vCar, vCamera, Vector3.up) * turnStrength;

            //read input values
            _movement = _inputActions.carControl.Movement.ReadValue<Vector2>();
            _lift = _inputActions.carControl.Lift.ReadValue<float>();
            
            //apply acceleration
            carTarget.AccelerationForward = _movement.y * accelerationStrength;
            carTarget.AccelerationStrafe = _movement.x * accelerationStrength;

            carTarget.AccelerationLift = _lift * liftUpStrength;
        }

        private void HandleRotation()
        {
            _look = _inputActions.carControl.Look.ReadValue<Vector2>();

            _cameraAngles += _look * sensitivity * Time.deltaTime;
            _cameraAngles.y = Mathf.Clamp(_cameraAngles.y, -80f, 80f);
            _desiredRotation = Quaternion.Euler(-_cameraAngles.y, _cameraAngles.x, 0);
        }

        private void MoveToDesiredPos()
        {
            //smoothly move camera to desired rotation
            carCamera.rotation = Quaternion.Lerp(carCamera.rotation, _desiredRotation, Time.deltaTime * dampening);

            //update camera local position
            carCamera.position = _car.position + Vector3.up * camHeight - carCamera.forward * camDistance;
        }

        private void OnDisable()
        {
            _inputActions.carControl.Disable();
        }
    }
}