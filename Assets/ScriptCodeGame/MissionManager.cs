using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [System.Serializable]
    public class GianHangConfig
    {
        [Tooltip("NPC đứng bán ở gian hàng")]
        public GameObject npcGianHang;

        [Tooltip("Điểm spawn của NPC gian hàng (có thể để trống nếu đã đặt sẵn trong scene)")]
        public Transform diemSpawnNpcGianHang;
    }

    [Header("--- ĐIỂM A (NPC ĐẾN ĐẶT ĐƠN) ---")]
    public Transform diemA;
    public float tocDoNPCDiToi = 2.5f;
    public float khoangCachDatDon = 1.2f;

    [Header("--- GIAN HÀNG (MỖI PHẦN TỬ = 1 NPC + 1 ĐIỂM SPAWN) ---")]
    public GianHangConfig[] danhSachGianHang;

    [Header("--- NPC KHÁCH (ĐANG DI CHUYỂN NGẪU NHIÊN TRONG SCENE) ---")]
    public GameObject khachHang;

    [Header("--- LA BÀN ---")]
    public ChiDuong muiTenChiDuong;

    [Header("--- UI LỜI NHẮN ---")]
    public GameObject oLoiNhan;
    public TextMeshProUGUI textNoiDung;

    private bool npcDangDiToiDiemA = false;
    private int soVungDiemADangDung = 0;
    private GameObject npcGianHangDangMucTieu;

    void Start()
    {
        TrangThaiChoNhanDon();
    }

    void Update()
    {
        if (!npcDangDiToiDiemA || khachHang == null || diemA == null) return;

        Vector3 mucTieu = diemA.position;
        mucTieu.y = khachHang.transform.position.y;

        Vector3 huongDi = mucTieu - khachHang.transform.position;
        float khoangCach = huongDi.magnitude;

        if (khoangCach <= khoangCachDatDon)
        {
            npcDangDiToiDiemA = false;
            HienLoiNhanDatHang();
            return;
        }

        if (khoangCach > 0.001f)
        {
            Vector3 huongChuan = huongDi.normalized;
            khachHang.transform.position += huongChuan * tocDoNPCDiToi * Time.deltaTime;
            khachHang.transform.rotation = Quaternion.Slerp(
                khachHang.transform.rotation,
                Quaternion.LookRotation(huongChuan),
                8f * Time.deltaTime
            );
        }
    }

    void HienLoiNhanDatHang()
    {
        if (!DangTrongDiemA()) return;

        if (oLoiNhan != null)
        {
            oLoiNhan.SetActive(true);
            if (textNoiDung != null)
            {
                textNoiDung.text = "Bạn ơi, mua giúp mình đồ ở gian hàng nhé!";
            }
        }

        ShipperManager shipper = FindFirstObjectByType<ShipperManager>();
        if (shipper != null && !shipper.dangGiaoHang)
        {
            shipper.NhanDonHang();
        }
    }

    bool DangTrongDiemA()
    {
        return soVungDiemADangDung > 0;
    }

    void BatDauKhachDiToiDatDon()
    {
        if (diemA == null)
        {
            Debug.LogWarning("MissionManager: Chưa gán Điểm A (diemA).");
            return;
        }

        if (khachHang != null) khachHang.SetActive(true);

        npcDangDiToiDiemA = true;
    }

    public void NguoiChoiVaoDiemA()
    {
        soVungDiemADangDung++;

        ShipperManager shipper = FindFirstObjectByType<ShipperManager>();
        if (shipper == null) return;

        // Chỉ gọi khách đến đặt đơn khi người chơi đang rảnh.
        if (!shipper.dangGiaoHang)
        {
            BatDauKhachDiToiDatDon();
        }
    }

    public void NguoiChoiRaDiemA()
    {
        soVungDiemADangDung = Mathf.Max(0, soVungDiemADangDung - 1);

        if (DangTrongDiemA()) return;

        // Rời Điểm A thì dừng nhận đơn mới từ NPC.
        npcDangDiToiDiemA = false;

        ShipperManager shipper = FindFirstObjectByType<ShipperManager>();
        if (shipper != null && !shipper.dangGiaoHang)
        {
            if (oLoiNhan != null) oLoiNhan.SetActive(false);
        }
    }

    public void TrangThaiChoNhanDon()
    {
        if (muiTenChiDuong != null) muiTenChiDuong.gameObject.SetActive(false);

        if (oLoiNhan != null) oLoiNhan.SetActive(false);

        // Chỉ cho phép gọi đơn khi người chơi đứng trong Điểm A.
        if (DangTrongDiemA())
        {
            BatDauKhachDiToiDatDon();
        }
        else
        {
            npcDangDiToiDiemA = false;
        }

        if (danhSachGianHang != null)
        {
            for (int i = 0; i < danhSachGianHang.Length; i++)
            {
                if (danhSachGianHang[i] == null || danhSachGianHang[i].npcGianHang == null) continue;
                danhSachGianHang[i].npcGianHang.SetActive(false);
            }
        }

        npcGianHangDangMucTieu = null;
    }

    public void TrangThaiDiLayHang()
    {
        npcDangDiToiDiemA = false;

        if (oLoiNhan != null)
        {
            oLoiNhan.SetActive(true);
            if (textNoiDung != null)
            {
                textNoiDung.text = "Đi đến gian hàng để mua vật phẩm khách cần.";
            }
        }

        if (danhSachGianHang == null || danhSachGianHang.Length == 0)
        {
            Debug.LogWarning("MissionManager: Danh sách gian hàng đang trống.");
            return;
        }

        for (int i = 0; i < danhSachGianHang.Length; i++)
        {
            if (danhSachGianHang[i] == null || danhSachGianHang[i].npcGianHang == null) continue;
            danhSachGianHang[i].npcGianHang.SetActive(false);
        }

        int[] chiSoHopLe = new int[danhSachGianHang.Length];
        int soLuongHopLe = 0;

        for (int i = 0; i < danhSachGianHang.Length; i++)
        {
            if (danhSachGianHang[i] != null && danhSachGianHang[i].npcGianHang != null)
            {
                chiSoHopLe[soLuongHopLe] = i;
                soLuongHopLe++;
            }
        }

        if (soLuongHopLe == 0)
        {
            Debug.LogWarning("MissionManager: Không có gian hàng hợp lệ (thiếu NPC).");
            return;
        }

        int chiSoRandom = chiSoHopLe[Random.Range(0, soLuongHopLe)];
        GianHangConfig gianHang = danhSachGianHang[chiSoRandom];

        npcGianHangDangMucTieu = gianHang.npcGianHang;

        if (npcGianHangDangMucTieu != null)
        {
            if (gianHang.diemSpawnNpcGianHang != null)
            {
                npcGianHangDangMucTieu.transform.position = gianHang.diemSpawnNpcGianHang.position;
                npcGianHangDangMucTieu.transform.rotation = gianHang.diemSpawnNpcGianHang.rotation;
            }

            npcGianHangDangMucTieu.SetActive(true);
        }

        if (muiTenChiDuong != null && npcGianHangDangMucTieu != null)
        {
            muiTenChiDuong.gameObject.SetActive(true);
            muiTenChiDuong.mucTieu = npcGianHangDangMucTieu.transform;
        }
    }

    public void TrangThaiDiGiaoHang()
    {
        // Đã lấy hàng, quay lại giao cho chính NPC đã đặt ở Điểm A.
        if (oLoiNhan != null)
        {
            oLoiNhan.SetActive(true);
            if (textNoiDung != null)
            {
                textNoiDung.text = "Đã lấy hàng xong! Giao lại cho NPC ở Điểm A.";
            }
        }

        if (npcGianHangDangMucTieu != null) npcGianHangDangMucTieu.SetActive(false);

        if (khachHang != null)
        {
            khachHang.SetActive(true);

            Animator animKhach = khachHang.GetComponent<Animator>();
            if (animKhach != null)
            {
                animKhach.Rebind();
                animKhach.Update(0f);
            }

            KhachHangDiChuyen scriptKhach = khachHang.GetComponent<KhachHangDiChuyen>();
            if (scriptKhach != null && scriptKhach.muiTenTrenDau != null)
            {
                scriptKhach.muiTenTrenDau.SetActive(true);
            }
        }

        if (muiTenChiDuong != null)
        {
            muiTenChiDuong.gameObject.SetActive(true);
            muiTenChiDuong.mucTieu = khachHang.transform;
        }
    }
}