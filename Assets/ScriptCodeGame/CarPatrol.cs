using UnityEngine;
using UnityEngine.AI;

public class CarPatrol : MonoBehaviour
{
    [Header("Danh sách điểm tuần tra")]
    public Transform[] danhSachDiem;

    [Header("--- CỔNG DỊCH CHUYỂN ---")]
    public bool batDichChuyen = false; // Tích vào để bật tính năng biến hình
    public int diemBatDauDichChuyen = 2; // Ví dụ: Điểm C là số 2 (vì máy tính đếm từ 0)
    public int diemDichChuyenToi = 3;    // Ví dụ: Điểm D là số 3

    [Header("Cấu hình Phạt Tiền")]
    public int soTienPhat = 100;

    private NavMeshAgent agent;
    private int diemHienTaiIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (danhSachDiem.Length > 0 && danhSachDiem[0] != null)
        {
            agent.SetDestination(danhSachDiem[0].position);
        }
    }

    void Update()
    {
        if (agent.isOnNavMesh)
        {
            // Xe cách điểm đích dưới 5 mét
            if (!agent.pathPending && agent.remainingDistance < 5.0f)
            {
                // KIỂM TRA XEM CÓ ĐANG Ở CỔNG DỊCH CHUYỂN KHÔNG?
                if (batDichChuyen && diemHienTaiIndex == diemBatDauDichChuyen)
                {
                    ThucHienDichChuyen();
                }
                else
                {
                    ChuyenSangDiemTiepTheo();
                }
            }
        }
    }

    void ThucHienDichChuyen()
    {
        // 1. Phép thuật biến hình: Dịch chuyển tức thời xe đến điểm D
        agent.Warp(danhSachDiem[diemDichChuyenToi].position);

        // 2. Cập nhật lại "trí nhớ" cho xe là nó đang ở điểm D rồi
        diemHienTaiIndex = diemDichChuyenToi;

        // 3. Ra lệnh cho xe tiếp tục chạy bình thường sang điểm sau điểm D
        ChuyenSangDiemTiepTheo();
    }

    void ChuyenSangDiemTiepTheo()
    {
        if (danhSachDiem.Length == 0) return;

        // Tự động nhảy số (0 -> 1 -> 2 -> 3 -> quay về 0)
        diemHienTaiIndex = (diemHienTaiIndex + 1) % danhSachDiem.Length;

        if (danhSachDiem[diemHienTaiIndex] != null)
        {
            agent.SetDestination(danhSachDiem[diemHienTaiIndex].position);
        }
    }

    // --- PHẦN TRỪ TIỀN KHI ĐỤNG TRÚNG ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShipperManager manager = FindFirstObjectByType<ShipperManager>();
            if (manager != null) manager.BiTruTien(soTienPhat);

            Rigidbody rbNguoiChoi = other.GetComponent<Rigidbody>();
            if (rbNguoiChoi != null)
            {
                Vector3 huongDay = (other.transform.position - transform.position).normalized;
                rbNguoiChoi.AddForce(huongDay * 500f + Vector3.up * 200f);
            }
        }
    }
}