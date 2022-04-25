using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scifi.Tools;

namespace Scifi.AI
{
    [CreateAssetMenu(fileName = "Police System", menuName = "Movement AI/Police System")]
    public class PoliceNavigation : TargetSystem
    {
        protected Vector3 LeeloDirection { get { return _leeloo.position - _carTransform.position; } }

        [Header("Catch mode settings")]
        [SerializeField]
        private LayerMask hostileMask;
        [SerializeField]
        private float hostileScanRadius = 30f;
        [SerializeField, Range(0f, 180f), Tooltip("Angle between forward and hostile to activate catch mode")]
        private float maxHostileAngle = 90f;
        [SerializeField]
        private float checkDelay = 0.5f;
        [SerializeField]
        private float maxAccelerationFactor = 5f;

        private Collider[] _hostileArray;
        private Transform _leeloo;
        private Timer _checkTimer;
        private float _sqrDist;

        public override void InitSO(CarAI carAI)
        {
            base.InitSO(carAI);
            _checkTimer = new Timer();
            _sqrDist = hostileScanRadius * hostileScanRadius;
        }

        public override void Initialize(CarAI carAI)
        {
            base.Initialize(carAI);

            _leeloo = null;
            //init timer with random time at start
            //to reduce spikes on all cars
            _checkTimer.Activate(Random.value * checkDelay);
            _hostileArray = null; 
            ValueFactor = 1f;//reset acceleration to 1
        }

        public override Vector3 CalcMoveVector()
        {
            float value;
            Vector3 result;
            if (_leeloo == null)
            {
                result = base.CalcMoveVector();

                //check if we see Leeloo
                if (_checkTimer.IsFinished)
                    ScanArea();
            }
            else
            {
                result = LeeloDirection.normalized;

                //calc acceleration as a DOT value between forward and target direction
                //with it police will fly faster on straight path
                value = Vector3.Dot(_carTransform.forward, result);
                ValueFactor = 1f + Mathf.Clamp01(value) * maxAccelerationFactor;

                //check if we lost leeloo
                if (_checkTimer.IsFinished)
                    CheckTarget();
            }

            return result;
        }

        private void ScanArea()
        {
            _hostileArray = Physics.OverlapSphere(_carTransform.position, hostileScanRadius, hostileMask);

            if (_hostileArray == null || _hostileArray.Length == 0)
                return;

            //we found an enemy. Since there is only one enemy, no need to loop
            //check vision angle between police forward and hostile position
            _leeloo = _hostileArray[0].transform;

            if (Vector3.Angle(_carTransform.forward, LeeloDirection.normalized) > maxHostileAngle)
                //out of range, clear target
                _leeloo = null;

            _checkTimer.Activate(checkDelay);
        }

        private void CheckTarget()
        {
            //if leeloo is too far then we lost it
            if (LeeloDirection.sqrMagnitude > _sqrDist)
            {
                _leeloo = null;
                ValueFactor = 1f;//reset acceleration to 1
            }

            _checkTimer.Activate(checkDelay);
        }
    }
}