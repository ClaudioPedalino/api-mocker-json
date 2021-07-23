using System;
using System.ComponentModel.DataAnnotations;

namespace mock_json.Entities
{
    public class MockData
    {
        public MockData(string key, string value)
        {
            Key = key;
            Value = value;
            UpsertAt = DateTime.UtcNow.AddHours(-3);
        }

        [Key]
        public string Key { get; private set; }
        public string Value { get; private set; }

        public DateTime UpsertAt { get; private set; }

        public void UpdateValue(string value)
        {
            Value = value;
            UpsertAt = DateTime.UtcNow.AddHours(-3);
        }

    }
}
