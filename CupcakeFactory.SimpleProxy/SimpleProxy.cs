using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace CupcakeFactory.SimpleProxy
{
    /// <summary>
    /// A simple way of proxying the invokation of methods on interfaecs and classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Runtime.Remoting.Proxies.RealProxy" />
    public class SimpleProxy<T> : RealProxy
    {
        Func<MethodBase, MethodParameterCollection, object> _invokeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleProxy{T}"/> class.
        /// </summary>
        /// <param name="invokeCallback">The invoke callback. This function will be called whenever a method is invoked from <see cref="T"/></param>
        public SimpleProxy(Func<MethodBase, MethodParameterCollection, object> invokeCallback) : base(typeof(T))
        {
            _invokeCallback = invokeCallback;
        }

        /// <summary>
        /// When overridden in a derived class, invokes the method that is specified in the provided <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> on the remote object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> that contains a <see cref="T:System.Collections.IDictionary" /> of information about the method call.</param>
        /// <returns>
        /// The message returned by the invoked method, containing the return value and any out or ref parameters.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
        /// </PermissionSet>
        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase;
            var methodArgs = new MethodParameterCollection(methodCall);

            var returnObj = _invokeCallback(methodInfo, methodArgs);

            var message = new ReturnMessage(returnObj, null, 0,
                methodCall.LogicalCallContext, methodCall);

            return message;
        }
    }
}
