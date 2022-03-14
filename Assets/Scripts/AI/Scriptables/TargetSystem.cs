using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    [CreateAssetMenu(fileName = "Target System", menuName = "Movement AI/Target System")]
    public class TargetSystem : MovementSO
    {

        [Header("Target system")]
        [SerializeField]
        private LayerMask targetLayer;
        [SerializeField]
        private float targetRadius = 12f;
        [SerializeField]
        private float minReachDistance = 0.2f;

        protected CarAI _carAI = null;
        protected Vector3 _targetPosition;
        protected Transform _target = null, _prevTarget = null, _tempTarget;
        protected Transform _carTransform = null;
        protected float _sqrMinReachDist;

        public override void InitSO(CarAI carAI)
        {
            base.InitSO(carAI);
            _sqrMinReachDist = minReachDistance * minReachDistance;
        }

        public override void Initialize(CarAI carAI)
        {
            base.Initialize(carAI);

            _target = _prevTarget = null;

            _carAI = carAI;
            _carTransform = carAI.transform;
            ValueFactor = 1f;
        }

        public override Vector3 CalcMoveVector()
        {
            if(ReachedTarget())
            {
                //check if chunk at target position exist - proceed navigation
                if(_target!=null && !CityGenerator.Instance.ChunkExistAtPos(_targetPosition))
                {
                    _carAI.DisableCar();
                    return Vector3.zero;
                }

                if (_target == null)
                    _tempTarget = GetNearestTarget(_carTransform.position);
                else
                    _tempTarget = GetNewTarget(_carTransform.position, _target, _prevTarget);

                //swap old target
                _prevTarget = _target;
                _target = _tempTarget;
            }

            //calculate movement vector
            if (_target != null)
                return (_targetPosition - _carTransform.position).normalized;
            else
                return Vector3.zero;
        }

        public override void MyGizmos(Transform owner)
        {
            base.MyGizmos(owner);

            //draw min touch radius
            Gizmos.color = Color.red;

            if (_target != null)
                Gizmos.DrawLine(owner.position, _targetPosition);
        }

        private void InitCarTransform()
        {
            Vector3 v;
            //set same height as target position
            v = _carTransform.position;
            v.y = _targetPosition.y;
            _carTransform.position = v;

            //set initial rotation to target
            _carTransform.rotation = Quaternion.LookRotation(_targetPosition - _carTransform.position, _carTransform.up);
        }

        private bool ReachedTarget()
        {
            //dont count Y position when check target destination
            _targetPosition.y = _carTransform.position.y;
            return _target==null || (_carTransform.position - _targetPosition).sqrMagnitude <= _sqrMinReachDist;
        }

        private Transform GetNearestTarget(Vector3 currentPosition)
        {
            Collider[] colliders;
            float f, minSqr = float.MaxValue;
            Transform closestTarget = null;

            //get colliders on whole map to find closest
            colliders = Physics.OverlapSphere(currentPosition, 
                CityGenerator.Instance.ChunkWidth * CityGenerator.Instance.GenerationRadius, targetLayer);

            if (colliders == null || colliders.Length == 0)
                return null;

            //start selection of random target
            for (int i = 0; i < colliders.Length; i++)
            {
                f = (colliders[i].transform.position - currentPosition).sqrMagnitude;
                if (f < minSqr)
                {
                    closestTarget = colliders[i].transform;
                    minSqr = f;
                }
            }

            if(closestTarget != null)
            {
                _targetPosition = closestTarget.position;
                InitCarTransform();
            }

            return closestTarget;
        }

        private Transform GetNewTarget(Vector3 currentPosition, Transform curTarget, Transform prevTarget)
        {
            Collider[] colliders;
            int index;

            colliders = Physics.OverlapSphere(currentPosition, targetRadius, targetLayer);

            if (colliders == null || colliders.Length == 0)
                return null;

            //select random point
            //but exclude current point and previous point where we came from
            //        *
            //    *  Excl  *
            //       Excl

            index = colliders.Length;
            if (curTarget != null)
                index--;
            if (prevTarget != null)
                index--;

            index = Random.Range(0, index - 1);

            //start selection of random target
            for(int i=0;i<colliders.Length;i++)
            {
                if (index == 0)
                {
                    //save position as vector
                    _targetPosition = colliders[i].transform.position;
                    //_targetPosition.y = _carTransform.position.y;
                    return colliders[i].transform;
                }
                else if (colliders[i].transform != curTarget && colliders[i].transform != prevTarget)
                    index--;
            }

            //by default return prev target
            //save position as vector
            _targetPosition = prevTarget.position;
            //_targetPosition.y = _carTransform.position.y;
            return prevTarget;
        }
    }
}