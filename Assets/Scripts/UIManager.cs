using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    public Image hpMaskImage;
    public Image mpMaskImage;

    /// <summary>
    /// 血条和蓝条原始宽度
    /// </summary>
    private float originalSize;

    // Awake先于 start()方法执行
    void Awake() {
        Instance = this;
        originalSize = hpMaskImage.rectTransform.rect.width;
    }

    /// <summary>
    /// 同时设置血条和蓝条的宽度图片百分比,1.0为满
    /// </summary>
    /// <param name="hpFillPercent">血条填充百分比</param>
    /// <param name="MpFillPercent">蓝条填充百分比</param>
    public void SetBar(float hpFillPercent, float MpFillPercent) {
        // 通过RectTransform的锚点来设置图片的宽度,第一个参数我们用锚点的水平轴向,第二个size是填充的百分比
        hpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpFillPercent * originalSize);
        mpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MpFillPercent * originalSize);
    }

}
