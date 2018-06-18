using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU
{
    public class TeamCollider : MonoBehaviour
    {
        public DanmakuCollider Collider;
        DanmakuEmitter emitter;

        //todo: make a thorough pass focusing on accessibility
        //todo: thorough update to danmaku

        ///How much time this unit has left invulnerable
        //private float invulnTimer = 0f;
        //How much health this unit is capable of at perfect condition
        public int maxHealth;

        //How much health this unit currently contains
        private int curHealth;

        void OnStart()
        {
            curHealth = maxHealth;
        }

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

            emitter = GetComponent<DanmakuEmitter>();
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


        void takeDamage(int amount_damage)
        {
            curHealth -= amount_damage;
        }

        // Update is called once per frame
        // void Update()
        // {
        /*if (invulnTimer > 0f)
        {
          invulnTimer -= Time.deltaTime;
          if (invulnTimer < 0f)
          {
            invulnTimer = 0f;
            //sprite.color = Color.white;
          }
        }*/
        // }

        void OnDanmakuCollision(DanmakuCollisionList collisions)
        {
            //if (emitter)
            foreach (var collision in collisions)
            {
                if (collision.Danmaku.Pool.TeamNo != emitter.set.Pool.TeamNo)
                {
                    collision.Danmaku.Destroy();
                    takeDamage(collision.Danmaku.Pool.Damage);
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