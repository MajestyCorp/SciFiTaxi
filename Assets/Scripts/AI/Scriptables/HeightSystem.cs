using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    [CreateAssetMenu(fileName = "Height System", menuName = "Movement AI/Height System")]
    public class HeightSystem : MovementSO
    {
        [SerializeField]
        private float minHeight = 3f;
        [SerializeField]
        private float maxHeight = 8f;

        private float _height;
        private Transform _carTransform;
        private Vector3 v;

        public override void Initialize(CarAI carAI)
        {
            base.Initialize(carAI);

            _carTransform = carAI.transform;
            _height = Random.Range(minHeight, maxHeight);
        }

        public override Vector3 CalcMoveVector()
        {
            //calc current height
            v = _carTransform.position;
            v.y = _height;

            return (v - _carTransform.position).normalized;
        }
    }
}