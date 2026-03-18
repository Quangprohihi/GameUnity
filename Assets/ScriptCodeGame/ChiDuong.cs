using UnityEngine;

public class ChiDuong : MonoBehaviour
{
    [Header("Cái mà mũi tên sẽ chỉ vào")]
    public Transform mucTieu;

    void Update()
    {
        // Nếu có mục tiêu thì mũi tên mới hoạt động
        if (mucTieu != null)
        {
            // Tính toán hướng nhìn về mục tiêu
            Vector3 huongChi = mucTieu.position - transform.position;
            huongChi.y = 0; // Giữ cho mũi tên luôn nằm ngang song song mặt đất (không bị cắm xuống đất)

            // Xoay mũi tên về hướng đó
            transform.rotation = Quaternion.LookRotation(huongChi);
        }
    }
}