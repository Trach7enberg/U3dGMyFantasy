using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float maxHealth { get; private set; }
    [Range(0, 5)]
    public float currentHealth;

    private void Awake() {
        Instance = this;

        maxHealth = 5;
        SetFullHealth();

    }

    private void Update() {
        UpdateBar();
    }

    public void IncreaseHealth(int hp) {
        currentHealth = Mathf.Clamp(currentHealth + hp, 0, maxHealth);

    }

    private void SetFullHealth() {
        currentHealth = maxHealth;
    }


    /// <summary>
    /// 血条和蓝条UI更新
    /// </summary>
    private void UpdateBar() {
        // 蓝条功能暂时没有完成,待更新
        UIManager.Instance.SetBar(currentHealth / maxHealth, 1);
    }
}
