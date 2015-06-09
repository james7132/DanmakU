// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers
{
    [System.Serializable]
    public class ColorChangeController : IDanmakuController
    {
        //TODO Document

        [SerializeField, Show] private Gradient _colorGradient;

        public Gradient ColorGradient
        {
            get { return _colorGradient; }
            set { _colorGradient = value; }
        }

        [SerializeField, Show] private float _startTime;

        public float StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        [SerializeField, Show] private float _endTime;

        public float EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        #region IDanmakuController implementation

        /// <summary>
        /// Updates the Danmaku controlled by the controller instance.
        /// </summary>
        /// <param name="danmaku">the bullet to update.</param>
        /// <param name="dt">the change in time since the last update</param>
        public void Update(Danmaku danmaku, float dt)
        {
            Gradient gradient = ColorGradient;
            if (gradient == null)
                return;

            float start = StartTime;
            float end = EndTime;
            float bulletTime = danmaku.Time;
            Color endColor = gradient.Evaluate(1f);

            if (bulletTime < start)
            {
                danmaku.Color = danmaku.Prefab.Color;
            }
            else if (bulletTime > end)
                danmaku.Color = endColor;
            else
            {
                if (EndTime <= start)
                    danmaku.Color = endColor;
                else
                    danmaku.Color = gradient.Evaluate((bulletTime - start)/(end - start));
            }
        }

        #endregion
    }
}