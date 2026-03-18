using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 0.5f; // Tốc độ mây trôi
    public float resetPositionX = 20f; // Điểm mà mây sẽ biến mất
    public float startPositionX = -20f; // Điểm mà mây sẽ xuất hiện lại

    void Update()
    {
        // Di chuyển mây theo trục X
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Nếu mây đi quá xa, đưa nó quay trở lại điểm bắt đầu
        if (transform.position.x > resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}