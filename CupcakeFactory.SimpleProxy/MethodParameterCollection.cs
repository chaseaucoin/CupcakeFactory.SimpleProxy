using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace CupcakeFactory.SimpleProxy
{
    /// <summary>
    /// Represents an enumeration of method parameters. Helpful for converting IMethodCallMessage into something a bit more useable.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{CupcakeFactory.SimpleProxy.MethodParameter}" />
    public class MethodParameterCollection : IEnumerable<MethodParameter>
    {
        List<MethodParameter> _internalList;
        object[] _args;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameterCollection"/> class.
        /// </summary>
        /// <param name="methodCallMessage">The method call message.</param>
        public MethodParameterCollection(IMethodCallMessage methodCallMessage)
        {
            //Create an internal holder for the data
            _internalList = new List<MethodParameter>();

            
            var parameterIndex = new Dictionary<string, int>();
            
            var parameters = methodCallMessage
                    .MethodBase
                    .GetParameters();

            //First we need to index the args so we know how to find the values later
            for (int i = 0; i < methodCallMessage.ArgCount; i++)
            {
                var key = methodCallMessage.GetArgName(i);

                parameterIndex.Add(key, i);
            }

            //Hydrate our list.
            foreach(var parameter in parameters)
            {
                var name = parameter.Name;
                var index = parameterIndex[name];
                var value = methodCallMessage.Args[index];
                var valueType = parameter.ParameterType;

                var methodArg = new MethodParameter()
                {
                    Index = index,
                    Name = name,
                    Value = value,
                    Type = valueType
                };

                _internalList.Add(methodArg);
            }

            _args = methodCallMessage.Args;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<MethodParameter> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public object[] Args { get { return _args; } }
    }
}
