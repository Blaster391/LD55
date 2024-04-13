using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player Player { get; private set; }


    void Start()
    {
        Player = GetComponentInChildren<Player>();

        Instance = this;
    }
}
