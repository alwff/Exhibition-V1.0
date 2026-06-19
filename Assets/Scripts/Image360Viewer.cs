using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Image360Viewer : MonoBehaviour
{
    public GameObject panel;
    public RawImage display;

    public GameObject hintContainer;
    public TextMeshProUGUI hintText;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public Rigidbody playerRigidbody;

    private Texture2D[] frames;
    private int index = 0;

    public float hintDuration = 3f;

    public void SetImages(Texture2D[] imgs)
    {
        frames = imgs;
        index = 0;

        if (frames != null && frames.Length > 0)
        {
            display.texture = frames[0];
        }
    }

    public void Open()
    {
        panel.SetActive(true);

        panel.transform.localScale = Vector3.zero;
        StartCoroutine(ScaleIn());

        InputBlocker.blockInput = true;

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.Sleep();
        }

        if (hintContainer != null)
        {
            hintContainer.SetActive(true);

            if (hintText != null)
            {
                hintText.text = "Arrastra presionando click izquierdo para rotar\nPresiona X para cerrar";
            }

            CancelInvoke(nameof(HideHint));
            Invoke(nameof(HideHint), hintDuration);
        }

        if (nameText != null)
            StartCoroutine(FadeText(nameText, 0.5f));

        if (descriptionText != null)
            StartCoroutine(FadeText(descriptionText, 0.8f));
    }

    public void Close()
    {
        panel.SetActive(false);
        InputBlocker.blockInput = false;
    }

    void Update()
    {
        if (!panel.activeSelf || frames == null || frames.Length == 0) return;

        if (Input.GetMouseButton(0))
        {
            float delta = Input.GetAxis("Mouse X");

            index += (int)(delta * 10);

            if (index >= frames.Length) index = 0;
            if (index < 0) index = frames.Length - 1;

            display.texture = frames[index];
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Close();
        }
    }

    IEnumerator ScaleIn()
    {
        float duration = 0.25f;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0, 1, t / duration);
            panel.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        panel.transform.localScale = Vector3.one;
    }

    IEnumerator FadeText(TextMeshProUGUI text, float duration)
    {
        if (text == null) yield break;

        Color c = text.color;
        c.a = 0;
        text.color = c;

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / duration);
            text.color = c;
            yield return null;
        }

        c.a = 1;
        text.color = c;
    }

    void HideHint()
    {
        if (hintContainer != null)
        {
            hintContainer.SetActive(false);
        }
    }

    public void SetInfo(string name, string description)
    {
        if (nameText != null)
            nameText.text = name;

        if (descriptionText != null)
            descriptionText.text = description;
    }
}