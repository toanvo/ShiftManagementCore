namespace ShiftManagement.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ObjectFactory : IObjectFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ObjectFactory(IServiceCollection services)
        {
            _serviceProvider = services.BuildServiceProvider();
        }

        public T Get<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }        

        public T GetByType<T>(Type type) where T : class
        {
            return _serviceProvider.GetService(type) as T;
        }
    }
}
