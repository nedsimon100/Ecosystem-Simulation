using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject spawnpoint;
    public Text killCount;
    public Text BulletsShot;
    public int currentKills = 0;
    public int currentBullets = 0;

    void Update()
    {
        if (player.activeSelf == false)
        {

            player.SetActive(true);
            player.GetComponent<PlayerController>().spawn();
            player.GetComponent<PlayerController>().man = this;

        }
            
    }
    public void kills(int a)
    {
        if (a == 0)
        {
            currentKills = 0;
        }
        else
        {
            currentKills ++;
        }
        killCount.text = (currentKills.ToString("0"));
    }
    public void Bullets(int a)
    {
        if (a == 0)
        {
            currentBullets = 0;
        }
        else
        {
            currentBullets ++;
        }
        BulletsShot.text = (currentBullets.ToString("0"));
    }
}
