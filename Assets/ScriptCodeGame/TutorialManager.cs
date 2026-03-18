using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    void Update()
    {
        // Liên tục kiểm tra: Nếu người chơi ấn phím X (trên bàn phím)
        if (Input.GetKeyDown(KeyCode.X))
        {
            TatBangHuongDan();
        }
    }

    public void TatBangHuongDan()
    {
        // 1. Tắt cái bảng Hướng dẫn đi
        gameObject.SetActive(false);

        // 2. Rã đông thời gian (để xe tiếp tục chạy, game hoạt động bình thường)
        Time.timeScale = 1f;
    }
}