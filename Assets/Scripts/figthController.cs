using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class fightController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static int Attack(playerController attacker, playerController defender, gameLogic gameLogic)
    {
        int damage;
        int diff = gameLogic.attack[(int)attacker.role] - gameLogic.defense[(int)defender.role];
        damage = diff + Random.Range(-3, 4);
        if (gameLogic.critChance[(int)attacker.role] > Random.Range(1, 101))
            damage += 5;
        Debug.Log("Damage: " + damage);
        PlaySound((int)attacker.role);
        return damage;
    }

    private static void PlaySound(int role)
    {
        switch (role)
        {
            case 0:
                GameObject.Find("EffectWarrior").GetComponent<AudioSource>().Play();
                break;
            case 1:
                GameObject.Find("EffectBow").GetComponent<AudioSource>().Play();
                break;
            case 2:
                GameObject.Find("EffectMage").GetComponent<AudioSource>().Play();
                break;
            default:
                break;
        }
    }
}