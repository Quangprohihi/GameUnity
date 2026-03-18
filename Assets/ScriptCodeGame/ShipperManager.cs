using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipperManager : MonoBehaviour
{
    [Header("THÔNG SỐ NGƯỜI CHƠI")]
    public float tienHienCo = 0f;
    public float doBenXe = 100f;
    private float thoiGianHoiPhuc = 0;

    [Header("TRẠNG THÁI GIAO HÀNG")]
    public bool dangGiaoHang = false;

    [Header("GIAO DIỆN (Kéo thả vào đây)")]
    public TextMeshProUGUI textTienUI;
    public GameObject chuBayPrefab;
    public Transform viTriHienChu;
    public Canvas mainCanvas;
    public InventoryManager tuiDo;

    public MissionManager heThongNhiemVu;

    [Header("--- HỆ THỐNG ĐUA XE ---")]
    public bool dangChayXe = false;

    [Header("--- ÂM THANH ---")]
    public AudioSource loaPhatNhac;
    public AudioClip amThanhTruTien;
    public AudioClip amThanhCongTien;

    void Start()
    {
        CapNhatTienUI();
    }

    public void NhanDonHang()
    {
        dangGiaoHang = true;
        Debug.Log("Shipper: Đã lấy hàng! Đang tìm nhà khách...");

        if (tuiDo != null)
        {
            tuiDo.NhatPizzaVaoTui();
        }

        if (heThongNhiemVu != null)
        {
            heThongNhiemVu.TrangThaiDiGiaoHang();
        }
    }

    public void HoanThanhDonHang(float tienThuong)
    {
        dangGiaoHang = false;
        tienHienCo += tienThuong;

        CapNhatTienUI();
        TaoHieuUngBay(tienThuong, true);

        Debug.Log("Shipper: Giao thành công! Nhận được: " + tienThuong);

        if (tuiDo != null)
        {
            tuiDo.XoaPizzaKhoiTui();
        }

        if (loaPhatNhac != null && amThanhCongTien != null)
        {
            loaPhatNhac.PlayOneShot(amThanhCongTien);
        }

        if (heThongNhiemVu != null)
        {
            // === MỚI THÊM: TÌM NPC KHÁCH HÀNG VÀ BẬT ANIMATION NHẬN BÁNH ===
            if (heThongNhiemVu.khachHang != null)
            {
                KhachHangDiChuyen khach = heThongNhiemVu.khachHang.GetComponent<KhachHangDiChuyen>();
                if (khach != null)
                {
                    khach.BatDauNhanBanh();
                }
            }
            // ===============================================================

            heThongNhiemVu.TrangThaiChoNhanDon();
        }
    }

    public void BiTruTien(float soTienMat)
    {
        if (Time.time < thoiGianHoiPhuc) return;

        tienHienCo -= soTienMat;
        if (tienHienCo < 0) tienHienCo = 0;

        CapNhatTienUI();
        TaoHieuUngBay(soTienMat, false);

        if (loaPhatNhac != null && amThanhTruTien != null)
        {
            loaPhatNhac.PlayOneShot(amThanhTruTien);
        }

        thoiGianHoiPhuc = Time.time + 5f;
        Debug.Log("Bị trừ tiền! Đang bất tử trong 5s...");
    }

    public void BiVaCham(float satThuong)
    {
        doBenXe -= satThuong;
        if (doBenXe < 0) doBenXe = 0;
    }

    void CapNhatTienUI()
    {
        if (textTienUI != null)
        {
            textTienUI.text = tienHienCo.ToString("N0") + " VND";
        }
    }

    void TaoHieuUngBay(float soTien, bool laCongTien)
    {
        if (chuBayPrefab != null && textTienUI != null)
        {
            GameObject textMoi = Instantiate(chuBayPrefab, textTienUI.transform.parent);
            textMoi.transform.position = textTienUI.transform.position;
            textMoi.transform.localPosition += new Vector3(0, 50, 0);

            FloatingText scriptText = textMoi.GetComponent<FloatingText>();
            if (scriptText != null)
            {
                scriptText.HienThiSoTien(soTien, laCongTien);
            }
        }
    }

    public void NhanTienThuongVeDich(float soTien)
    {
        tienHienCo += soTien;
        CapNhatTienUI();
        TaoHieuUngBay(soTien, true);

        if (loaPhatNhac != null && amThanhCongTien != null)
        {
            loaPhatNhac.PlayOneShot(amThanhCongTien);
        }

        Debug.Log("Về đích xuất sắc! Đã cộng: " + soTien + " VNĐ");
    }

    public void NhanCuocDua()
    {
        dangChayXe = true;
        Debug.Log("Đã nhận cuốc xe! Hãy chạy bạt mạng đến đích.");

        RaceManager heThongDuaXe = FindFirstObjectByType<RaceManager>();

        if (heThongDuaXe != null)
        {
            heThongDuaXe.TrangThaiChayDenDich();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy RaceManager trong Scene này!");
        }
    }

    public void HoanThanhCuocDua(float tienThuong)
    {
        dangChayXe = false;
        tienHienCo += tienThuong;

        CapNhatTienUI();
        TaoHieuUngBay(tienThuong, true);

        if (loaPhatNhac != null && amThanhCongTien != null)
        {
            loaPhatNhac.PlayOneShot(amThanhCongTien);
        }

        Debug.Log("Về đích an toàn! Nhận được: " + tienThuong);

        RaceManager heThongDuaXe = FindFirstObjectByType<RaceManager>();

        if (heThongDuaXe != null)
        {
            heThongDuaXe.TrangThaiChoNhanCuoc();
        }
    }
}