# CupcakeFactory.SimpleProxy

A very lightweight, and easy to consume proxy. Ideal for the proxying of interface methods.

# Example passthrough with logging

In this example we are going to use the original class to do some body of work, but we are going to add logging with each method call. 

```csharp	
	public class LoggerProxy<T>
    {        
        SimpleProxy<T> _proxy;
		T _baseObject;
		MethodLogger _logger;

		public LoggerProxy()
        {
            _proxy = new SimpleProxy<T>(InvokeWithLogging);			
			_logger = new MethodLogger();
        }

		public T CreateProxy(T baseObject)
		{
			_baseObject = baseObject;

			return (T)_proxy.GetTransparentProxy();
		}

		private object InvokeWithLogging(MethodBase method, MethodParameterCollection methodParameters)
        {
			_logger.Log(method, methodParameters);

			object returnValue = method.Invoke(_baseObject, methodParameters.Args);

			return returnValue;
		}
	}
```

```csharp
	static void Main(string[] args)
    {
		SomeServiceClass someService = new SomeServiceClass();

        SomeServiceClass proxiedService = new LoggerProxy<SomeServiceClass>()
            .CreateProxy(someService);

		proxiedService.SomeMethod();
	}
```