using UnityEngine;
using UnityEngine.AI;

public class CarPatrol : MonoBehaviour
{
    [Header("Danh sách điểm tuần tra (Theo thứ tự)")]
    public Transform[] danhSachDiem;

    [Header("Cấu hình Phạt Tiền")]
    public int soTienPhat = 100;

    private NavMeshAgent agent;
    private int diemHienTaiIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Tắt tự động hãm phanh để xe đi qua các điểm mượt hơn, không bị giật cục
        agent.autoBraking = false;

        DiChuyenDenDiemHienTai();
    }

    void Update()
    {
        if (agent.isOnNavMesh)
        {
            // Kiểm tra xem xe đã thực sự đến sát điểm đích hiện tại chưa (cách dưới 1 mét)
            if (!agent.pathPending && agent.remainingDistance < 1.0f)
            {
                ChuyenSangDiemTiepTheo();
            }
        }
    }

    void DiChuyenDenDiemHienTai()
    {
        if (danhSachDiem.Length > 0 && danhSachDiem[diemHienTaiIndex] != null)
        {
            agent.SetDestination(danhSachDiem[diemHienTaiIndex].position);
        }
    }

    void ChuyenSangDiemTiepTheo()
    {
        if (danhSachDiem.Length == 0) return;

        // Ép buộc xe phải đi theo đúng thứ tự trong mảng (0 -> 1 -> 2 -> 3 -> quay lại 0)
        diemHienTaiIndex = (diemHienTaiIndex + 1) % danhSachDiem.Length;

        // Cập nhật đích đến mới
        DiChuyenDenDiemHienTai();
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