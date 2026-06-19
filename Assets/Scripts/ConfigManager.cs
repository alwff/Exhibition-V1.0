using System.IO;
using UnityEngine;

[System.Serializable]
public class ConfigData
{
    public string[] slots;
}

public class ConfigManager : MonoBehaviour
{
    public string filePath = "C:/Specimens/config.json";
    public GalleryManager gallery;

    void Start()
    {
        // Load();
    }

    public void Save()
    {
        if (gallery == null)
        {
            Debug.LogError("Gallery no asignado en ConfigManager");
            return;
        }

        ConfigData data = new ConfigData();

        string[] raw = gallery.GetCurrentAssignments();
        string[] clean = new string[raw.Length];

        for (int i = 0; i < raw.Length; i++)
        {
            clean[i] = string.IsNullOrEmpty(raw[i]) ? "" : raw[i];
        }

        data.slots = clean;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        Debug.Log("CONFIG GUARDADA");
    }

    public void Load()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("No existe config.json aún");
            return;
        }

        string json = File.ReadAllText(filePath);
        ConfigData data = JsonUtility.FromJson<ConfigData>(json);

        if (data == null || data.slots == null)
        {
            Debug.LogWarning("Config inválida");
            return;
        }

        if (gallery == null)
        {
            Debug.LogError("Gallery no asignado en ConfigManager");
            return;
        }

        for (int i = 0; i < data.slots.Length && i < gallery.slots.Length; i++)
        {
            string path = data.slots[i];

            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                gallery.AssignSpecimen(i, path);
            }
        }

        Debug.Log("CONFIG CARGADA");
    }
}