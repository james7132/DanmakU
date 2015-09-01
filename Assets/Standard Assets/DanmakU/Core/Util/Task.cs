// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;

namespace DanmakU {

    public sealed class Task {

        private readonly IEnumerator _task;
        private readonly MonoBehaviour _context;
        private Coroutine _coroutine;

        internal Task(MonoBehaviour context, IEnumerator task) {
            _context = context;
            _task = task;
        }

        internal void Start() {
            if (_context)
                _coroutine = _context.StartCoroutine(_task);
        }

        public void Stop() {
            if(_context)
                _context.StopCoroutine(_task);
        }

        public static implicit operator Coroutine(Task task) {
            return task != null ? task._coroutine : null;
        }

    }

}