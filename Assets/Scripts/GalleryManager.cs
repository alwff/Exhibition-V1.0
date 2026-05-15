using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    public PaintingSlot[] slots;

    public void AssignSpecimen(int slotIndex, string path)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;

        slots[slotIndex].SetFolder(path);
    }

    public string[] GetCurrentAssignments()
    {
        string[] data = new string[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            data[i] = slots[i].folderPath;
        }

        return data;
    }
}