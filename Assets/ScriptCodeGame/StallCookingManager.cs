using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StallCookingManager : MonoBehaviour
{
    [Header("Công thức món (MVP = 3 món)")]
    public FoodRecipeData[] danhSachMon;

    [Header("UI Chế Biến")]
    public GameObject panelCheBien;
    public TextMeshProUGUI textTenMon;
    public TextMeshProUGUI textDongHo;
    public TextMeshProUGUI textThongBao;
    public Toggle[] toggleNguyenLieu;
    public TextMeshProUGUI[] textNguyenLieu;

    [Header("Cấu hình")]
    public float soGiayPhatKhiSai = 2f;

    [Header("Kết nối hệ thống")]
    public ShipperManager shipper;

    private FoodRecipeData monDangYeuCau;
    private bool dangCheBien = false;
    private float thoiGianConLai = 0f;

    void Start()
    {
        if (panelCheBien != null) panelCheBien.SetActive(false);
        if (textThongBao != null) textThongBao.text = "";
        CapNhatDongHo();
    }

    void Update()
    {
        if (!dangCheBien) return;

        thoiGianConLai -= Time.deltaTime;
        CapNhatDongHo();

        if (thoiGianConLai <= 0f)
        {
            XuLyHetGio();
        }
    }

    public void TaoDonMoiNgauNhien()
    {
        if (danhSachMon == null || danhSachMon.Length == 0)
        {
            monDangYeuCau = null;
            Debug.LogWarning("StallCookingManager: Chưa có danh sách món.");
            return;
        }

        monDangYeuCau = danhSachMon[Random.Range(0, danhSachMon.Length)];
        dangCheBien = false;
        thoiGianConLai = monDangYeuCau != null ? monDangYeuCau.thoiGianCheBien : 0f;

        if (panelCheBien != null) panelCheBien.SetActive(false);
        if (textThongBao != null) textThongBao.text = "";

        Debug.Log("[Cooking] Đơn mới: " + GetTenMonDangYeuCau());
    }

    public string GetTenMonDangYeuCau()
    {
        if (monDangYeuCau == null || string.IsNullOrWhiteSpace(monDangYeuCau.tenMon))
        {
            return "Món đặc biệt";
        }

        return monDangYeuCau.tenMon;
    }

    public bool CoDonDangXuLy()
    {
        return monDangYeuCau != null;
    }

    public void MoBangCheBien()
    {
        if (monDangYeuCau == null)
        {
            Debug.LogWarning("StallCookingManager: Không có đơn để chế biến.");
            return;
        }

        if (panelCheBien != null) panelCheBien.SetActive(true);

        if (textTenMon != null)
        {
            textTenMon.text = "Làm món: " + GetTenMonDangYeuCau();
        }

        if (!dangCheBien)
        {
            thoiGianConLai = Mathf.Max(1f, monDangYeuCau.thoiGianCheBien);
        }

        dangCheBien = true;
        if (textThongBao != null) textThongBao.text = "Chọn đủ nguyên liệu rồi bấm Hoàn thành.";

        DoDuLieuNguyenLieuLenUI();
        CapNhatDongHo();
    }

    public void NutHoanThanhCheBien()
    {
        if (!dangCheBien || monDangYeuCau == null) return;

        if (KiemTraNguyenLieuDung())
        {
            dangCheBien = false;
            if (panelCheBien != null) panelCheBien.SetActive(false);

            Debug.Log("[Cooking] Hoàn thành món: " + GetTenMonDangYeuCau());

            if (shipper != null)
            {
                shipper.LayHangTaiGian();
            }

            monDangYeuCau = null;
            return;
        }

        thoiGianConLai = Mathf.Max(0f, thoiGianConLai - soGiayPhatKhiSai);
        CapNhatDongHo();

        if (textThongBao != null)
        {
            textThongBao.text = "Sai nguyên liệu! Bị trừ " + soGiayPhatKhiSai.ToString("0") + " giây.";
        }

        Debug.Log("[Cooking] Sai nguyên liệu, trừ thời gian.");
    }

    void XuLyHetGio()
    {
        dangCheBien = false;
        thoiGianConLai = 0f;
        CapNhatDongHo();

        if (panelCheBien != null) panelCheBien.SetActive(false);
        if (textThongBao != null) textThongBao.text = "Hết giờ! Đơn bị hủy.";

        Debug.Log("[Cooking] Hết giờ chế biến, hủy đơn.");

        monDangYeuCau = null;

        if (shipper != null)
        {
            shipper.HuyDonHang();
        }
    }

    void DoDuLieuNguyenLieuLenUI()
    {
        int soNguyenLieu = monDangYeuCau != null && monDangYeuCau.danhSachNguyenLieu != null
            ? monDangYeuCau.danhSachNguyenLieu.Length
            : 0;

        for (int i = 0; i < toggleNguyenLieu.Length; i++)
        {
            bool hien = i < soNguyenLieu;

            if (toggleNguyenLieu[i] != null)
            {
                toggleNguyenLieu[i].isOn = false;
                toggleNguyenLieu[i].gameObject.SetActive(hien);
            }

            if (textNguyenLieu != null && i < textNguyenLieu.Length && textNguyenLieu[i] != null)
            {
                textNguyenLieu[i].text = hien ? monDangYeuCau.danhSachNguyenLieu[i] : "";
            }
        }
    }

    bool KiemTraNguyenLieuDung()
    {
        int soNguyenLieu = monDangYeuCau != null && monDangYeuCau.danhSachNguyenLieu != null
            ? monDangYeuCau.danhSachNguyenLieu.Length
            : 0;

        if (soNguyenLieu == 0) return false;

        for (int i = 0; i < soNguyenLieu; i++)
        {
            if (i >= toggleNguyenLieu.Length || toggleNguyenLieu[i] == null || !toggleNguyenLieu[i].isOn)
            {
                return false;
            }
        }

        return true;
    }

    void CapNhatDongHo()
    {
        if (textDongHo != null)
        {
            textDongHo.text = "Thời gian: " + Mathf.CeilToInt(Mathf.Max(0f, thoiGianConLai));
        }
    }
}
