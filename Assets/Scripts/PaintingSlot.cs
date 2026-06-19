using UnityEngine;
using SUPERCharacter;

public class PaintingSlot : MonoBehaviour, IInteractable
{
    public Image360Viewer viewer;
    public string specimenID;

    public Renderer targetRenderer;
    public SpecimenAPIClient apiClient;

    private Texture2D[] images;
    private bool isLoaded = false;
    private int materialIndex = -1;

    private SpecimenData lastData;

    void Start()
    {
        if (!string.IsNullOrEmpty(specimenID) && apiClient != null)
        {
            StartCoroutine(apiClient.GetSpecimen(specimenID, (data) =>
            {
                lastData = data;

                StartCoroutine(apiClient.LoadImagesStreaming(
                    data.images,

                    (firstImage) =>
                    {
                        images = new Texture2D[] { firstImage };
                        ApplyPreview();
                    },

                    (loadedImages) =>
                    {
                        images = loadedImages;
                        isLoaded = true;
                        ApplyPreview();
                    }
                ));
            }));
        }
    }

    public bool Interact()
    {
        if (viewer == null || apiClient == null)
        {
            Debug.LogError("Falta asignar viewer o apiClient");
            return false;
        }

        if (string.IsNullOrEmpty(specimenID))
        {
            Debug.LogError("ID inválido");
            return false;
        }

        if (!isLoaded && images == null)
        {
            StartCoroutine(apiClient.GetSpecimen(specimenID, (data) =>
            {
                lastData = data;

                StartCoroutine(apiClient.LoadImagesStreaming(
                    data.images,

                    (firstImage) =>
                    {
                        images = new Texture2D[] { firstImage };
                        ApplyPreview();
                    },

                    (loadedImages) =>
                    {
                        images = loadedImages;
                        isLoaded = true;

                        ApplyPreview();

                        viewer.SetImages(images);
                        viewer.SetInfo(data.name, data.description);
                        viewer.Open();
                    }
                ));
            }));

            return true;
        }

        viewer.SetImages(images);

        if (lastData != null)
        {
            viewer.SetInfo(lastData.name, lastData.description);
        }

        viewer.Open();
        return true;
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
}