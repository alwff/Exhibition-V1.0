using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Image360Viewer : MonoBehaviour
{
    public GameObject panel;
    public RawImage display;

    public GameObject hintContainer;
    public TextMeshProUGUI hintText;

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
                hintText.text = "Arrastra para rotar\nPresiona X o ESC para cerrar";
            }

            CancelInvoke(nameof(HideHint));
            Invoke(nameof(HideHint), hintDuration);
        }
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

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    void HideHint()
    {
        if (hintContainer != null)
        {
            hintContainer.SetActive(false);
        }
    }
}