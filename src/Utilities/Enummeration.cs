using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utilities
{
    /// <summary>
    /// Enumeration as a Class. Also known as Value Object.
    /// </summary>
    public abstract class EnumAsClass<T> : Enummeration, IComparable where T : Enummeration
    {
        private readonly bool _noIndex;
        /// <summary>
        /// Method to call to get the result when no match occurs
        /// </summary>
        protected static Func<T> Default { get; set; }

        protected EnumAsClass(string name): base(name)
        {
            _noIndex = true;
            Default = () => (T) null;
        }

        protected EnumAsClass(int index, string name) : this(name)
        {
            _noIndex = false;
            var test = GetAll();
        }

        /// <summary>
        /// Get all possible values of EnumAsClass type
        /// </summary>
        public static IEnumerable<T> GetAll() 
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                if (info.GetValue(null) is T locatedValue) yield return locatedValue; 
            }
        }

        /// <summary>
        /// Pass Value to get a list of EnumAsClass. Assumed the EnumAsClass set index as powers of 2.
        /// EnumAsClass returned will ignore negative and zero value.
        /// </summary>
        public static IEnumerable<T> AsBitFlags(uint index) => GetAll()
            .Where(enumeration => enumeration > 0 && enumeration == (index & enumeration));

        /// <summary> Convert int to {T} EnumAsClass type </summary>
        protected static T FromIndex(int index)  => Parse(item => item.Index == index);

        /// <summary> Convert string to {T} EnumAsClass type </summary>
        protected static T FromName(string name)
        {
            if (name == null) name = string.Empty;
            return Parse(item => name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        private static T Parse(Func<T, bool> predicate) 
        {
            var matching = GetAll().SingleOrDefault(predicate);
            return matching ?? Default();
        }

    }

    public class Enummeration
    {
        protected Enummeration(string name)
        {
            Name = name;
        }

        protected Enummeration(int index, string name) : this(name)
        {
            Index = index;
        }

        public int Index { get; }

        public string Name { get; }

        public static implicit operator int(Enummeration enumAsClass) => enumAsClass.Index;
        public static implicit operator string(Enummeration enumAsClass) => enumAsClass.Name;


        public override string ToString() => Name;

        public override int GetHashCode() => Index.GetHashCode() ^ Name.GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is Enummeration otherValue)) return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Index.Equals(otherValue.Index) && Name.Equals(otherValue.Name, StringComparison.InvariantCultureIgnoreCase);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Index.CompareTo(((Enummeration)other).Index);

        public static bool operator ==(Enummeration left, Enummeration right) => Equals(right, left);
        public static bool operator !=(Enummeration left, Enummeration right) => !(left == right);

        public static bool operator <(Enummeration left, Enummeration right) =>
            left is null ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        public static bool operator <=(Enummeration left, Enummeration right) =>
            left is null || left.CompareTo(right) <= 0;

        public static bool operator >(Enummeration left, Enummeration right) =>
            left != null && left.CompareTo(right) > 0;

        public static bool operator >=(Enummeration left, Enummeration right) =>
            left is null ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
