using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveProfile (ProfileInfo profile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/profile.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        ProfileData data = new ProfileData(profile);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ProfileData LoadProfile()
    {
        string path = Application.persistentDataPath + "/profile.json";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ProfileData data = formatter.Deserialize(stream) as ProfileData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
