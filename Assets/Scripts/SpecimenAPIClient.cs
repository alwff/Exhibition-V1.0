using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class SpecimenData
{
    public string id;
    public string name;
    public string description;
    public string[] images;
}

public class SpecimenAPIClient : MonoBehaviour
{
    public string baseURL = "http://192.168.1.25:5000/api/specimen/";

    public IEnumerator GetSpecimen(string id, System.Action<SpecimenData> callback)
    {
        string url = baseURL + id;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            SpecimenData data = JsonUtility.FromJson<SpecimenData>(request.downloadHandler.text);
            callback(data);
        }
        else
        {
            Debug.LogError("Error API: " + request.error);
        }
    }

    public IEnumerator LoadImagesStreaming(
        string[] urls,
        System.Action<Texture2D> onFirstLoaded,
        System.Action<Texture2D[]> onAllLoaded)
    {
        List<Texture2D> textures = new List<Texture2D>();

        for (int i = 0; i < urls.Length; i++)
        {
            UnityWebRequest req = UnityWebRequestTexture.GetTexture(urls[i]);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(req);
                textures.Add(tex);

                if (i == 0 && onFirstLoaded != null)
                {
                    onFirstLoaded(tex);
                }
            }
        }

        onAllLoaded?.Invoke(textures.ToArray());
    }
}