using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinScriptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _coins_Collected;
    private int _count_coins = 0;
    private static CoinScriptUI instance;
    public static CoinScriptUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CoinScriptUI>();
            }
            return instance;
        }
    }
    void Start()
    {
        _count_coins = 0;
        _coins_Collected.text = (_count_coins).ToString();
    }
    public void Add()
    {
        _count_coins++;
        _coins_Collected.text = (_count_coins).ToString();
    }
    public int RetCoinCount() { return _count_coins; }
}
