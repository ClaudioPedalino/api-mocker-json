using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using mock_json.Data;
using mock_json.Entities;
using mock_json.Helper;
using mock_json.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace mock_json.Service
{
    public class MockService : IMockService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;

        public MockService(IConfiguration config, DataContext context, IMemoryCache cache)
        {
            _config = config;
            _context = context;
            _cache = cache;
        }

        private MockData GetDataByKey(string key)
            => _context.MockData.FirstOrDefault(x => x.Key == key.ToLower());


        public string GetMockData(string fileName = default)
            => !string.IsNullOrWhiteSpace(fileName)
                ? GetJsonFileAsString(fileName, _config)
                : _config.GetValue<string>("MOCK_RESULT");


        private static string GetJsonFileAsString(string fileName, IConfiguration _config)
        {
            string FolderToRead = _config.GetValue<string>("FolderToRead");
            var path = $"{Environment.CurrentDirectory}/{FolderToRead}";
            using StreamReader reader = new StreamReader($"{path}/{fileName}");
            return reader.ReadToEnd();
        }


        public List<string> GetAllKeys(int paginationSize, int pageNumber)
            => _context.MockData
                //.Skip(pageNumber * paginationSize)
                .Take(paginationSize)
                .Select(x => $"{nameof(x.Key)}: [{x.Key}]   => created at: {x.UpsertAt:dd/MM/yyyy hh:mm}")
                .ToList();


        public string GetByKey(string key)
        {
            var cached = CacheHelper.Get(key);
            if (!string.IsNullOrWhiteSpace(cached))
                return cached;

            var data = GetDataByKey(key);

            return data != null
                ? JsonConvert.DeserializeObject(data.Value).ToString()
                : $"No hay data para esa key =>  [{key}]";
        }


        public string Upsert(string key, JsonElement payload)
        {
            var entity = GetDataByKey(key);
            if (entity == null)
            {
                var newEntity = new MockData(
                    key,
                    payload.ToString()
                    );
                _context.MockData.Add(newEntity);
            }
            else
            {
                entity.UpdateValue(payload.ToString());
                _context.MockData.Update(entity);
            }
            _context.SaveChanges();

            CacheHelper.Set(key, payload.ToString());
            return $"Saved with key =>  [{key}]";
        }
    }
}