using System;

namespace libplctag
{
    public class ByteOrder
    {
        public int NumberOfBytes => _value.Length;

        string _value;
        ByteOrder(string value)
        {
            foreach (var character in value)
                if (!int.TryParse(character.ToString(), out _))
                    throw new ArgumentOutOfRangeException();
            _value = value;
        }

        public static ByteOrder Create(string order)
        {
            return new ByteOrder(order);
        }

        public override string ToString() => _value;
    }
}