// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers
{
    [System.Serializable]
    public class SpeedLimitController : IDanmakuController
    {
        [SerializeField, Show] private float _minSpeed;

        public float MinSpeed
        {
            get { return _minSpeed; }
            set
            {
                _minSpeed = value;
                if (_minSpeed > _maxSpeed)
                {
                    float temp = _maxSpeed;
                    _maxSpeed = _minSpeed;
                    _minSpeed = temp;
                }
            }
        }

        [SerializeField, Show] private float _maxSpeed;

        public float MaxSpeed
        {
            get { return _maxSpeed; }
            set
            {
                _maxSpeed = value;
                if (_minSpeed > _maxSpeed)
                {
                    float temp = _maxSpeed;
                    _maxSpeed = _minSpeed;
                    _minSpeed = temp;
                }
            }
        }


        public SpeedLimitController()
        {
            _minSpeed = float.NegativeInfinity;
            _maxSpeed = float.PositiveInfinity;
        }

        public SpeedLimitController(float minimum,
            float maximum)
        {
            _minSpeed = minimum;
            _maxSpeed = maximum;
            if (_minSpeed > _maxSpeed)
            {
                float temp = _maxSpeed;
                _maxSpeed = _minSpeed;
                _minSpeed = temp;
            }
        }

        public SpeedLimitController(float value)
        {
            float absValue = Mathf.Abs(value);
            _minSpeed = -absValue;
            _maxSpeed = absValue;
        }

        public SpeedLimitController(float value, bool max)
        {
            if (max)
            {
                this._maxSpeed = value;
                _minSpeed = float.NegativeInfinity;
            }
            else
            {
                this._maxSpeed = float.PositiveInfinity;
                _minSpeed = value;
            }
        }

        #region IDanmakuController implementation

        /// <summary>
        /// Updates the Danmaku controlled by the controller instance.
        /// </summary>
        /// <param name="danmaku">the bullet to update.</param>
        /// <param name="dt">the change in time since the last update</param>
        public void Update(Danmaku danmaku, float dt)
        {
            float speed = danmaku.Speed;
            if (speed < _minSpeed)
                danmaku.Speed = _minSpeed;
            if (speed > _maxSpeed)
                danmaku.Speed = _maxSpeed;
        }

        #endregion
    }
}