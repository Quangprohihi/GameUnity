using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("--- ẨN HIỆN HOTBAR ---")]
    [Tooltip("Kéo nguyên cái bảng giao diện chứa 5 ô slot vào đây")]
    public GameObject khungHotbar;
    [Tooltip("Nút bấm để ẩn/hiện túi đồ")]
    public KeyCode nutBamAnHien = KeyCode.Tab;

    [Header("Kéo 5 cái Slot (từ 1 đến 5) vào đây")]
    public Transform[] slots;

    [Header("Kéo cục màu xanh Pizza Prefab vào đây")]
    public GameObject pizzaPrefab;

    private bool[] isFull;
    private GameObject[] itemTrongO;

    void Start()
    {
        isFull = new bool[slots.Length];
        itemTrongO = new GameObject[slots.Length]; // Khởi tạo bộ nhớ
    }

    void Update()
    {
        // Liên tục kiểm tra xem người chơi có bấm nút ẩn/hiện không
        if (Input.GetKeyDown(nutBamAnHien))
        {
            if (khungHotbar != null)
            {
                // Phép thuật ở đây: Đang hiện thì sẽ thành ẩn, đang ẩn sẽ thành hiện
                khungHotbar.SetActive(!khungHotbar.activeSelf);
            }
            else
            {
                Debug.LogWarning("Bạn chưa kéo khung Hotbar vào ô Khung Hotbar trong Inspector!");
            }
        }
    }

    public void NhatPizzaVaoTui()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (isFull[i] == false)
            {
                isFull[i] = true;

                // Lệnh Instatiate đẻ ra miếng bánh, ĐỒNG THỜI lưu nó vào bộ nhớ luôn!
                itemTrongO[i] = Instantiate(pizzaPrefab, slots[i], false);

                Debug.Log("Đã nhét Pizza vào ô: " + slots[i].name);
                break;
            }
        }
    }

    public void XoaPizzaKhoiTui()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (isFull[i] == true)
            {
                // Kiểm tra xem bộ nhớ có đang giữ cái bánh nào không
                if (itemTrongO[i] != null)
                {
                    // Phá hủy CHÍNH XÁC cái bánh đó
                    Destroy(itemTrongO[i]);
                }

                isFull[i] = false; // Trả lại ô trống

                Debug.Log("Đã giao bánh! Ô " + slots[i].name + " đã trống rỗng.");
                break;
            }
        }
    }
}