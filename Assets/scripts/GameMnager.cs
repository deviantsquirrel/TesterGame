using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMnager : MonoBehaviour
{
    [SerializeField] private GameObject _You_lose;

    public int _Amount_0f_players = 0;
    private bool LastOneRem = false;
    private static GameMnager instance;
    public static GameMnager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameMnager>();
            }
            return instance;
        }
    }
    public void GameEnd(bool res, int ind)
    {
        GameObject endscreen = Instantiate(_You_lose, transform.position, transform.rotation);
        endscreen.GetComponent<LoseScreem>().SetConis(CoinScriptUI.Instance.RetCoinCount());
        endscreen.GetComponent<LoseScreem>().SetResult(res, ind);
    }
    public int CountPlayer()
    {
        _Amount_0f_players++;
        return _Amount_0f_players;
    }
    public void SmbDied()
    {
        _Amount_0f_players--;
        
        if (_Amount_0f_players == 1) { LastOneRem=true; }
    }
    public bool AmITheLastManStanding()
    {
        return LastOneRem;
    }
}
