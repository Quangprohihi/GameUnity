using UnityEngine;

public class RaceZone : MonoBehaviour
{
    public enum LoaiZone { DiemXuatPhat, VachDich }

    [Header("Cài đặt Vùng Đua Xe")]
    public LoaiZone loaiZone;
    public float tienThuong = 1000f; // Số tiền cho chặng đua này

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShipperManager shipper = FindFirstObjectByType<ShipperManager>();
            if (shipper != null) XuLyNhiemVu(shipper);
        }
    }

    void XuLyNhiemVu(ShipperManager shipper)
    {
        if (loaiZone == LoaiZone.DiemXuatPhat)
        {
            // Chỉ nhận khi chưa bắt đầu đua
            if (!shipper.dangChayXe)
            {
                shipper.NhanCuocDua();
            }
        }
        else if (loaiZone == LoaiZone.VachDich)
        {
            // Chỉ nhận tiền nếu xe đang trong trạng thái đua
            if (shipper.dangChayXe)
            {
                shipper.HoanThanhCuocDua(tienThuong);
            }
        }
    }
}