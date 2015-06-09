// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers
{
    /// <summary>
    /// A Danmaku Controller that makes Danmaku speed up or slow down over time.
    /// </summary>
    public class AccelerationController : IDanmakuController
    {
        //TODO Document

        [SerializeField, Show] private float _acceleration;

        public float Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakU.Controllers.AccelerationController"/> class.
        /// </summary>
        /// <param name="acceleration">Acceleration.</param>
        public AccelerationController(float acceleration = 0f) : base()
        {
            _acceleration = acceleration;
        }

        #region IDanmakuController implementation

        /// <summary>
        /// Updates the Danmaku controlled by the controller instance.
        /// </summary>
        /// <param name="danmaku">the bullet to update.</param>
        /// <param name="dt">the change in time since the last update</param>
        public virtual void Update(Danmaku danmaku, float dt)
        {
            if (Math.Abs(_acceleration) > float.Epsilon)
            {
                danmaku.Speed += _acceleration*dt;
            }
        }

        #endregion
    }
}