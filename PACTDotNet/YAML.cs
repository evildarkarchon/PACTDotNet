using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace PACTDotNet
{
    public class YamlData
    {
        public static YamlCache Pact_Main { get; set; } = new YamlCache("PACT Data/PACT Main.yaml");
        public static YamlCache Pact_Settings { get; set; } = new YamlCache("PACT Settings.yaml");
        public static YamlCache Pact_Ignore { get; set; } = new YamlCache("PACT Ignore.yaml");
        public static dynamic? Settings_Query(string key)
        {
            if (!File.Exists("PACT Settings.yaml"))
            {
                using StreamWriter sw = File.CreateText("CLASSIC Settings.yaml");
                sw.Write(Pact_Main.ReadOrUpdateEntry("default_settings"));
            }
            string? value = Pact_Settings.ReadOrUpdateEntry(key);
            switch (value)
            {
                case "True":
                case "true":
                    return true;
                case "False":
                case "false":
                    return false;
                case "Null":
                case "":
                case null:
                    return null;
                default:
                    return value;
            }
        }
    }
    public class YamlCache
    {
        private readonly string? _filePath;
        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
        private readonly IDeserializer _deserializer;
        private readonly ISerializer _serializer;
        private Dictionary<string, object>? _yamlData;

        public YamlCache(string? filePath)
        {
            _filePath = filePath;
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            _serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            LoadYamlFile();
        }

        private void LoadYamlFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var yamlContent = File.ReadAllText(_filePath);
                    _yamlData = _deserializer.Deserialize<Dictionary<string, object>>(yamlContent);
                }
                else
                {
                    _yamlData = new Dictionary<string, object>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading YAML file: {ex.Message}");
                _yamlData = new Dictionary<string, object>();
            }
        }

        public string ReadOrUpdateEntry(string key, string? newValue = null)
        {
            return ReadOrUpdateEntry<string>(key, newValue);
        }

        public T ReadOrUpdateEntry<T>(string key, T newValue = default(T))
        {
            try
            {
                var keys = key.Split('.');
                var currentNode = _yamlData as object;

                for (int i = 0; i < keys.Length - 1; i++)
                {
                    if (currentNode is Dictionary<string, object> currentDict)
                    {
                        if (currentDict.ContainsKey(keys[i]))
                        {
                            currentNode = currentDict[keys[i]];
                        }
                        else
                        {
                            if (EqualityComparer<T>.Default.Equals(newValue, default(T)))
                            {
                                return default(T);
                            }
                            var newDict = new Dictionary<string, object>();
                            currentDict[keys[i]] = newDict;
                            currentNode = newDict;
                        }
                    }
                    else
                    {
                        return default(T);
                    }
                }

                var finalKey = keys[^1];

                if (currentNode is Dictionary<string, object> finalDict)
                {
                    if (finalDict.ContainsKey(finalKey))
                    {
                        var currentValue = finalDict[finalKey];

                        if (!EqualityComparer<T>.Default.Equals(newValue, default(T)) && !newValue.Equals(currentValue))
                        {
                            finalDict[finalKey] = newValue;
                            SaveYamlFile();
                        }

                        return ConvertValue<T>(currentValue);
                    }

                    if (!EqualityComparer<T>.Default.Equals(newValue, default(T)))
                    {
                        finalDict[finalKey] = newValue;
                        SaveYamlFile();
                        return newValue;
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ReadOrUpdateEntry: {ex.Message}");
                return default(T);
            }
        }

        private T ConvertValue<T>(object value)
        {
            try
            {
                if (value is T variable)
                {
                    return variable;
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        private void SaveYamlFile()
        {
            try
            {
                var yamlContent = _serializer.Serialize(_yamlData);
                File.WriteAllText(_filePath, yamlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving YAML file: {ex.Message}");
            }
        }
    }
}
