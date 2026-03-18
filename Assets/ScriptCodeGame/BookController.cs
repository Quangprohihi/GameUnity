using UnityEngine;

public class BookController : MonoBehaviour
{
    [Header("Kéo cái Panel cuốn sách vào đây")]
    public GameObject cuonSachPanel;

    // --- PHẦN MỚI THÊM VÀO ---
    void Update()
    {
        // Kiểm tra xem người chơi có vừa gõ phím F xuống không
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DoiTrangThaiSach(); // Nếu có thì tự động gọi hàm mở/đóng sách
        }
    }
    // -------------------------

    // Hàm này vẫn giữ nguyên như cũ
    public void DoiTrangThaiSach()
    {
        if (cuonSachPanel != null)
        {
            bool trangThaiHienTai = cuonSachPanel.activeSelf;
            cuonSachPanel.SetActive(!trangThaiHienTai);
        }
    }
}