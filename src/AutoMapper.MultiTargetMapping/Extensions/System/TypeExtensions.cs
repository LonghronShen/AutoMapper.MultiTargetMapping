using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{

    internal static class TypeExtensions
    {

#if NET471 || NETSTANDARD1_1 || NETSTANDARD2_0 || PROFILE_111
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static bool IsTuple(this Type type, bool checkBaseTypes = true)
        {
#if NET471
            return type.GetInterface(nameof(ITuple), checkBaseTypes) != null;
#else
            if (type == typeof(Tuple))
            {
                return true;
            }

            while (type != null)
            {
#if NETSTANDARD1_1 || PROFILE_111
                if (type.GetTypeInfo().IsGenericType)
#else
                if (type.IsGenericType)
#endif
                {
                    var genType = type.GetGenericTypeDefinition();
                    if (genType == typeof(Tuple<>)
                        || genType == typeof(Tuple<,>)
                        || genType == typeof(Tuple<,,>)
                        || genType == typeof(Tuple<,,,>)
                        || genType == typeof(Tuple<,,,,>)
                        || genType == typeof(Tuple<,,,,,>)
                        || genType == typeof(Tuple<,,,,,,>)
                        || genType == typeof(Tuple<,,,,,,,>)
                        || genType == typeof(Tuple<,,,,,,,>))
                        return true;
                }

                if (!checkBaseTypes)
                {
                    break;
                }

#if NETSTANDARD1_1 || PROFILE_111
                type = type.GetTypeInfo().BaseType;
#else
                type = type.BaseType;
#endif
            }

            return false;
#endif
            }

    }

}