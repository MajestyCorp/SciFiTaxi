using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi.Tools
{
    public class Timer
    {
        public bool IsActive { get { return Time.time < _timeEnd; } }
        public bool IsFinished { get { return Time.time >= _timeEnd; } }
        public float Progress { get { return CalcProgress(); } }

        private float _timeEnd = 0f, _timeStart = 0f;

        public void Activate(float timeAmount)
        {
            _timeStart = Time.time;
            _timeEnd = _timeStart + timeAmount;
        }

        public void Activate(float timeAmount, float progress = 0f)
        {
            _timeStart = Time.time - timeAmount * progress;
            _timeEnd = _timeStart + timeAmount;
        }

        private float CalcProgress()
        {
            if (IsFinished)
                return 1f;
            else
                return (Time.time - _timeStart) / (_timeEnd - _timeStart);
        }
    }
}
