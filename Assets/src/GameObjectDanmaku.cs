using UnityEngine;
using UnityEngine.Assertions;

namespace DanmakU {

    public class GameObjectDanamku : IDanmaku {

        public int Id { get; set; }

        Vector2 position;
        public Vector2 Position {
            get { return position; }
            set { 
                position = value;
                transform.position = value; 
            }
        }

        float rotation;
        public float Rotation {
            get { return rotation; }
            set {
                rotation = value;
                var euler = transform.eulerAngles;
                euler.z = value * Mathf.Rad2Deg - 90f;
                transform.eulerAngles = euler;
            }
        }

        public float Speed { get; set; }
        public float AngularVelocity { get; set; }

        readonly GameObject gameObject;
        readonly Transform transform;

        public GameObjectDanamku(GameObject obj) {
            Assert.IsNotNull(obj);
            gameObject = obj;
            transform = obj.transform;
            position = transform.position;
            rotation = transform.eulerAngles.z;
        }

        public void SetActive(bool active) {
            gameObject.SetActive(active);
        }

    }

}
