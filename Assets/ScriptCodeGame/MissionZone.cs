using UnityEngine;

public class MissionZone : MonoBehaviour
{
    public enum LoaiZone { DiemA, GianHangLayHang, DiemTraHang }

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
            if (loaiZone == LoaiZone.DiemA)
            {
                MissionManager mission = FindFirstObjectByType<MissionManager>();
                if (mission != null) mission.NguoiChoiVaoDiemA();
            }
            else if (loaiZone == LoaiZone.GianHangLayHang)
            {
                Debug.Log("[Mission] Người chơi đã đến gian hàng: " + gameObject.name);
            }

            ShipperManager shipper = FindFirstObjectByType<ShipperManager>();

            if (shipper != null)
            {
                XuLyNhiemVu(shipper);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (loaiZone == LoaiZone.DiemA)
        {
            MissionManager mission = FindFirstObjectByType<MissionManager>();
            if (mission != null) mission.NguoiChoiRaDiemA();
        }
    }

    void XuLyNhiemVu(ShipperManager shipper)
    {
        // TRƯỜNG HỢP 1: Tại điểm LẤY HÀNG (A)
        if (loaiZone == LoaiZone.GianHangLayHang)
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

            if (shipper.dangGiaoHang && !shipper.daLayHang)
            {
                MissionManager mission = FindFirstObjectByType<MissionManager>();
                if (mission != null)
                {
                    mission.BatDauCheBienTaiQuay();
                    Debug.Log("[Mission] Đã vào quầy chế biến. Hãy chọn đủ nguyên liệu để hoàn thành món.");
                }
            }
        }
        // TRƯỜNG HỢP 2: Tại điểm TRẢ HÀNG (B)
        else if (loaiZone == LoaiZone.DiemTraHang)
        {
            if (shipper.dangGiaoHang && shipper.daLayHang)
            {
                shipper.HoanThanhDonHang(tienThuong);
            }
        }
    }
}