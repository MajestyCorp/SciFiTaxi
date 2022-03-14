using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    public class MovementSO : ScriptableObject
    {
        public float ValueFactor { get; protected set; }
        public virtual void InitSO(CarAI carAI)
        { }

        public virtual void Initialize(CarAI carAI)
        { }

        public virtual Vector3 CalcMoveVector()
        {
            return Vector3.zero;
        }

        public virtual void MyGizmos(Transform owner)
        { }
    }
}