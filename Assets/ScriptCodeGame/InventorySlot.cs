using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Kéo cái Khung Sáng vào đây")]
    public GameObject selectionFrame;

    [Header("Cài đặt phím tắt")]
    // Chọn phím trên bàn phím (Alpha1, Alpha2...)
    public KeyCode phimBam;

    // Biến static: Là biến DÙNG CHUNG cho tất cả các ô Slot
    public static InventorySlot currentActiveSlot;

    void Start()
    {
        // Mặc định khi game chạy, tắt cái khung đi cho chắc
        if (selectionFrame != null)
        {
            selectionFrame.SetActive(false);
        }
    }

    // --- PHẦN MỚI THÊM VÀO ---
    void Update()
    {
        // Máy tính sẽ kiểm tra liên tục mỗi khung hình
        // Nếu người chơi bấm đúng cái phím đã cài cho ô này
        if (Input.GetKeyDown(phimBam))
        {
            OnClickSlot(); // Tự động gọi hàm chọn giống như click chuột
        }
    }
    // -------------------------

    // Hàm này sẽ được gọi khi bấm chuột HOẶC bấm phím
    public void OnClickSlot()
    {
        // 1. Tắt đèn thằng cũ (nếu đang có thằng nào đó sáng)
        if (currentActiveSlot != null)
        {
            // Kiểm tra kỹ xem cái khung có tồn tại không để tránh lỗi đỏ
            if (currentActiveSlot.selectionFrame != null)
            {
                currentActiveSlot.selectionFrame.SetActive(false);
            }
        }

        // 2. Bật đèn chính mình lên
        if (selectionFrame != null)
        {
            selectionFrame.SetActive(true);
        }

        // 3. Ghi tên mình lên bảng vàng (Cập nhật biến static)
        currentActiveSlot = this;

        // 4. Thông báo (Debug)
        Debug.Log("Đang chọn slot bằng phím/chuột: " + phimBam.ToString());
    }
}