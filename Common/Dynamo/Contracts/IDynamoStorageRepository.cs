using Common.Dynamo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Dynamo.Contracts
{
    public interface IDynamoStorageRepository<T> where T : DynamoDocument
    {
        Task<string> Create(T obj, string createdById);
        Task Delete(string id);
        Task<T> Read(string id);
        Task<IList<T>> ReadAll();
        Task Update(T oldObj, T newObj, string id, string updatedbyId);
    }
}