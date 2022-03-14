using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCar : MonoBehaviour
    {
        public float TurnRate { get; set; } = 0f; //-1 turn left, 1 turn right
        public float AccelerationForward { get; set; } = 0f; //1 forward, -1 back
        public float AccelerationStrafe { get; set; } = 0f; //1 right strafe, -1 left strafe
        public float AccelerationLift { get; set; } = 0f; //1 - move up, -1 move down

        [Header("Engines")]
        [SerializeField]
        private float thrusterStrength = 1f;
        [SerializeField]
        private float minThrusterDistance = 1.5f;
        [SerializeField]
        private LayerMask hitLayers;
        [SerializeField]
        private List<Transform> trusters;

        [Header("Movement")]
        [SerializeField]
        private float torque = 1f;
        [SerializeField]
        private float acceleration = 1f;

        [Header("Vertical turn angle")]
        [SerializeField]
        private float maxVerticalAngle = 25f;
        [SerializeField]
        private float rotationSpeed = 0.2f;

        private Rigidbody _rb;

        private RaycastHit _hit;
        private float _thrusterDistance;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _thrusterDistance = minThrusterDistance;
        }

        private void FixedUpdate()
        {
            EnginesTick();
            MovementTick();
        }

        private void EnginesTick()
        {
            float thrusterFactor;
            Vector3 force;

            //do engine raycast to down and add "up" force depending on raycast distance
            foreach (Transform thruster in trusters)
            {
                if (Physics.Raycast(thruster.position, Vector3.down, out _hit, _thrusterDistance, hitLayers))
                {
                    //calc thruster distance to the ground
                    //closer the ground - more power
                    thrusterFactor = 1f - _hit.distance / _thrusterDistance;

                    force = Vector3.up * _rb.mass * thrusterFactor * thrusterStrength;

                    //apply force from thrusters
                    _rb.AddForceAtPosition(force, thruster.position);
                }
            }
        }

        private void MovementTick()
        {
            float turn, angleVelocity = 0f;
            Vector3 angles;

            //add forward force
            _rb.AddForce(transform.forward * acceleration * AccelerationForward * _rb.mass);

            //add strafe force
            _rb.AddForce(transform.right * acceleration * AccelerationStrafe * _rb.mass);

            //add lift change
            _thrusterDistance += AccelerationLift * Time.fixedDeltaTime;
            _thrusterDistance = Mathf.Max(minThrusterDistance, _thrusterDistance);

            //add torque
            turn = TurnRate;//Input.GetAxis("Horizontal");
            _rb.AddRelativeTorque(Vector3.up * torque * turn);

            //add vertical rotation
            angles = transform.eulerAngles;
            //make smooth lerp to max angle
            angles.z = Mathf.SmoothDampAngle(angles.z, (turn + AccelerationStrafe) * -maxVerticalAngle, ref angleVelocity, rotationSpeed);
            transform.eulerAngles = angles;
        }
    }
}