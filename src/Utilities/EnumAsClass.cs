using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utilities
{
    /// <summary>
    /// Enumeration as a Class. Also known as Value Object.
    /// </summary>
    public abstract class EnumAsClass : IComparable
    {
        /// <summary>
        /// Method to Execute when there is no match result in parsing to type
        /// </summary>
        protected Action ExecuteWhenNoMatch { get; set; }

        /// <summary>
        /// Return same result as no match when multiple result return when parsing
        /// if false, the 1st of the multiple result will return
        /// </summary>
        protected bool MultipleResultSameAsNoMatch { get; set; }

        /// <summary>
        /// Method to call to get the result when no match occurs
        /// </summary>
        protected Func<EnumAsClass> NoMatchResult { get; set; }

        protected EnumAsClass(int index, string name)
        {
            Index = index;
            Name = name;

            NoMatchResult = () => null;
            MultipleResultSameAsNoMatch = false;
            ExecuteWhenNoMatch = null;
        }

        public int Index { get; }

        public string Name { get; }

        /// <summary>
        /// Get all possible values of EnumAsClass type
        /// </summary>
        public static IEnumerable<T> GetAll<T>() where T : EnumAsClass
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
        public static IEnumerable<T> AsBitFlags<T>(uint index) where T : EnumAsClass => GetAll<T>()
            .Where(enumeration => enumeration > 0 && enumeration == (index & enumeration));

        /// <summary> Convert int to {T} EnumAsClass type </summary>
        public static T FromIndex<T>(int index) where T : EnumAsClass => Parse<T>(item => item.Index == index);

        /// <summary> Convert string to {T} EnumAsClass type </summary>
        public static T FromName<T>(string name) where T : EnumAsClass
        {
            if (name == null) name = string.Empty;
            return Parse<T>(item => name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        private static T Parse<T>(Func<T, bool> predicate) where T : EnumAsClass {
            var allItems = GetAll<T>().ToList();
            var matchingItems = allItems.Where(predicate).ToList();

            if (matchingItems.Any() ){
                if (matchingItems.Count == 1) return matchingItems.First();

                if (matchingItems.Count > 1 && !matchingItems.First().MultipleResultSameAsNoMatch) {
                    return matchingItems.First();
                }
            }

            allItems.First().ExecuteWhenNoMatch?.Invoke();
            return (T) allItems.First().NoMatchResult();
            
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EnumAsClass otherValue)) return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Index.Equals(otherValue.Index) && Name.Equals(otherValue.Name,StringComparison.InvariantCultureIgnoreCase);

            return typeMatches && valueMatches;
        }

        public override string ToString() => Name;

        public override int GetHashCode() => Index.GetHashCode();

        public int CompareTo(object other) => Index.CompareTo(((EnumAsClass) other).Index);

        public static implicit operator int(EnumAsClass enumAsClass) => enumAsClass.Index;
        public static implicit operator string(EnumAsClass enumAsClass) => enumAsClass.Name;
        public static bool operator ==(EnumAsClass left, EnumAsClass right) => Equals(right, left);
        public static bool operator !=(EnumAsClass left, EnumAsClass right) => !(left == right);

        public static bool operator <(EnumAsClass left, EnumAsClass right) =>
            left is null ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        public static bool operator <=(EnumAsClass left, EnumAsClass right) => 
            left is null || left.CompareTo(right) <= 0;

        public static bool operator >(EnumAsClass left, EnumAsClass right) =>
            left != null && left.CompareTo(right) > 0;

        public static bool operator >=(EnumAsClass left, EnumAsClass right) =>
            left is null ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;

    }
}
