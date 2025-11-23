using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MLOOP2_L7
{
    public static class BorysSerializingManager
    {
        private const string SEPARATOR = "|";
        private const string LINE_SEPARATOR = "\n";
        private const string NULL_VALUE = "~NULL~";

        public static void Serialize<T>(List<T> objects, string filePath) where T : class
        {
            try
            {
                StringBuilder content = new StringBuilder();

                if (objects == null || objects.Count == 0)
                {
                    File.WriteAllText(filePath, string.Empty, Encoding.UTF8);
                    return;
                }

                Type type = typeof(T);
                var properties = type.GetProperties();

                foreach (var obj in objects)
                {
                    List<string> values = new List<string>();

                    foreach (var prop in properties)
                    {
                        object value = prop.GetValue(obj);
                        string serializedValue = SerializeValue(value);
                        values.Add(serializedValue);
                    }

                    content.AppendLine(string.Join(SEPARATOR, values));
                }

                File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка серіалізації: " + ex.Message);
            }
        }

        public static List<T> Deserialize<T>(string filePath) where T : class, new()
        {
            List<T> result = new List<T>();

            try
            {
                if (!File.Exists(filePath))
                {
                    return result;
                }

                string content = File.ReadAllText(filePath, Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(content))
                {
                    return result;
                }

                string[] lines = content.Split(new[] { LINE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                Type type = typeof(T);
                var properties = type.GetProperties();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] values = line.Trim().Split(new[] { SEPARATOR }, StringSplitOptions.None);

                    if (values.Length != properties.Length)
                    {
                        continue;
                    }

                    T obj = new T();

                    for (int i = 0; i < properties.Length; i++)
                    {
                        var prop = properties[i];
                        string value = values[i];
                        object deserializedValue = DeserializeValue(value, prop.PropertyType);
                        prop.SetValue(obj, deserializedValue);
                    }

                    result.Add(obj);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка десеріалізації: " + ex.Message);
            }
        }

        private static string SerializeValue(object value)
        {
            if (value == null)
            {
                return NULL_VALUE;
            }

            Type type = value.GetType();

            if (type == typeof(DateTime))
            {
                DateTime dateTime = (DateTime)value;
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (type == typeof(bool))
            {
                return value.ToString();
            }

            string stringValue = value.ToString();
            stringValue = stringValue.Replace(SEPARATOR, "&#124;");
            stringValue = stringValue.Replace(LINE_SEPARATOR, "&#10;");

            return stringValue;
        }

        private static object DeserializeValue(string value, Type targetType)
        {
            if (value == NULL_VALUE)
            {
                return null;
            }

            value = value.Replace("&#124;", SEPARATOR);
            value = value.Replace("&#10;", LINE_SEPARATOR);

            if (targetType == typeof(string))
            {
                return value;
            }

            if (targetType == typeof(int))
            {
                int result;
                if (int.TryParse(value, out result))
                {
                    return result;
                }
                return 0;
            }

            if (targetType == typeof(bool))
            {
                bool result;
                if (bool.TryParse(value, out result))
                {
                    return result;
                }
                return false;
            }

            if (targetType == typeof(DateTime))
            {
                DateTime result;
                if (DateTime.TryParse(value, out result))
                {
                    return result;
                }
                return DateTime.MinValue;
            }

            return value;
        }
    }
}