using System.Text.Json;


namespace Commons.Helpers
{
    public static class JsonElementValidators
    {
        public static bool IsValidDateTime(JsonElement? json)
        {
            if (!json.HasValue)
            {
                return false;
            }

            var valueType = json.Value.ValueKind;
            if (valueType == JsonValueKind.String)
            {
                return DateTime.TryParse(json.Value.GetString(), out _);
            }
            else if (valueType == JsonValueKind.Array)
            {
                return json.Value.EnumerateArray().ToList().All(s => DateTime.TryParse(s.GetString(), out _));
            }

            return false;
        }

        public static bool IsValidString(JsonElement? json, int min = 0, int max = 0)
        {
            if (!json.HasValue || (min != max && min > max))
            {
                return false;
            }

            var valueType = json.Value.ValueKind;
            if (valueType == JsonValueKind.String)
            {
                return json.Value.GetString().Length >= min && json.Value.GetString().Length <= max;
            }
            else if (valueType == JsonValueKind.Array)
            {
                return json.Value.EnumerateArray().ToList().All(s => s.GetString().Length >= min && s.GetString().Length <= max);
            }

            return false;
        }
    }
}
