using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU.Fireables {

  /// <summary>
  /// Shot a pain to the scene
  /// </summary>
  /// <remarks>
  /// Read the text from the _paintData. 
  /// Ignore the line that start with the '#'.
  /// In a valid line, the '*' means bullet, otherwise the space.   
  /// </remarks>
  [Serializable]
  public class Paint : Fireable {
    /// <summary>
    /// Paint Data. Load from the asset.
    /// </summary>
    public TextAsset PaintData;

    /// <summary>
    /// Shot center
    /// </summary>
    [Radians] public Range CenterAngle;

    /// <summary>
    /// Angle between each bullet
    /// </summary>
    [Radians] public Range ShiftAngle;

    /// <summary>
    /// Delay time in seconds between each shot
    /// </summary>
    public Range Delay;

    public override void Fire (DanmakuConfig config) {
      var emitter = new GameObject ("PaintEmitter", typeof (PaintEmitter)).GetComponent<PaintEmitter> ();
      emitter.SetEmitParams (PaintData, CenterAngle.GetValue (), ShiftAngle.GetValue (), Delay.GetValue ());
      emitter.Emit (config, Child);
    }

    internal class PaintEmitter : MonoBehaviour {
      private static readonly string[] SPLIT_VAL = { "\n", "\r", "\r\n" };

      private TextAsset _paintData;

      private float _centerAngle;

      private float _shiftAngle;

      private float _delay;

      private DanmakuConfig _config;

      private IFireable _subfire;

      private bool _emit = false;

      public void SetEmitParams (TextAsset paintData, float centerAngle, float shiftAngle, float delay) {
        _paintData = paintData;
        _centerAngle = centerAngle;
        _shiftAngle = shiftAngle;
        _delay = delay;
      }

      public void Emit (DanmakuConfig config, IFireable subfire) {
        _config = config;
        _subfire = subfire;

        if (!_emit) {
          StartCoroutine (EmitCoroutine ());
          _emit = true;
        }
      }

      private IEnumerator EmitCoroutine () {
        float timer = 0;
        float startAngle;
        List<List<bool>> shotData = ParseText ();

        if (shotData == null || shotData.Count == 0) {
          yield break;
        }

        if (shotData[0].Count % 2 == 0) {
          startAngle = _centerAngle - (shotData[0].Count / 2 + 0.5f) * _shiftAngle;
        } else {
          startAngle = _centerAngle - (shotData[0].Count / 2) * _shiftAngle;
        }

        for (int i = 0; i < shotData.Count; i++) {
          timer = 0f;
          while (timer <= _delay) {
            timer += Time.deltaTime;
            yield return null;
          }

          for (int j = 0; j < shotData[i].Count; j++) {
            if (shotData[i][j]) {
              _subfire.Fire (new DanmakuConfig {
                Position = _config.Position,
                  Rotation = startAngle + _shiftAngle * j,
                  Speed = _config.Speed,
                  AngularSpeed = _config.AngularSpeed,
                  Color = _config.Color
              });
            }
          }
        }

        Destroy (this.gameObject);
      }

      /// <summary>
      /// Parse the paint text
      /// </summary>
      /// <remarks>
      /// Read the text from the _paintData. 
      /// Ignore the line that start with the '#';
      /// In a valid line, the '*' means bullet, otherwise the space.   
      /// </remarks>
      /// <return>
      /// Return the parse result. Each list represent a line of shot bullets.
      /// If the value is true, means a bullet will be shot.
      /// If the value is false, means the bullet in that place will be ignored.
      /// </return>
      private List<List<bool>> ParseText () {
        var result = new List<List<bool>> ();

        if (string.IsNullOrEmpty (_paintData.text)) {
          return null;
        }

        var lines = _paintData.text.Split (SPLIT_VAL, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines) {
          if (line.StartsWith ("#")) {
            continue;
          }

          int index = result.Count;
          result.Add (new List<bool> ());

          for (int i = 0; i < line.Length; i++) {
            result[index].Add (line[i] == '*');
          }
        }

        result.Reverse ();

        return result;
      }
    }
  }

}