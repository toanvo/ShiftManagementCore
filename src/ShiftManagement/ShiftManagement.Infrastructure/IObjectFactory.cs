namespace ShiftManagement.Infrastructure
{
    using System;

    public interface IObjectFactory
    {
        T Get<T>() where T : class;
        T GetByType<T>(Type type) where T : class;
    }
}
