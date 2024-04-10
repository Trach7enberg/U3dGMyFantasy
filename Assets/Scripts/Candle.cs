﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour {
    public GameObject startEffect;

    private void OnTriggerEnter2D(Collider2D collision) {
        MyLunaController Luna = collision.GetComponent<MyLunaController>();
        if (Luna != null) {
            Instantiate(startEffect, Luna.transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}