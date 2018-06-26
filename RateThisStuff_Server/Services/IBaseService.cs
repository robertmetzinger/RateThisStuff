using System.Collections.Generic;
using System.ServiceModel;

namespace RateThisStuff_Server.Services
{
    public interface IBaseService<T>
    {
        
        T GetById(int id);

        
        List<T> GetAll();

        
        bool SaveOrUpdate(T element);

        
        bool Delete(T element);
    }
}
