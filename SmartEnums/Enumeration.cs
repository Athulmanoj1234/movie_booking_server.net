using System.Reflection;

namespace movie_booking.SmartEnums
{
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {
        protected Enumeration(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        //we are creating dictionary to store values for the strongly typed enums
        public static readonly Dictionary<string, TEnum> Enumerations = CreateEnumerations();

        public string Key { get; protected init; } // this Value can be only initialized in same class or derived class that is why it is being protected init
        public string Value { get; protected init; }


        public bool Equals(Enumeration<TEnum> other)
        {
            if (other is null)
            {
                return false;
            }

            return GetType() == other.GetType() && Key == other.Key;
        }

        public override bool Equals(object? obj) // this overide equals method will gets exeuted first and internally calls real equal method
        {
            return obj is Enumeration<TEnum> other && this.Equals(other);
        }

        // we are getting enumeration(credit card) by the help of value and name

        public static TEnum FromKey(string Key)
        {
            return Enumerations.TryGetValue(Key, out TEnum? enumeration) ? enumeration : default;
        }

        public static TEnum FromValue(string Value)
        {
            return Enumerations.Values.SingleOrDefault(value => value.Value == Value);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        private static Dictionary<string, TEnum> CreateEnumerations()
        {

            var enumerationType = typeof(TEnum); // Gets the type information of the class.So now C# can inspect the class metadata.

            var fieldsForType = enumerationType
                .GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy) // This gets all fields that are: Public, Static and Includes inherited static fields too
                .Where(fieldInfo =>
                    enumerationType.IsAssignableFrom(fieldInfo.FieldType)) // This filters only fields whose type matches TEnum. ie if TEnum is credit card then this field is not taken - public static string Test = "Hello"; because this is only string
                .Select(fieldInfo =>
                    (TEnum)fieldInfo.GetValue(default)!); // it extracts the actual object: like new CreditCard(2, "Premium")

            // now we return dictionary with key as enumeration values
            return fieldsForType.ToDictionary(x => x.Key);
        }

    }
}
