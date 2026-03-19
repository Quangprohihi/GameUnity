using System;
using UnityEngine;

[Serializable]
public class FoodRecipeData
{
    [Header("Thông tin món")]
    public string tenMon;
    public float thoiGianCheBien = 20f;

    [Header("Nguyên liệu cần có")]
    public string[] danhSachNguyenLieu;
}
