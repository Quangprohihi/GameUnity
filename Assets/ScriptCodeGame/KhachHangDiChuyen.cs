using UnityEngine;

public class KhachHangDiChuyen : MonoBehaviour
{
    [Header("--- MŨI TÊN CHỈ ĐIỂM ---")]
    [Tooltip("Kéo cục mũi tên trên đầu NPC vào đây")]
    public GameObject muiTenTrenDau;

    public float tocDoDiBo = 1.5f;
    private Animator anim;
    private bool daQuayLung = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Hàm này sẽ được gọi từ ShipperManager khi giao bánh
    public void BatDauNhanBanh()
    {
        daQuayLung = false;

        if (anim != null) anim.SetTrigger("NhanBanh");

        // === MỚI THÊM: TẮT MŨI TÊN NGAY KHI NHẬN BÁNH ===
        if (muiTenTrenDau != null)
        {
            muiTenTrenDau.SetActive(false);
        }
    }

    void Update()
    {
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

        // === MỚI THÊM: BẬT LẠI MŨI TÊN CHO CUỐC XE MỚI ===
        if (muiTenTrenDau != null)
        {
            muiTenTrenDau.SetActive(true);
        }
    }
}