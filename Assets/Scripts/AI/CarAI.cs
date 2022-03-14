using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    enum ETargetRotation { Target, MoveVector, VelocityVector};

    [System.Serializable]
    public class CarSystem
    {
        public MovementSO system;
        public float multiplier;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class CarAI : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField]
        private bool drawGizmo = false;
        [Header("Target system")]
        [SerializeField]
        private CarSystem navigationSystem;
        [SerializeField]
        private float maxForce = 50f;
        [SerializeField]
        private float minForce = 30f;
        [SerializeField, Tooltip("Degrees per second")]
        private float rotationSpeed = 15f;
        [SerializeField]
        private ETargetRotation lookAt;


        [Header("Other systems")]
        [SerializeField]
        private List<CarSystem> systemList;

        private Rigidbody _rb;
        private float _limitForce;
        private Quaternion _desiredRotation, _tempRotation;

        public void DisableCar()
        {
            gameObject.SetActive(false);
            TransportManager.Instance.RespawnCar(transform);
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            if (navigationSystem.system != null)
            {
                navigationSystem.system = Instantiate(navigationSystem.system);
                navigationSystem.system.InitSO(this);
            }
            else
            {
                enabled = false;
                Debug.LogError("assign navigation system!", gameObject);
            }


            for (int i = 0; i < systemList.Count; i++)
            {
                systemList[i].system = Instantiate(systemList[i].system);
                systemList[i].system.InitSO(this);
            }
        }

        private void OnEnable()
        {
            _desiredRotation = transform.rotation;
            _limitForce = Random.Range(minForce, maxForce);
            if(navigationSystem.system != null)
                navigationSystem.system.Initialize(this);

            for (int i = 0; i < systemList.Count; i++)
                systemList[i].system.Initialize(this);
        }

        private void FixedUpdate()
        {
            Vector3 move = Vector3.zero;
            Vector3 force = Vector3.zero;

            //calculate navigation vector into separate Vector3
            //for using in Rotation stuff
            move = navigationSystem.system.CalcMoveVector();
            //init car force by navigation force
            force = move * navigationSystem.multiplier;
            //add other system forces
            for (int i = 0; i < systemList.Count; i++)
                force += systemList[i].system.CalcMoveVector() * systemList[i].multiplier;

            //limit max force size
            force.Normalize();
            _rb.AddForce(force * _limitForce * navigationSystem.system.ValueFactor);

            switch (lookAt)
            {
                case ETargetRotation.MoveVector:
                    RotateTo(force);
                    break;
                case ETargetRotation.VelocityVector:
                    RotateTo(_rb.velocity);
                    break;
                case ETargetRotation.Target:
                    RotateTo(move);
                    break;
            }
        }

        private void RotateTo(Vector3 dir)
        {
            if (dir == Vector3.zero)
                return;
            //make a smooth rotation
            _tempRotation = Quaternion.LookRotation(dir, Vector3.up);
            _desiredRotation = Quaternion.RotateTowards(_desiredRotation, _tempRotation, rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, Time.fixedDeltaTime);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (drawGizmo)
            {
                if (navigationSystem.system != null)
                    navigationSystem.system.MyGizmos(transform);

                Gizmos.color = Color.white;
                for (int i = 0; i < systemList.Count; i++)
                    systemList[i].system.MyGizmos(transform);
            }
        }
#endif
    }
}