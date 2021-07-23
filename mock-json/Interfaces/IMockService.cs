using System.Collections.Generic;
using System.Text.Json;

namespace mock_json.Interfaces
{
    public interface IMockService
    {
        string GetMockData(string fileName = default);
        List<string> GetAllKeys(int paginationSize, int pageNumber);
        string GetByKey(string key);
        string Create(string key, JsonElement payload);
    }
}