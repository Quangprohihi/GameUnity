using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("Cài đặt tiền thưởng")]
    public float tienThuong = 500f; // Số tiền sẽ cộng khi về đích

    private void OnTriggerEnter(Collider other)
    {
        // 1. Kiểm tra xem người chạm vào vạch đích có đúng là Xe của người chơi không
        if (other.CompareTag("Player"))
        {
            // 2. Tìm cái Ví tiền (ShipperManager) trong Scene
            ShipperManager shipper = FindFirstObjectByType<ShipperManager>();

            if (shipper != null)
            {
                // 3. Gọi hàm phát tiền!
                shipper.NhanTienThuongVeDich(tienThuong);

                // 4. (Quan trọng) Biến mất vạch đích để người chơi không lùi xe lại ăn tiền liên tục
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}