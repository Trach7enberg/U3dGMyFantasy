using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 控制血瓶、恢复、星星动画效果结束销毁时间
 */

public class EffectControl : MonoBehaviour
{
    public float destroy = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,destroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
