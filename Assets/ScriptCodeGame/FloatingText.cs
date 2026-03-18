using UnityEngine;
using TMPro; // Bắt buộc

public class FloatingText : MonoBehaviour
{
    // --- SỬA Ở ĐÂY: Thêm chữ UGUI vào đuôi ---
    public TextMeshProUGUI textMesh;

    public float tocDoBay = 50f; // Tăng tốc độ lên vì UI tính theo Pixel
    public float thoiGianBienMat = 1.5f;

    void Awake()
    {
        // Tự tìm component UI
        if (textMesh == null) textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        Destroy(gameObject, thoiGianBienMat);
    }

    void Update()
    {
        // Bay lên theo hướng trục Y của màn hình
        transform.Translate(Vector3.up * tocDoBay * Time.deltaTime);
    }

    public void HienThiSoTien(float soTien, bool laCongTien)
    {
        if (textMesh == null) return;

        if (laCongTien)
        {
            textMesh.text = "+" + soTien.ToString("N0") + "VNĐ";
            textMesh.color = Color.green; // Màu Xanh
        }
        else
    {
            textMesh.text = "-" + soTien.ToString("N0") + "VNĐ";
            textMesh.color = Color.red;   // Màu Đỏ
        }
    }
}