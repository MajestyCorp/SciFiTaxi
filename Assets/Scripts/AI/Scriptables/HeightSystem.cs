using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi.AI
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
        private Vector3 _newPosition;

        public override void Initialize(CarAI carAI)
        {
            base.Initialize(carAI);

            _carTransform = carAI.transform;
            _height = Random.Range(minHeight, maxHeight);
        }

        public override Vector3 CalcMoveVector()
        {
            //calc current height
            _newPosition = _carTransform.position;
            _newPosition.y = _height;

            return (_newPosition - _carTransform.position).normalized;
        }
    }
}