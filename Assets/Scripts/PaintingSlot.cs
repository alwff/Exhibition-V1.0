using UnityEngine;
using SUPERCharacter;
using System.IO;

public class PaintingSlot : MonoBehaviour, IInteractable
{
    public Image360Viewer viewer;
    public string folderPath;

    public Renderer targetRenderer;

    private Texture2D[] images;
    private bool isLoaded = false;
    private int materialIndex = -1;

    public bool Interact()
    {
        Debug.Log("INTERACTUASTE CON EL CUADRO");

        if (viewer == null)
        {
            Debug.LogError("Viewer no asignado");
            return false;
        }

        if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
        {
            Debug.LogError("Ruta inválida: " + folderPath);
            return false;
        }

        if (!isLoaded)
        {
            images = SpecimenLoader.LoadImages(folderPath);
            isLoaded = true;

            ApplyPreview();
        }

        if (images != null && images.Length > 0)
        {
            viewer.SetImages(images);
            viewer.Open();
            return true;
        }

        Debug.LogWarning("No hay imágenes cargadas");
        return false;
    }

    public void ApplyPreview()
    {
        if (images == null || images.Length == 0 || targetRenderer == null) return;

        Material[] mats = targetRenderer.materials;

        if (materialIndex < 0)
        {
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.ToLower().Contains("picture"))
                {
                    materialIndex = i;
                    break;
                }
            }

            if (materialIndex < 0 && mats.Length > 1)
            {
                materialIndex = 1;
            }
        }

        if (materialIndex >= 0 && materialIndex < mats.Length)
        {
            mats[materialIndex].mainTexture = images[0];
            targetRenderer.materials = mats;
        }
    }

    public void SetFolder(string path)
    {
        folderPath = path;
        isLoaded = false;

        Texture2D[] preview = SpecimenLoader.LoadImages(path);

        if (preview != null && preview.Length > 0)
        {
            images = preview;
            ApplyPreview();
        }
    }
}