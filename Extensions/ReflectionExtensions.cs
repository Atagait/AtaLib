using System;
using System.Reflection;

namespace AtaLib.ReflectionExtensions
{
    public static class reflectionExtensions
    {
        /// <summary>
        /// Has wraps GetCustomAttribute to simplify checking for custom attributes.
        /// </summary>
        /// <typeparam name="A">The attribute you want to get</typeparam>
        /// <returns>Whether or not an attribute was found</returns>
        public static bool Has<T>(this MemberInfo member) where T : Attribute => member.Has<T>(out _);
        /// <summary>
        /// Has wraps GetCustomAttribute to simplify fetching custom attributes.
        /// It primarily removes the need to check for null in code.
        /// </summary>
        /// <typeparam name="A">The attribute you want to get</typeparam>
        /// <param name="aAttribute">The output of the attribute</param>
        /// <returns>Whether or not an attribute was found</returns>
        public static bool Has<T>(this MemberInfo member, out T foundAttribute) where T : Attribute
        {
            foundAttribute = member.GetCustomAttribute<T>();
            if (foundAttribute != null)
                return true;
            return false;
        }

        /// <summary>
        /// Has wraps GetCustomAttribute to simplify checking for custom attributes.
        /// </summary>
        /// <typeparam name="A">The attribute you want to get</typeparam>
        /// <returns>Whether or not an attribute was found</returns>
        public static bool Has<T>(this Type type) where T : Attribute => type.Has<T>(out _);
        /// <summary>
        /// Has wraps GetCustomAttribute to simplify fetching custom attributes.
        /// It primarily removes the need to check for null in code.
        /// </summary>
        /// <typeparam name="A">The attribute you want to get</typeparam>
        /// <param name="aAttribute">The output of the attribute</param>
        /// <returns>Whether or not an attribute was found</returns>
        public static bool Has<T>(this Type type, out T foundAttribute) where T : Attribute
        {
            foundAttribute = type.GetCustomAttribute<T>();
            if (foundAttribute != null)
                return true;
            return false;
        }
    }
}