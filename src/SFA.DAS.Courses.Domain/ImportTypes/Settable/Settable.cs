using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Domain.ImportTypes.Settable
{
    [JsonConverter(typeof(SettableJsonConverter<>))]
    public class Settable<T>
    {
        private T _value;
        private bool _isSet;
        private bool _hasValue;
        private readonly object _invalidValue;
        public static Settable<T> Undefined => new Settable<T>();

        public Settable()
        {
            _isSet = false;
            _hasValue = false;
            _value = default;
            _invalidValue = null;
        }

        public Settable(T value)
        {
            _isSet = true;
            _hasValue = typeof(T).IsValueType || !EqualityComparer<T>.Default.Equals(value, default);
            _value = value;
            _invalidValue = null;
        }

        private Settable(object invalidValue)
        {
            _isSet = true;
            _hasValue = false;
            _value = default;
            _invalidValue = invalidValue;
        }

        public static Settable<T> FromInvalidValue(object invalidValue)
        {
            return new Settable<T>(invalidValue);
        }

        public Settable<T> Clone()
        {
            if (!_isSet) return Undefined;
            if (_hasValue) return new Settable<T>(_value);
            return FromInvalidValue(_invalidValue);
        }


        public bool IsSet => _isSet;
        public bool HasValue => _isSet && _hasValue;
        public bool HasInvalidValue => _invalidValue != null; 
        public object InvalidValue => _invalidValue;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _isSet = true;
                _hasValue = typeof(T).IsValueType || !EqualityComparer<T>.Default.Equals(value, default);
                _value = value;
            }
        }

        public static implicit operator Settable<T>(T value) => new(value);
        public static implicit operator T(Settable<T> value) => value != null && value.HasValue ? value._value : default;

        public override bool Equals(object obj) =>
            obj is Settable<T> other && this == other;

        public static bool operator ==(Settable<T> t1, Settable<T> t2)
        {
            if (ReferenceEquals(t1, t2))
                return true;

            if (t1 is null || t2 is null)
                return false;

            return
                t1.IsSet == t2.IsSet &&
                t1.HasValue == t2.HasValue &&
                t1.HasInvalidValue == t2.HasInvalidValue &&
                (!t1.HasValue || EqualityComparer<T>.Default.Equals(t1.Value, t2.Value)) &&
                (!t1.HasInvalidValue || Equals(t1.InvalidValue, t2.InvalidValue));
        }


        public static bool operator !=(Settable<T> t1, Settable<T> t2) => !(t1 == t2);

        public override int GetHashCode()
        {
            if (!_isSet)
            {
                return -1;
            }

            if (_hasValue)
            {
                return _value?.GetHashCode() ?? 0;
            }

            if (_invalidValue != null)
            {
                return _invalidValue.GetHashCode();
            }

            return 0;
        }


        public override string ToString()
        {
            if (!_isSet)
            {
                return "undefined";
            }

            if (_hasValue)
            {
                return _value?.ToString() ?? "null";
            }

            if (_invalidValue != null)
            {
                return _invalidValue.ToString();
            }

            return "null";
        }
    }
}
