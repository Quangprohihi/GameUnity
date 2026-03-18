using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Cài đặt kết nối")]
    public Transform seat;
    public GameObject player;

    [Header("Thông số xe")]
    public float speed = 15f;
    public float turnSpeed = 70f;

    [Header("--- ÂM THANH MÁY XE ---")]
    public AudioSource amThanhDongCo;
    public float doGamNhoNhat = 0.8f; // Tiếng xình xịch lúc xe đứng im
    public float doGamLonNhat = 2.5f; // Tiếng rú ga lúc chạy nhanh nhất

    // --- MỚI THÊM: KHAI BÁO BIẾN CAMERA THEO KIỂU BẬT/TẮT ---
    [Header("--- CAMERA BẬT TẮT ---")]
    public GameObject camNhanVat; // Kéo PlayerFollowCamera vào đây
    public GameObject camXe;      // Kéo Camera Xe (sắp tạo) vào đây

    private bool isInside = false;
    private Rigidbody rb;
    private float moveInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // --- MỚI THÊM: TỰ ĐỘNG TÌM CAMERA NGƯỜI CHƠI ---
        if (camNhanVat == null)
        {
            // Tự động tìm object có tên này gán vào, khỏi cần kéo tay!
            camNhanVat = GameObject.Find("PlayerFollowCamera");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isInside && CheckDistance())
                EnterCar();
            else if (isInside)
                ExitCar();
        }

        if (isInside)
        {
            moveInput = Input.GetAxis("Vertical");   // W/S
            turnInput = Input.GetAxis("Horizontal"); // A/D

            // XỬ LÝ ĐỘ GẦM CỦA ĐỘNG CƠ KHI ĐANG LÁI
            if (amThanhDongCo != null)
            {
                // Lấy tốc độ thực tế của xe
                float tocDo = rb.linearVelocity.magnitude;

                // Tính toán độ gầm rú (chia cho speed để lấy tỷ lệ chuẩn)
                float doGamHienTai = doGamNhoNhat + (tocDo / speed);

                // Giới hạn không cho tiếng kêu quá chói tai
                amThanhDongCo.pitch = Mathf.Clamp(doGamHienTai, doGamNhoNhat, doGamLonNhat);
            }
        }
        else
        {
            moveInput = 0;
            turnInput = 0;
        }
    }

    void FixedUpdate()
    {
        if (isInside) HandleMovement();
    }

    void HandleMovement()
    {
        // Mình đã thêm dấu trừ (-) vào đây để đảo Tiến/Lùi
        Vector3 movement = -transform.forward * moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            float reverseMultiplier = (moveInput > 0) ? 1f : -1f;

            // Mình đã thêm dấu trừ (-) vào đây để đảo Trái/Phải
            float turn = turnInput * turnSpeed * reverseMultiplier * Time.fixedDeltaTime;

            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    void EnterCar()
    {
        isInside = true;

        // 1. Gắn Player vào ghế
        player.transform.SetParent(seat);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        // Bật Animation ngồi (nếu bạn đã cài đặt)
        if (player.TryGetComponent<Animator>(out var anim)) anim.SetBool("isSitting", true);

        // 2. ĐÓNG BĂNG 3 LỚP VẬT LÝ CỦA PLAYER (QUAN TRỌNG NHẤT ĐỂ KHÔNG CHÌM XE)
        if (player.TryGetComponent<CharacterController>(out var cc)) cc.enabled = false;

        // Tắt Box Collider của Player
        if (player.TryGetComponent<BoxCollider>(out var boxCol)) boxCol.enabled = false;

        // Khóa Rigidbody của Player để không bị rơi trọng lực
        if (player.TryGetComponent<Rigidbody>(out var playerRb)) playerRb.isKinematic = true;

        // 3. Tắt các script di chuyển của nhân vật
        MonoBehaviour tpc = player.GetComponent("ThirdPersonController") as MonoBehaviour;
        if (tpc != null) tpc.enabled = false;

        MonoBehaviour playerInput = player.GetComponent("PlayerInput") as MonoBehaviour;
        if (playerInput != null) playerInput.enabled = false;

        // BẬT CHÌA KHÓA NỔ MÁY XE
        if (amThanhDongCo != null) amThanhDongCo.Play();

        // --- MỚI THÊM: BẬT CAMERA XE, TẮT CAMERA NGƯỜI ---
        if (camXe != null) camXe.SetActive(true);
        if (camNhanVat != null) camNhanVat.SetActive(false);
    }

    void ExitCar()
    {
        isInside = false;
        player.transform.SetParent(null);

        // Đẩy nhân vật ra bên phải xe 2 mét để không kẹt vào xe
        player.transform.position += transform.right * 2f;

        // Tắt Animation ngồi
        if (player.TryGetComponent<Animator>(out var anim)) anim.SetBool("isSitting", false);

        // 4. BẬT LẠI 3 LỚP VẬT LÝ CHO PLAYER ĐỂ ĐI BỘ BÌNH THƯỜNG
        if (player.TryGetComponent<CharacterController>(out var cc)) cc.enabled = true;

        // Bật lại Box Collider của Player
        if (player.TryGetComponent<BoxCollider>(out var boxCol)) boxCol.enabled = true;

        // Mở khóa Rigidbody của Player
        if (player.TryGetComponent<Rigidbody>(out var playerRb)) playerRb.isKinematic = false;

        // Bật lại các script di chuyển
        MonoBehaviour tpc = player.GetComponent("ThirdPersonController") as MonoBehaviour;
        if (tpc != null) tpc.enabled = true;

        MonoBehaviour playerInput = player.GetComponent("PlayerInput") as MonoBehaviour;
        if (playerInput != null) playerInput.enabled = true;

        // TẮT MÁY XE
        if (amThanhDongCo != null) amThanhDongCo.Stop();

        // --- MỚI THÊM: TRẢ LẠI CAMERA CHO NGƯỜI ---
        if (camXe != null) camXe.SetActive(false);
        if (camNhanVat != null) camNhanVat.SetActive(true);
    }

    bool CheckDistance()
    {
        return Vector3.Distance(player.transform.position, transform.position) < 4f;
    }
}