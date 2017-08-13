using System.Linq;
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

        Color? color;
        public Color? Color {
            get { return color; }
            set {
                var block = new MaterialPropertyBlock();
                block.SetColor("_Color", value.HasValue ? value.Value : UnityEngine.Color.white);
                foreach (var renderer in renderers)
                    renderer.SetPropertyBlock(block);
            }
        }

        public float Speed { get; set; }
        public float AngularVelocity { get; set; }

        readonly GameObject gameObject;
        readonly Transform transform;
        readonly Renderer[] renderers;
        readonly Color[] defaultColors;

        public GameObjectDanamku(GameObject obj) {
            Assert.IsNotNull(obj);
            gameObject = obj;
            transform = obj.transform;
            position = transform.position;
            rotation = transform.eulerAngles.z;
            renderers = obj.GetComponentsInChildren<Renderer>();
        }

        public void SetActive(bool active) {
            gameObject.SetActive(active);
        }

    }

}
