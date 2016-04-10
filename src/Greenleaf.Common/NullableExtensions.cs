using System;
using System.Collections.Generic;
using System.Linq;

namespace Greenleaf
{
    public static class NullableExtensions
    {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            return o == null ? null : evaluator(o);
        }

        public static TResult? With<TInput, TResult>(this TInput o, Func<TInput, TResult?> evaluator)
            where TResult : struct
            where TInput : class
        {
            return o == null ? null : evaluator(o);
        }

        public static TResult WithOverString<TResult>(this string o, Func<string, TResult> evaluator)
            where TResult : class
        {
            return string.IsNullOrEmpty(o) ? null : evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator,
            TResult failureValue)
            where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator,
            Func<TResult> failure)
            where TInput : class
        {
            return o == null ? failure() : evaluator(o);
        }

        public static IEnumerable<TResult> ManyWith<TInput, TResult>(
            this TInput o,
            Func<TInput, IEnumerable<TResult>> evaluator)
            where TInput : class
        {
            return o == null ? Enumerable.Empty<TResult>() : evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            return o == null ? null : (evaluator(o) ? o : null);
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            return o == null ? null : (evaluator(o) ? null : o);
        }

        public static TInput DoIfNull<TInput>(this TInput source, Action action)
            where TInput : class
        {
            if (source == null)
            {
                action();
            }

            return source;
        }

        public static TInput Do<TInput>(this TInput source, Action<TInput> action)
            where TInput : class
        {
            if (source == null)
            {
                return null;
            }

            action(source);

            return source;
        }
    }
}