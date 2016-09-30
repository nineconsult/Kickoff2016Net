using System;

namespace Abp.EntityFramework
{
    public interface IDbContextTypeMatcher
    {
        void Add(Type sourceDbContextType, Type targetDbContextType);

        void Populate(Type[] dbContextTypes);

        Type GetConcreteType(Type sourceDbContextType);
    }
}