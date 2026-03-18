using UnityEngine;
using UnityEngine.AI;

public class ObstacleMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    // Không cần mảng diemDen nữa, thay bằng bán kính
    public float banKinhDiChuyen = 20f;
    public float soTienPhat = 20f;

    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        timer = 0;

        // Bắt đầu game là tìm điểm đi luôn
        DiChuyenDenDiemMoi();
    }

    void Update()
    {
        if (agent == null) return;

        // Xử lý Animation
        bool dangDi = agent.velocity.sqrMagnitude > 0.1f && agent.remainingDistance > 0.5f;
        if (anim != null) anim.SetBool("isMoving", dangDi);

        // Kiểm tra xem đã đến nơi chưa
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Đứng chơi 3-5 giây rồi đi tiếp
            timer += Time.deltaTime;
            if (timer > Random.Range(3f, 5f))
            {
                DiChuyenDenDiemMoi();
                timer = 0;
            }
        }
    }

    // Hàm thông minh: Tự tìm điểm trên mặt đường NavMesh
    void DiChuyenDenDiemMoi()
    {
        Vector3 randomDirection = Random.insideUnitSphere * banKinhDiChuyen;
        randomDirection += transform.position;

        NavMeshHit hit;
        // Tìm điểm hợp lệ trên NavMesh gần vị trí ngẫu nhiên đó
        if (NavMesh.SamplePosition(randomDirection, out hit, banKinhDiChuyen, agent.areaMask))
        {
            agent.SetDestination(hit.position);
        }
    }

    // Hàm trừ tiền (Giữ nguyên)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShipperManager manager = FindFirstObjectByType<ShipperManager>();
            if (manager != null)
            {
                manager.BiTruTien(soTienPhat);
                manager.BiVaCham(10f);
            }
        }
    }
}