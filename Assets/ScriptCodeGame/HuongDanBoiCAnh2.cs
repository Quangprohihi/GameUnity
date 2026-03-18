using UnityEngine;

public class HienThiHuongDan : MonoBehaviour
{
    [Header("GIAO DIỆN")]
    [Tooltip("Kéo Panel Hướng Dẫn từ Canvas vào đây")]
    public GameObject panelHuongDan;

    [Header("CÀI ĐẶT")]
    [Tooltip("Tích vào đây nếu bạn chỉ muốn bảng này hiện lên 1 lần duy nhất trong suốt ván chơi")]
    public bool chiHienMotLan = true;
    private bool daHienThi = false;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem vật chạm vào có mang thẻ "Player" (nhân vật hoặc xe) không
        if (other.CompareTag("Player"))
        {
            // Nếu cài đặt chỉ hiện 1 lần và đã hiện rồi, thì bỏ qua không làm gì cả
            if (chiHienMotLan && daHienThi) return;

            // Bật bảng hướng dẫn lên
            if (panelHuongDan != null)
            {
                panelHuongDan.SetActive(true);
                Time.timeScale = 0f; // Đóng băng thời gian để người chơi rảnh tay đọc chữ
                daHienThi = true;    // Đánh dấu là đã hiện rồi
            }
        }
    }
}