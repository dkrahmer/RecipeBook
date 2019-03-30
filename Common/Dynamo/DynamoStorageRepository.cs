using Amazon.DynamoDBv2.DataModel;
using Common.Dynamo.Contracts;
using Common.Dynamo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Dynamo
{
    public class DynamoStorageRepository<T> : IDynamoStorageRepository<T> where T : DynamoDocument
    {
        private readonly IDynamoDBContext _context;

        public DynamoStorageRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<IList<T>> ReadAll()
        {
            var masterResults = new List<T>();

            var query = _context.ScanAsync<T>(null);
            while (!query.IsDone)
            {
                var currentResults = await query.GetNextSetAsync();
                masterResults.AddRange(currentResults);
            }

            return masterResults;
        }

        public async Task<T> Read(string id)
        {
            return await _context.LoadAsync<T>(id);
        }

        public async Task<string> Create(T obj, string createdById)
        {
            obj.Id = Guid.NewGuid().ToString();
            obj.CreateDate = DateTime.Now;
            obj.UpdateDate = DateTime.Now;
            obj.CreatedById = createdById;
            obj.UpdatedById = createdById;

            await _context.SaveAsync(obj);

            return obj.Id;
        }

        public async Task Update(T oldObj, T newObj, string id, string updatedbyId)
        {
            newObj.Id = id;
            newObj.CreateDate = oldObj.CreateDate;
            newObj.UpdateDate = DateTime.Now;
            newObj.CreatedById = oldObj.CreatedById;
            newObj.UpdatedById = updatedbyId;

            await _context.SaveAsync(newObj);
        }

        public async Task Delete(string id)
        {
            await _context.DeleteAsync<T>(id);
        }
    }
}