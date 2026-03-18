using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [Header("--- ĐIỂM XUẤT PHÁT ---")]
    public GameObject diemXuatPhat;
    public Transform[] danhSachViTriXuatPhat; // Các vị trí có thể bắt đầu đua

    [Header("--- VẠCH ĐÍCH ---")]
    public GameObject vachDich;
    public Transform[] danhSachViTriDich; // Các vị trí đích ngẫu nhiên

    [Header("--- LA BÀN ĐUA XE ---")]
    public ChiDuong muiTenChiDuong; // Mũi tên chỉ đường (có thể dùng chung hoặc tạo mũi tên mới)

    void Start()
    {
        // Vừa vào game là gọi điểm xuất phát ra
        TrangThaiChoNhanCuoc();
    }

    public void TrangThaiChoNhanCuoc()
    {
        // Tắt Đích và La bàn
        if (muiTenChiDuong != null) muiTenChiDuong.gameObject.SetActive(false);
        if (vachDich != null) vachDich.SetActive(false);

        // Bật điểm Xuất Phát ở một vị trí ngẫu nhiên
        if (danhSachViTriXuatPhat.Length > 0 && diemXuatPhat != null)
        {
            int viTri = Random.Range(0, danhSachViTriXuatPhat.Length);
            diemXuatPhat.transform.position = danhSachViTriXuatPhat[viTri].position;
            diemXuatPhat.SetActive(true);
        }
    }

    public void TrangThaiChayDenDich()
    {
        // Tắt điểm xuất phát
        if (diemXuatPhat != null) diemXuatPhat.SetActive(false);

        // Bật Vạch Đích ngẫu nhiên
        if (danhSachViTriDich.Length > 0 && vachDich != null)
        {
            int viTri = Random.Range(0, danhSachViTriDich.Length);
            vachDich.transform.position = danhSachViTriDich[viTri].position;
            vachDich.SetActive(true);
        }

        // Bật la bàn trỏ về đích
        if (muiTenChiDuong != null)
        {
            muiTenChiDuong.gameObject.SetActive(true);
            muiTenChiDuong.mucTieu = vachDich.transform;
        }
    }
}