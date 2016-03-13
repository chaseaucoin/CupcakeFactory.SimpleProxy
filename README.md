# CupcakeFactory.SimpleProxy

A very lightweight, and easy to consume proxy. Ideal for the proxying of interface methods.

# Usage

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
			method.Invoke(_baseObject, methodParameters.Args);
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