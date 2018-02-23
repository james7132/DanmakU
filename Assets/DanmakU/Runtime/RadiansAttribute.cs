using System;
using UnityEngine;

namespace DanmakU {

/// <summary>
/// A PropertyAttribute to automatically allow saving radian values while exposing
/// a field in degrees.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class RadiansAttribute : PropertyAttribute {
}

}