using UnityEngine;
using TMPro;
using System.IO;
using SUPERCharacter;

public class AdminUI : MonoBehaviour
{
    public GameObject adminCanvas;

    public TextMeshProUGUI specimenText;
    public TextMeshProUGUI slotText;
    public TextMeshProUGUI instructionsText;

    public SpecimenManager specimenManager;
    public GalleryManager galleryManager;
    public ConfigManager configManager;

    private string[] specimenPaths;
    private string[] tempAssignments;

    private int currentSpecimen = 0;
    private int currentSlot = 0;

    private bool isConfirming = false;

    void Start()
    {
        specimenPaths = specimenManager.GetAllSpecimens();

        if (specimenPaths == null || specimenPaths.Length == 0)
        {
            Debug.LogError("No hay especímenes");
            return;
        }

        var current = galleryManager.GetCurrentAssignments();
        tempAssignments = current.Clone() as string[];

        UpdateUI();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

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

        // Confirmar con Y
        if (Input.GetKeyDown(KeyCode.Y))
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

        // Cancelar selección
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isConfirming)
            {
                isConfirming = false;
                UpdateUI();
            }
        }

        // Salir del admin
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            adminCanvas.SetActive(false);

            var controller = FindFirstObjectByType<SUPERCharacterAIO>();
            if (controller != null)
                controller.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            configManager.Load();
        }

        // Guardar
        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < tempAssignments.Length; i++)
            {
                if (!string.IsNullOrEmpty(tempAssignments[i]))
                {
                    galleryManager.AssignSpecimen(i, tempAssignments[i]);
                }
            }

            configManager.Save();
            Debug.Log("CONFIG GUARDADA");
        }
    }

    void Assign()
    {
        string path = specimenPaths[currentSpecimen] + "/images";

        tempAssignments[currentSlot] = path;

        galleryManager.AssignSpecimen(currentSlot, path);
    }

    public void UpdateUI()
    {
        string specimenName = Path.GetFileName(specimenPaths[currentSpecimen]);

        specimenText.text = "Especimen: " + specimenName;
        slotText.text = "Cuadro: " + (currentSlot + 1);

        if (!isConfirming)
        {
            instructionsText.text =
                "W/S: Cambiar espécimen\n" +
                "A/D: Cambiar cuadro\n" +
                "Y: Seleccionar\n" +
                "G: Guardar\n" +
                "ESC: Salir";
        }
        else
        {
            instructionsText.text =
                "Y: Confirmar\n" +
                "X: Cancelar";
        }
    }
}