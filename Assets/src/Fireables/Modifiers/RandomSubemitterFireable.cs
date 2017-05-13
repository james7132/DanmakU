using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    public class RandomSubemitterFireable : IFireable {

        public List<IFireable> Subemitters { get; set;}

        public RandomSubemitterFireable(IEnumerable<IFireable> subemitters) {
            Subemitters = new List<IFireable>(subemitters);
        }

        public void Fire(DanmakuInitialState state) {
            var subemitter = Subemitters[Mathf.FloorToInt(Random.value * Subemitters.Count)];
            if (subemitter != null)
                subemitter.Fire(state);
        }

    }

}