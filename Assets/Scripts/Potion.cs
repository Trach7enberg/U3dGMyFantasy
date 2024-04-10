﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 血瓶药剂类
 */
public class Potion : MonoBehaviour {

    public GameObject startEffect;

    private void OnTriggerEnter2D(Collider2D collision) {
        MyLunaController Luna = collision.GetComponent<MyLunaController>();
        if (Luna != null) {
            if (Luna.currentHealth < Luna.maxHealth) {
                Luna.IncreaseHealth(1);

                // 利用Instantiate函数生成游戏物体
                // 生成的是healEffect的副本,不然到时候不能销毁,因为销毁healEffect相当于销毁prefab,所以需要它的副本
                //GameObject a = Instantiate(startEffect, Luna.transform.position, Quaternion.identity) as GameObject;
                Instantiate(startEffect, Luna.transform.position, Quaternion.identity);

                Destroy(this.gameObject);
            }

        }
    }
}
