using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#if NET40 || NETSTANDARD1_1 || NETSTANDARD2_0 || PROFILE_111
using System.Dynamic;
#endif

namespace AutoMapper.MultiTargetMapping
{

    /// <summary>
    /// A simple multi-target mapping enhancement for AutoMapper.
    /// </summary>
    public static class MultiTargetMapper
    {

#if NET471 || NETSTANDARD1_1 || NETSTANDARD2_0 || PROFILE_111
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static object MapInternal(object source, Type tupleType)
        {
            var itemTypes =
#if NETSTANDARD1_1 || PROFILE_111
                tupleType.GenericTypeArguments;
#else
                tupleType.GetGenericArguments();
#endif
            var values = itemTypes
                .Select(x => Mapper.Map(source, source.GetType(), x))
                .ToArray();
            return Activator.CreateInstance(tupleType, args: values);
        }

        /// <summary>
        /// Map the given object to a Tuple object so that you can do a multi-target mapping.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tupleType"></param>
        /// <returns></returns>
        public static object Map(object source, Type tupleType)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (tupleType == null)
            {
                throw new ArgumentNullException(nameof(tupleType));
            }
            if (!tupleType.IsTuple())
            {
                throw new InvalidOperationException($"Type {tupleType.FullName} is not a Tuple type.");
            }
            return MapInternal(source, tupleType);
        }

        /// <summary>
        /// Map the given object to a Tuple object so that you can do a multi-target mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Map<T>(object source)
#if NET471
            where T : ITuple
#endif
        {
#if NET471
            // Since generic constraint is available on this platform, we can skip the Tuple type check.
            return (T)MapInternal(source, typeof(T));
#else
            return (T)Map(source, typeof(T));
#endif
        }

        /// <summary>
        /// General purpose multi-target mapping function.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="itemTypes"></param>
        /// <returns></returns>
#if NET471 || NETSTANDARD1_1 || NETSTANDARD2_0 || PROFILE_111
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IList<object> Map(object source, params Type[] itemTypes)
        {
            var destinations = itemTypes
                .Select(x => Mapper.Map(source, source.GetType(), x))
                .ToList();
            return destinations;
        }

#if NET40 || NETSTANDARD1_1 || NETSTANDARD2_0 || PROFILE_111
        /// <summary>
        /// Map the given object to a dynamic object so that you can do a multi-target mapping.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="itemTypes"></param>
        /// <returns></returns>
        public static dynamic MapDynamic(object source, params Type[] itemTypes)
        {
            var dict = itemTypes.Select((itemType, i) => new
            {
                Key = $"Item{i + 1}",
                Value = Mapper.Map(source, source.GetType(), itemType)
            }).ToDictionary(x => x.Key, x => x.Value);
            return (dynamic)dict.Aggregate(
                new ExpandoObject() as IDictionary<string, object>, (a, p) =>
                {
                    a.Add(p.Key, p.Value);
                    return a;
                });
        }
#endif

    }

}
