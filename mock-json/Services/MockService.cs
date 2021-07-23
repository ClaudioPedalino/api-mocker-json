using Microsoft.Extensions.Configuration;
using mock_json.Data;
using mock_json.Entities;
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

        public MockService(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

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
            var data = GetDataByKey(key);
            if (data != null)
            {
                var result = JsonConvert.DeserializeObject(data.Value).ToString();
                return result;
            }
            else
            {
                return $"no hay data para esa key =>  [{key}]";
            }
        }

        private MockData GetDataByKey(string key)
            => _context.MockData.FirstOrDefault(x => x.Key == key.ToLower());


        public string Create(string key, JsonElement payload)
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

            return $"Saved with key =>  [{key}]";
        }
    }
}
