﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NSubstitute.Core;

// Disable nullability for client API, so it does not affect clients.
#nullable disable annotations

namespace NSubstitute.ExceptionExtensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Throw an exception for this call.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall Throws(this object value, Exception ex) =>
            value.Returns(_ => throw ex);

        /// <summary>
        /// Throw an exception of the given type for this call.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConfiguredCall Throws<TException>(this object value)
            where TException : notnull, Exception, new()
        {
            return value.Returns(_ => throw new TException());
        }

        /// <summary>
        /// Throw an exception for this call, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall Throws(this object value, Func<CallInfo, Exception> createException) =>
            value.Returns(ci => throw createException(ci));

        /// <summary>
        /// Throw an exception for this call made with any arguments.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsForAnyArgs(this object value, Exception ex) =>
            value.ReturnsForAnyArgs(_ => throw ex);

        /// <summary>
        /// Throws an exception of the given type for this call made with any arguments.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsForAnyArgs<TException>(this object value)
            where TException : notnull, Exception, new()
        {
            return value.ReturnsForAnyArgs(_ => throw new TException());
        }

        /// <summary>
        /// Throws an exception for this call made with any arguments, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsForAnyArgs(this object value, Func<CallInfo, Exception> createException) =>
            value.ReturnsForAnyArgs(ci => throw createException(ci));

        /// <summary>
        /// Throw an exception for this call.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync(this Task value, Exception ex) =>
            value.Returns(_ => TaskFromException(ex));

        /// <summary>
        /// Throw an exception for this call.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync<T>(this Task<T> value, Exception ex) =>
            value.Returns(_ => TaskFromException<T>(ex));

        /// <summary>
        /// Throw an exception of the given type for this call.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync<TException>(this Task value)
            where TException : notnull, Exception, new()
        {
            return value.Returns(_ => FromException(value, new TException()));
        }

        /// <summary>
        /// Throw an exception for this call, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync(this Task value, Func<CallInfo, Exception> createException) =>
            value.Returns(ci => TaskFromException(createException(ci)));

        /// <summary>
        /// Throw an exception for this call, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync<T>(this Task<T> value, Func<CallInfo, Exception> createException) =>
            value.Returns(ci => TaskFromException<T>(createException(ci)));

        /// <summary>
        /// Throw an exception for this call made with any arguments.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs(this Task value, Exception ex) =>
            value.ReturnsForAnyArgs(_ => TaskFromException(ex));

        /// <summary>
        /// Throw an exception for this call made with any arguments.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs<T>(this Task<T> value, Exception ex) =>
            value.ReturnsForAnyArgs(_ => TaskFromException<T>(ex));

        /// <summary>
        /// Throws an exception of the given type for this call made with any arguments.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs<TException>(this Task value)
            where TException : notnull, Exception, new()
        {
            return value.ReturnsForAnyArgs(_ => FromException(value, new TException()));
        }

        /// <summary>
        /// Throws an exception for this call made with any arguments, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs(this Task value, Func<CallInfo, Exception> createException) =>
            value.ReturnsForAnyArgs(ci => TaskFromException(createException(ci)));

        /// <summary>
        /// Throws an exception for this call made with any arguments, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs<T>(this Task<T> value, Func<CallInfo, Exception> createException) =>
            value.ReturnsForAnyArgs(ci => TaskFromException<T>(createException(ci)));

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <summary>
        /// Throw an exception for this call.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync<T>(this ValueTask<T> value, Exception ex) =>
            value.Returns(_ => ValueTask.FromException<T>(ex));

        /// <summary>
        /// Throw an exception of the given type for this call.
        /// </summary>
        /// <typeparam name="TResult">Type of exception to throw</typeparam>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync<TResult, TException>(this ValueTask<TResult> value)
            where TException : notnull, Exception, new()
        {
            return value.Returns(_ => ValueTask.FromException<TResult>(new TException()));
        }

        /// <summary>
        /// Throw an exception for this call, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsync<T>(this ValueTask<T> value, Func<CallInfo, Exception> createException) =>
            value.Returns(ci => ValueTask.FromException<T>(createException(ci)));

        /// <summary>
        /// Throws an exception of the given type for this call made with any arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs<T, TException>(this ValueTask<T> value)
            where TException : notnull, Exception, new()
        {
            return value.ReturnsForAnyArgs(_ => ValueTask.FromException<T>(new TException()));
        }

        /// <summary>
        /// Throw an exception for this call made with any arguments.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ex">Exception to throw</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs<T>(this ValueTask<T> value, Exception ex) =>
            value.ReturnsForAnyArgs(_ => ValueTask.FromException<T>(ex));

        /// <summary>
        /// Throws an exception for this call made with any arguments, as generated by the specified function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="createException">Func creating exception object</param>
        /// <returns></returns>
        public static ConfiguredCall ThrowsAsyncForAnyArgs<T>(this ValueTask<T> value, Func<CallInfo, Exception> createException) =>
            value.ReturnsForAnyArgs(ci => ValueTask.FromException<T>(createException(ci)));
#endif

        private static object FromException(object value, Exception exception)
        {
            // Handle Task<T>
            var valueType = value.GetType();
            if (valueType.IsConstructedGenericType)
            {
                var fromExceptionMethodInfo = typeof(Task).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(m => m.Name == "FromException" && m.ContainsGenericParameters);
                var specificFromExceptionMethod = fromExceptionMethodInfo.MakeGenericMethod(valueType.GenericTypeArguments);
                return specificFromExceptionMethod.Invoke(null, new object[] {exception});
            }

            return TaskFromException(exception);
        }

        private static Task TaskFromException(Exception ex)  {
#if NET45
            return new Task(() => throw ex);
#endif
            return Task.FromException(ex);
        }

        private static Task<T> TaskFromException<T>(Exception ex)
        {
#if NET45
            return new Task<T>(() => throw ex);
#endif
            return Task<T>.FromException<T>(ex);
        }

    }
}
