using System;

namespace Web.Domian.Mapping.Extensions
{
    public static class AutoMapperExtensions
    {
        public static bool ExceptDefaultValues<TSource, TDest>(TSource source, TDest dest, object srcMember)
        {
            return srcMember != null && srcMember.GetType().IsClass ||
                   srcMember.GetType().IsValueType && !srcMember.Equals(Activator.CreateInstance(srcMember.GetType()));
        }
    }
}