using UnityEngine;
using SUPERCharacter;

public class AdminToggle : MonoBehaviour
{
    public GameObject panel;
    public GameObject uiContainer;

    public SUPERCharacterAIO playerController;
    public Rigidbody playerRigidbody;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            bool isActive = !panel.activeSelf;

            panel.SetActive(isActive);
            uiContainer.SetActive(isActive);

            InputBlocker.blockInput = isActive;

            if (isActive)
            {
                if (playerController != null)
                    playerController.enabled = false;

                if (playerRigidbody != null)
                {
                    playerRigidbody.linearVelocity = Vector3.zero;
                    playerRigidbody.angularVelocity = Vector3.zero;
                    playerRigidbody.Sleep();
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                if (playerController != null)
                    playerController.enabled = true;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}