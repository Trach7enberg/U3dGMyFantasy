using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ����Ѫƿ���ָ������Ƕ���Ч����������ʱ��
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
