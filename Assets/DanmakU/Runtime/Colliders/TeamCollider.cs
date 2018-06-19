using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU
{
    public class TeamCollider : MonoBehaviour
    {
        public DanmakuCollider Collider;
        public int TeamNo;

        //todo: make a thorough pass focusing on accessibility
        //todo: thorough update to danmaku

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if (Collider != null)
            {
                Debug.Log("Subscribed");
                Collider.OnDanmakuCollision += OnDanmakuCollision;
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            if (Collider != null)
            {
                Collider.OnDanmakuCollision -= OnDanmakuCollision;
            }
        }

        void getHit()
        {
            gameObject.SetActive(false);
        }


        void OnDanmakuCollision(DanmakuCollisionList collisions)
        {
            //if (emitter)
            foreach (var collision in collisions)
            {
                if (collision.Danmaku.Pool.TeamNo != TeamNo)
                {
                    collision.Danmaku.Destroy();
                    getHit();
                }
            }
        }

        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset()
        {
            Collider = GetComponent<DanmakuCollider>();
        }
    }
}