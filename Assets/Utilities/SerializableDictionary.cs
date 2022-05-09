using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace TGP.Utilites
{
    [Serializable]
    public abstract class SerializableDictionary<TKey, Tvalue> : Dictionary<TKey, Tvalue>, ISerializationCallbackReceiver
    {
        protected abstract List<SerializableKeyValuePair<TKey,Tvalue>> _KeyPairValues { get; set; }

        public void OnAfterDeserialize()
        {
            this.Clear();

            for (int i = 0; i < _KeyPairValues.Count; i++)
            {
                this[_KeyPairValues[i].Key] = _KeyPairValues[i].Value;
            }
        }

        public void OnBeforeSerialize()
        {
            _KeyPairValues.Clear();
            foreach (KeyValuePair<TKey, Tvalue> pair in this)
            {
                _KeyPairValues.Add(new SerializableKeyValuePair<TKey, Tvalue>(pair.Key, pair.Value));
            }
        }
    }

    [Serializable]
    public class SerializableKeyValuePair<Tkey, TValue> : IEquatable<SerializableKeyValuePair<Tkey, TValue>>
    {
        [SerializeField]
        Tkey _key;
        public Tkey Key { get { return _key; } }

        [SerializeField]
        TValue _value;
        public TValue Value { get { return _value; } }

        public SerializableKeyValuePair()
        {

        }

        public SerializableKeyValuePair(Tkey key, TValue value)
        {
            this._key = key;
            this._value = value;
        }

        public bool Equals(SerializableKeyValuePair<Tkey, TValue> other)
        {
            var comparer1 = EqualityComparer<Tkey>.Default;
            var comparer2 = EqualityComparer<TValue>.Default;

            return comparer1.Equals(_key, other._key) &&
                comparer2.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            var comparer1 = EqualityComparer<Tkey>.Default;
            var comparer2 = EqualityComparer<TValue>.Default;

            int h0;
            h0 = comparer1.GetHashCode(_key);
            h0 = (h0 << 5) + h0 ^ comparer2.GetHashCode(_value);
            return h0;
        }

        public override string ToString()
        {
            return String.Format("(Key: {0}, Value: {1})", _key, _value);
        }
    }

    [Serializable]
    public class ColorGameobjectTuple : SerializableKeyValuePair<Color, GameObject>
    {
        public ColorGameobjectTuple(Color item1, GameObject item2) : base(item1, item2) { }
    }

    [Serializable]
    public class ColorAndGameObjectDictonary : SerializableDictionary<Color, GameObject>
    {
        [SerializeField] private List<ColorGameobjectTuple> _pairs = new List<ColorGameobjectTuple>();

        protected override List<SerializableKeyValuePair<Color, GameObject>> _KeyPairValues
        {
            get
            {
                var list = new List<SerializableKeyValuePair<Color, GameObject>>();
                foreach (var pair in _pairs)
                {
                    list.Add(new SerializableKeyValuePair<Color, GameObject>(pair.Key, pair.Value));
                }
                return list;
            }
            set
            {
                _pairs.Clear();
                foreach (var Kvp in value)
                {
                    _pairs.Add(new ColorGameobjectTuple(Kvp.Key, Kvp.Value));
                }
            }
        }

        public void AddIndex(Color color, GameObject gameObject)
        {
            _pairs.Add(new ColorGameobjectTuple(color, gameObject));
        }

        public void RemoveIndex(Color color, GameObject gameObject)
        {
            _pairs.Remove(new ColorGameobjectTuple(color, gameObject));
        }

        public bool Containscolor(Color color, GameObject gameObject)
        {
            return _pairs.Contains(new ColorGameobjectTuple(color, gameObject));
        }
    }

}
