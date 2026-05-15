using UnityEngine;
using TMPro;
using System.IO;
using SUPERCharacter;

public class AdminUI : MonoBehaviour
{
    public GameObject uiContainer;

    public TextMeshProUGUI specimenText;
    public TextMeshProUGUI slotText;
    public TextMeshProUGUI instructionsText;

    public SpecimenManager specimenManager;
    public GalleryManager galleryManager;
    public ConfigManager configManager;

    private string[] specimenPaths;
    private int currentSpecimen = 0;
    private int currentSlot = 0;

    private bool isConfirming = false;

    void Start()
    {
        specimenPaths = specimenManager.GetAllSpecimens();

        if (specimenPaths == null || specimenPaths.Length == 0)
        {
            Debug.LogError("No hay especímenes en la carpeta");
            return;
        }

        UpdateUI();
    }

    void Update()
    {
        if (uiContainer == null || !uiContainer.activeSelf) return;
        if (specimenPaths == null || specimenPaths.Length == 0) return;

        if (!isConfirming)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                currentSpecimen = (currentSpecimen - 1 + specimenPaths.Length) % specimenPaths.Length;
                UpdateUI();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                currentSpecimen = (currentSpecimen + 1) % specimenPaths.Length;
                UpdateUI();
            }

            if (galleryManager != null && galleryManager.slots.Length > 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currentSlot = (currentSlot - 1 + galleryManager.slots.Length) % galleryManager.slots.Length;
                    UpdateUI();
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    currentSlot = (currentSlot + 1) % galleryManager.slots.Length;
                    UpdateUI();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isConfirming)
            {
                isConfirming = true;
            }
            else
            {
                Assign();
                isConfirming = false;
            }

            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isConfirming)
            {
                isConfirming = false;
                UpdateUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiContainer.SetActive(false);
            InputBlocker.blockInput = false;

            var controller = FindFirstObjectByType<SUPERCharacterAIO>();
            if (controller != null)
                controller.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (configManager != null)
            {
                configManager.Save();
                Debug.Log("CONFIG GUARDADA");
            }
            else
            {
                Debug.LogError("ConfigManager no asignado");
            }
        }
    }

    void Assign()
    {
        if (galleryManager == null) return;

        string path = specimenPaths[currentSpecimen] + "/images";
        galleryManager.AssignSpecimen(currentSlot, path);
    }

    void UpdateUI()
    {
        string specimenName = Path.GetFileName(specimenPaths[currentSpecimen]);

        if (specimenText != null)
            specimenText.text = "Especimen: " + specimenName;

        if (slotText != null)
            slotText.text = "Cuadro: " + (currentSlot + 1);

        if (instructionsText != null)
        {
            if (!isConfirming)
            {
                instructionsText.text =
                    "W/S: Cambiar espécimen\n" +
                    "A/D: Cambiar cuadro\n" +
                    "ENTER: Seleccionar\n" +
                    "G: Guardar\n" +
                    "ESC: Salir";
            }
            else
            {
                instructionsText.text =
                    "ENTER: Confirmar\n" +
                    "X: Cancelar";
            }
        }
    }
}