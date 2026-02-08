using System;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int value;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Game.Instance.audioManager.PlayMoney();
            Game.Instance.player.money += value;
            Game.Instance.UpdateMoneyUI(Game.Instance.player.money);
            
            Destroy(gameObject);
        }
    }
    
}
