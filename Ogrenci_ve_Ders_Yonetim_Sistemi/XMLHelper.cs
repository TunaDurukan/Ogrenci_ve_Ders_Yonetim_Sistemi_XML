using System;
using System.IO;
using System.Xml.Serialization;

namespace Ogrenci_ve_Ders_Yonetim_Sistemi.Helpers
{
    public static class XMLHelper
    {
        public static void SaveToXml<T>(string filePath, T data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML kaydetme hatası: {ex.Message}");
            }
        }

        public static T? LoadFromXml<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        return (T?)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML okuma hatası: {ex.Message}");
            }
            return default;
        }
    }
}
