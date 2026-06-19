using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class SpecimenLoader : MonoBehaviour
{
    public static Texture2D[] LoadImages(string path)
    {
        var files = Directory.GetFiles(path)
            .Where(f => f.ToLower().EndsWith(".png") || f.ToLower().EndsWith(".jpg"))
            .OrderBy(f =>
            {
                var match = Regex.Match(f, @"\d+");
                return match.Success ? int.Parse(match.Value) : 0;
            })
            .ToArray();

        Texture2D[] textures = new Texture2D[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            byte[] bytes = File.ReadAllBytes(files[i]);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            textures[i] = tex;
        }

        return textures;
    }
}