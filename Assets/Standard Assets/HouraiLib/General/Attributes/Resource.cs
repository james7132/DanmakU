using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hourai {

    [Serializable]
    public class Resource<T> where T : Object {

        private readonly string _path;
        private T _asset;

        public Resource(string path) {
            if (path == null)
                throw new ArgumentNullException("path");
            _path = path;
        }

        public string Path {
            get { return _path; }
        }

        public bool IsLoaded {
            get { return _asset != null; }
        }

        public T Asset {
            get { return _asset; }
        }

        public T Load() {
            if (_asset != null)
                return _asset;
            Object loadedObject = Resources.Load(_path);
            if (loadedObject != null) {
                var asT = loadedObject as T;
                if (asT != null)
                    _asset = asT;
                else {
                    Debug.LogError("Tried to load asset of type" + typeof (T) + " at " + _path +
                                   " and found an Object of type " + loadedObject.GetType() + " instead");
                }
                return _asset;
            }
            Debug.LogError("Tried to load asset of type" + typeof (T) + " at " + _path + " and found nothing.");
            return null;
        }

        public virtual void Unload() {
            if (IsLoaded)
                Resources.UnloadAsset(_asset);
            _asset = null;
        }

    }

}