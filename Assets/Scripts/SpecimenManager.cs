using System.IO;
using UnityEngine;

public class SpecimenManager : MonoBehaviour
{
    public string basePath = "C:/Specimens/";

    public string[] GetAllSpecimens()
    {
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        return Directory.GetDirectories(basePath);
    }

    public string GetSpecimenName(string path)
    {
        return Path.GetFileName(path);
    }
}