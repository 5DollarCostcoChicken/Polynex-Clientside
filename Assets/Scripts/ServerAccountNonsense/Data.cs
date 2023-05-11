using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

    public static class Data
    {
    public class Player
    {
        public int level = 0;
        public int xp = 0;
        public string username;
        public int power = 0;
        public int pfp = 0;
        public List<Character> characters = new List<Character>();
    }
    public class Character
    {
        public int char_index = 0;
        public int level = 0;
        public int xp = 0;
        public string characterName;
        public string cName;
        public int stars = 0;
        public int shards = 0;
        public int min_shards = 0;
        public int power = 0;
        public bool activated = false;
    }

    public static string Serialize<T>(this T target)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter();
            xml.Serialize(writer, target);
            return writer.ToString();
        }

        public static T Deserialize<T>(this string target)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(target);
            return (T)xml.Deserialize(reader);
        }
    }

