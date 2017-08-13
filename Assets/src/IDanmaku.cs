using UnityEngine;

namespace DanmakU {

    public interface IDanmaku {
        int Id { get; set; }

        Vector2 Position { get; set; }
        float Rotation { get; set; }
        float Speed { get; set; }
        float AngularVelocity { get; set; }

        Color? Color { get; set; }

        void SetActive(bool active);
    }

}