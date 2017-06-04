using System;
using NSubstitute.Callbacks;
using NSubstitute.Routing;

namespace NSubstitute.Core
{
    public class WhenCalled<T>
    {
        private readonly T _substitute;
        private readonly Action<T> _call;
        private readonly MatchArgs _matchArgs;
        private readonly ICallRouter _callRouter;
        private readonly IRouteFactory _routeFactory;

        public WhenCalled(ISubstitutionContext context, T substitute, Action<T> call, MatchArgs matchArgs)
        {
            _substitute = substitute;
            _call = call;
            _matchArgs = matchArgs;
            _callRouter = context.GetCallRouterFor(substitute);
            _routeFactory = context.GetRouteFactory();
        }

        /// <summary>
        /// Perform this action when called.
        /// </summary>
        /// <param name="callbackWithArguments"></param>
        public void Do(Action<CallInfo> callbackWithArguments)
        {
            _callRouter.SetRoute(x => _routeFactory.DoWhenCalled(x, callbackWithArguments, _matchArgs));
            _call(_substitute);
        }

        /// <summary>
        /// Perform this configured callcback when called.
        /// </summary>
        /// <param name="callback"></param>
        public void Do(Callback callback)
        {
            _callRouter.SetRoute(x => _routeFactory.DoWhenCalled(x, callback.Call, _matchArgs));
            _call(_substitute);
        }

        /// <summary>
        /// Do not call the base implementation on future calls. For use with partial substitutes.
        /// </summary>
        public void DoNotCallBase()
        {
            _callRouter.SetRoute(x => _routeFactory.DoNotCallBase(x, _matchArgs));
            _call(_substitute);
        }

        /// <summary>
        /// Throw the specified exception when called.
        /// </summary>
        public void Throw(Exception exception)
        {
            Do(ci => { throw exception; });
        }

        /// <summary>
        /// Throw an exception of the given type when called.
        /// </summary>
        public TException Throw<TException>() where TException : Exception, new()
        {
            var exception = new TException();
            Do(ci =>
            {
                throw exception;
            });
            return exception;
        }

        /// <summary>
        /// Throw an exception generated by the specified function when called.
        /// </summary>
        public void Throw(Func<CallInfo, Exception> createException)
        {
            Do(ci => { throw createException(ci); });
        }
    }
}