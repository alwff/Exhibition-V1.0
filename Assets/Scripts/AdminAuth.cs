using UnityEngine;
using TMPro;
using SUPERCharacter;

public class AdminAuth : MonoBehaviour
{
    public GameObject passwordCanvas;
    public GameObject adminCanvas;

    public TextMeshProUGUI passwordText;

    public string correctPassword = "1234";

    private string input = "";
    private bool isEntering = false;

    public SUPERCharacterAIO playerController;
    public Rigidbody playerRigidbody;

    void Start()
    {
        passwordCanvas.SetActive(false);
        adminCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OpenAuth();
        }

        if (!isEntering) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAll();
            return;
        }

        // Leer números
        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c))
                input += c;
        }

        // Borrar
        if (Input.GetKeyDown(KeyCode.Backspace) && input.Length > 0)
        {
            input = input.Substring(0, input.Length - 1);
        }

        // Mostrar contraseña
        if (passwordText != null)
            passwordText.text = "Clave: " + new string('*', input.Length);

        // Confirmar
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input == correctPassword)
            {
                passwordCanvas.SetActive(false);
                adminCanvas.SetActive(true);

                isEntering = false;
            }
            else
            {
                if (passwordText != null)
                    passwordText.text = "Clave incorrecta";

                input = "";
            }
        }
    }

    void OpenAuth()
    {
        passwordCanvas.SetActive(true);
        adminCanvas.SetActive(false);

        isEntering = true;
        input = "";

        if (passwordText != null)
            passwordText.text = "Ingrese clave\nESC para cancelar";

        // Bloquear jugador
        if (playerController != null)
            playerController.enabled = false;

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseAll()
    {
        passwordCanvas.SetActive(false);
        adminCanvas.SetActive(false);

        isEntering = false;
        input = "";

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}