using UnityEngine;

public class KhachHangDiChuyen : MonoBehaviour
{
    [Header("--- MŨI TÊN CHỈ ĐIỂM ---")]
    [Tooltip("Kéo cục mũi tên trên đầu NPC vào đây")]
    public GameObject muiTenTrenDau;

    [Header("--- ĐIỂM RỜI ĐI SAU KHI NHẬN HÀNG ---")]
    [Tooltip("NPC sẽ chọn ngẫu nhiên 1 điểm trong danh sách này để đi ra chỗ khác")]
    public Transform[] danhSachDiemRoiDi;
    public float khoangCachDenDich = 0.5f;

    public float tocDoDiBo = 1.5f;
    private Animator anim;
    private bool daQuayLung = false;
    private bool dangRoiDi = false;
    private Transform diemRoiDiHienTai;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void ChonDiemRoiDiNgauNhien()
    {
        if (danhSachDiemRoiDi == null || danhSachDiemRoiDi.Length == 0)
        {
            dangRoiDi = false;
            diemRoiDiHienTai = null;
            return;
        }

        int viTriNgauNhien = Random.Range(0, danhSachDiemRoiDi.Length);
        diemRoiDiHienTai = danhSachDiemRoiDi[viTriNgauNhien];
        dangRoiDi = diemRoiDiHienTai != null;
    }

    // Gọi khi NPC đã đặt đơn xong và muốn đi ra chỗ khác ngay.
    public void BatDauDiRoiDi()
    {
        daQuayLung = false;
        ChonDiemRoiDiNgauNhien();
    }

    // Hàm này sẽ được gọi từ ShipperManager khi giao bánh
    public void BatDauNhanBanh()
    {
        daQuayLung = false;
        dangRoiDi = false;
        diemRoiDiHienTai = null;

        if (anim != null) anim.SetTrigger("NhanBanh");

        // === MỚI THÊM: TẮT MŨI TÊN NGAY KHI NHẬN BÁNH ===
        if (muiTenTrenDau != null)
        {
            muiTenTrenDau.SetActive(false);
        }

        ChonDiemRoiDiNgauNhien();
    }

    void Update()
    {
        if (dangRoiDi && diemRoiDiHienTai != null)
        {
            Vector3 mucTieu = diemRoiDiHienTai.position;
            mucTieu.y = transform.position.y;

            Vector3 huongDi = mucTieu - transform.position;
            float khoangCach = huongDi.magnitude;

            if (khoangCach <= khoangCachDenDich)
            {
                dangRoiDi = false;
                return;
            }

            Vector3 huongChuan = huongDi.normalized;
            transform.position += huongChuan * tocDoDiBo * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(huongChuan),
                8f * Time.deltaTime
            );
            return;
        }

        if (anim == null) return;

        AnimatorStateInfo trangThaiHienTai = anim.GetCurrentAnimatorStateInfo(0);

        if (trangThaiHienTai.IsName("locom_m_basicWalk_30f"))
        {
            if (daQuayLung == false)
            {
                transform.Rotate(0, 180, 0);
                daQuayLung = true;
            }

            transform.Translate(Vector3.forward * tocDoDiBo * Time.deltaTime);
        }
    }

    // Reset lại trạng thái khi NPC bay sang nhà mới
    private void OnEnable()
    {
        daQuayLung = false;
        dangRoiDi = false;
        diemRoiDiHienTai = null;

        // === MỚI THÊM: BẬT LẠI MŨI TÊN CHO CUỐC XE MỚI ===
        if (muiTenTrenDau != null)
        {
            muiTenTrenDau.SetActive(true);
        }
    }
}