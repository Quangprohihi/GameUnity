using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [Header("--- GIAI ĐOẠN 1: TÌM CỬA HÀNG LẤY BÁNH ---")]
    public GameObject npcLayHang;
    public Transform[] danhSachViTriLayHang;

    [Header("--- GIAI ĐOẠN 2: TÌM KHÁCH GIAO BÁNH ---")]
    public GameObject khachHang;
    public Transform[] danhSachNhaKhach;

    [Header("--- LA BÀN ---")]
    public ChiDuong muiTenChiDuong;

    [Header("--- UI LỜI NHẮN ---")]
    public GameObject oLoiNhan;
    public TextMeshProUGUI textNoiDung;

    void Start()
    {
        TrangThaiChoNhanDon();
    }

    public void TrangThaiChoNhanDon()
    {
        if (muiTenChiDuong != null) muiTenChiDuong.gameObject.SetActive(false);

        // HIỆN LỜI NHẮN
        if (oLoiNhan != null)
        {
            oLoiNhan.SetActive(true);
            if (textNoiDung != null)
            {
                textNoiDung.text = "Cảm ơn Shipper nhé! Pizza ngon lắm!";
            }
            Debug.Log("<color=green>===> DA BAT DIALOGUE!</color>");
        }

        // Đảm bảo NPC khách hàng vẫn hiện để giữ cái Dialogue trên đầu nó
        if (khachHang != null) khachHang.SetActive(true);

        if (danhSachViTriLayHang.Length > 0 && npcLayHang != null)
        {
            int viTri = Random.Range(0, danhSachViTriLayHang.Length);
            npcLayHang.transform.position = danhSachViTriLayHang[viTri].position;
            npcLayHang.SetActive(true);
        }
    }

    public void TrangThaiDiGiaoHang()
    {
        // Nhận đơn mới thì ẩn lời nhắn cũ đi
        if (oLoiNhan != null) oLoiNhan.SetActive(false);

        if (danhSachNhaKhach.Length > 0 && khachHang != null)
        {
            int viTri = Random.Range(0, danhSachNhaKhach.Length);

            // 1. Chỉnh vị trí và xoay mặt
            khachHang.transform.position = danhSachNhaKhach[viTri].position;
            khachHang.transform.rotation = danhSachNhaKhach[viTri].rotation;
            khachHang.SetActive(true);

            // 2. Reset animation về đứng thở (Idle)
            Animator animKhach = khachHang.GetComponent<Animator>();
            if (animKhach != null)
            {
                animKhach.Rebind();
                animKhach.Update(0f);
            }

            // === MỚI THÊM: ÉP BẬT LẠI MŨI TÊN TRÊN ĐẦU ===
            KhachHangDiChuyen scriptKhach = khachHang.GetComponent<KhachHangDiChuyen>();
            if (scriptKhach != null && scriptKhach.muiTenTrenDau != null)
            {
                scriptKhach.muiTenTrenDau.SetActive(true); // Bật sáng mũi tên!
            }
            // ===========================================
        }

        if (muiTenChiDuong != null)
        {
            muiTenChiDuong.gameObject.SetActive(true);
            muiTenChiDuong.mucTieu = khachHang.transform;
        }
    }
}