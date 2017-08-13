using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace DanmakU {

    public class DanmakuPool {

        class DanmakuElement {
            public DanmakuElement Previous;
            public DanmakuElement Next;
            public IDanmaku Danmaku;
        }

        DanmakuElement[] _allDanmaku;
        DanmakuElement _activeHead;
        DanmakuElement _inactiveHead;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<IDanmaku> GetActive() {
            var current = _activeHead;
            while (current != null) {
                yield return current.Danmaku;
                current = current.Next;
            }
        }

        public int ActiveCount { get; set;}

        public DanmakuPool(int size, Func<IDanmaku> danmakuFactory) {
            Assert.IsTrue(size > 0);
            Assert.IsNotNull(danmakuFactory);
            var _allDanmaku = new DanmakuElement[size];
            DanmakuElement last = null;
            for (var i = 0; i < _allDanmaku.Length; i++) {
                var current = new DanmakuElement {
                    Danmaku = danmakuFactory(),
                    Previous = last
                };
                current.Danmaku.Id = i;
                current.Danmaku.SetActive(false);
                if (last != null)
                    last.Next = current;
                _allDanmaku[i] = current;
                last = current;
            }
            ActiveCount = 0;
            _activeHead = null;
            _inactiveHead = _allDanmaku[0];
        }

        public IDanmaku Get() {
            var danmakuElement = _inactiveHead;
            if (danmakuElement == null)
                throw new InvalidOperationException("No more available danmaku! You've exhausted the entire pool!");
            _inactiveHead = danmakuElement.Next;
            if (_inactiveHead != null)
                _inactiveHead.Previous = null;
            danmakuElement.Next = _activeHead;
            danmakuElement.Previous = null;
            _activeHead = danmakuElement;
            Assert.IsNotNull(danmakuElement.Danmaku);
            danmakuElement.Danmaku.SetActive(true);
            ActiveCount++;
            return danmakuElement.Danmaku;
        }

        public void Return(IDanmaku element) {
            Assert.IsNotNull(element);
            var danmakuElement = _allDanmaku[element.Id];
            var prev = danmakuElement.Previous;
            var next = danmakuElement.Next;
            if (prev != null)
                prev.Next = next;
            if (next != null)
                next.Previous = prev;
            if (danmakuElement == _activeHead)
                _activeHead = next;
            danmakuElement.Next = _inactiveHead;
            danmakuElement.Previous = null;
            ActiveCount--;
            _inactiveHead = danmakuElement;
        }

    }

}