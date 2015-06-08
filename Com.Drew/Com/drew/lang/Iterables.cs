using System.Collections.Generic;
using Sharpen;

namespace Com.Drew.Lang
{
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public class Iterables
    {
        public static IList<E> ToList<E>(Iterable<E> iterable)
        {
            AList<E> list = new AList<E>();
            list.AddRange(iterable);
            return list;
        }
    }
}
