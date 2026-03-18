using UnityEngine;

public class MissionZone : MonoBehaviour
{
    public enum LoaiZone { DiemLayHang, DiemTraHang }

    [Header("Cấu hình Nhiệm vụ")]
    public LoaiZone loaiZone;
    public float tienThuong = 50f;

    [Header("Giao diện Hướng Dẫn")]
    public GameObject panelHuongDan; // Sẽ kéo HuongDan_Panel vào đây
    private bool daXemHuongDan = false; // Bộ nhớ: Đã xem chưa? (Tránh hiện lại nhiều lần)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShipperManager shipper = FindFirstObjectByType<ShipperManager>();

            if (shipper != null)
            {
                XuLyNhiemVu(shipper);
            }
        }
    }

    void XuLyNhiemVu(ShipperManager shipper)
    {
        // TRƯỜNG HỢP 1: Tại điểm LẤY HÀNG (A)
        if (loaiZone == LoaiZone.DiemLayHang)
        {
            // --- THÊM PHẦN HIỆN HƯỚNG DẪN Ở ĐÂY ---
            // Nếu có bảng hướng dẫn và người chơi chưa xem bao giờ
            if (panelHuongDan != null && daXemHuongDan == false)
            {
                panelHuongDan.SetActive(true); // Bật bảng lên
                Time.timeScale = 0f;           // Đóng băng thời gian (xe phanh gấp)
                daXemHuongDan = true;          // Đánh dấu là đã xem
            }
            // --------------------------------------

            if (!shipper.dangGiaoHang)
            {
                shipper.NhanDonHang();
                Debug.Log("Đã nhận đơn! Hãy chạy đến điểm giao hàng.");
            }
        }
        // TRƯỜNG HỢP 2: Tại điểm TRẢ HÀNG (B)
        else if (loaiZone == LoaiZone.DiemTraHang)
        {
            if (shipper.dangGiaoHang)
            {
                shipper.HoanThanhDonHang(tienThuong);
            }
        }
    }
}