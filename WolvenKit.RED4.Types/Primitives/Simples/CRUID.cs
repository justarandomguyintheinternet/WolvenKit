using System;
using System.Diagnostics;

namespace WolvenKit.RED4.Types
{
    [RED("CRUID")]
    [DebuggerDisplay("{_value}", Type = "CRUID")]
    public readonly struct CRUID : IRedPrimitive<ulong>, IEquatable<CRUID>, IRedInteger
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ulong _value;

        private CRUID(ulong value)
        {
            _value = value;
        }

        public static implicit operator CRUID(ulong value) => new(value);
        public static implicit operator ulong(CRUID value) => value._value;


        public override int GetHashCode() => _value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is CRUID cObj)
            {
                return Equals(cObj);
            }

            return false;
        }

        public bool Equals(CRUID other) => Equals(_value, other._value);
    }
}