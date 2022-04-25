using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scifi.Tools;

namespace Scifi.AI
{
    [CreateAssetMenu(fileName = "Avoid Obstacles System", menuName = "Movement AI/Avoid Obstacles")]
    public class AvoidObstaclesSystem : MovementSO
    {
        [Header("Avoid Obstacles System")]
        [SerializeField]
        private LayerMask obstaclesMask;
        [SerializeField]
        private float avoidRadius = 1f;
        [SerializeField]
        private float checkDelay = 0.2f;

        private Timer _timer;
        private Collider[] _obstaclesArray;
        private CarAI _carAI;
        private Transform _carTransform;
        private Rigidbody _rb;

        public override void InitSO(CarAI carAI)
        {
            base.InitSO(carAI);
            _timer = new Timer();
            _rb = carAI.GetComponent<Rigidbody>();
        }

        public override void Initialize(CarAI carAI)
        {
            base.Initialize(carAI);
            _carAI = carAI;
            _carTransform = _carAI.transform;
            _obstaclesArray = null;
            _timer.Activate(Random.value * checkDelay);
        }

        public override Vector3 CalcMoveVector()
        {
            //calculate avoidance vector
            if(_timer.IsFinished)
            {
                OverlapCollisions();
            }

            return CalcAvoidance();
        }

        /// <summary>
        /// calculate soft avoidance, closer objects affects more than far objects
        /// </summary>
        private Vector3 CalcAvoidance()
        {
            Vector3 avoidance = Vector3.zero;
            Vector3 v;
            float f;

            if (_obstaclesArray == null)
                return Vector3.zero;

            for (int i = 0; i < _obstaclesArray.Length; i++)
                if (_obstaclesArray[i].attachedRigidbody != _rb)
                {
                    v = _carTransform.position - _obstaclesArray[i].transform.position;
                    f = v.sqrMagnitude;
                    if(f>0.01f)
                        avoidance += v / v.sqrMagnitude; //v.normalized * (_sqrDist - v.sqrMagnitude) / _sqrDist;
                }

            //avoidance /= _obstaclesArray.Length;

            Debug.DrawRay(_carTransform.position, avoidance.normalized * 5f, Color.blue);

            return avoidance.normalized;
        }

        private void OverlapCollisions()
        {
            _timer.Activate(checkDelay);
            _obstaclesArray = Physics.OverlapSphere(_carTransform.position, avoidRadius, obstaclesMask);
        }
    }
}