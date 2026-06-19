using UnityEngine;
using TMPro;

public class AdminUI : MonoBehaviour
{
    public GameObject adminCanvas;

    public TextMeshProUGUI specimenText;
    public TextMeshProUGUI slotText;
    public TextMeshProUGUI instructionsText;

    public GalleryManager galleryManager;

    // LISTA DE IDs TEMPORAL
    public string[] specimenIDs = { "BUTTERFLY01", "DINO01" };

    private string[] tempAssignments;

    private int currentSpecimen = 0;
    private int currentSlot = 0;

    private bool isConfirming = false;

    void Start()
    {
        if (galleryManager == null)
        {
            Debug.LogError("GalleryManager no asignado");
            return;
        }

        tempAssignments = galleryManager.GetCurrentAssignments();

        if (tempAssignments == null || tempAssignments.Length == 0)
        {
            tempAssignments = new string[galleryManager.slots.Length];
        }

        UpdateUI();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        // Navegación entre especímenes
        if (!isConfirming)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                currentSpecimen = (currentSpecimen - 1 + specimenIDs.Length) % specimenIDs.Length;
                UpdateUI();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                currentSpecimen = (currentSpecimen + 1) % specimenIDs.Length;
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

        // Confirmar selección
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

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Assign()
    {
        string id = specimenIDs[currentSpecimen];

        tempAssignments[currentSlot] = id;

        galleryManager.AssignSpecimen(currentSlot, id);

        Debug.Log("Asignado: " + id + " al cuadro " + currentSlot);
    }

    public void UpdateUI()
    {
        string specimenID = specimenIDs[currentSpecimen];

        if (specimenText != null)
            specimenText.text = "Especimen: " + specimenID;

        if (slotText != null)
            slotText.text = "Cuadro: " + (currentSlot + 1);

        if (instructionsText != null)
        {
            if (!isConfirming)
            {
                instructionsText.text =
                    "W/S: Cambiar espécimen\n" +
                    "A/D: Cambiar cuadro\n" +
                    "Y: Seleccionar\n" +
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
}