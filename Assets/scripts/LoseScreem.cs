using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseScreem : MonoBehaviour
{
    [SerializeField] private TMP_Text _coins_Collected;
    [SerializeField] private TMP_Text _Result_Text;
    [SerializeField] private TMP_Text _Name_Text;
    [SerializeField] private TMP_Text _Name_Name;
    private string _Name;

    public void SetConis(int coins)
    {
        _coins_Collected.text = (coins).ToString();
    }
    public void SetResult(bool won, int id)
    {
        _Result_Text.text = (won)?"YOU WON":"YOU LOST";
        if (won)
        {
            SetName(id);
        }
    }
    public void SetName(int id)
    {
        switch (id)
        {
            case 1:
                _Name = "Yellow";
                break;
            case 2:
                _Name = "Blue";
                break;
            case 3:
                _Name = "Green";
                break;
            case 4:
                _Name = "Red";
                break;
            case 5:
                _Name = "Black";
                break;
            case 6:
                _Name = "White";
                break;
            case 7:
                _Name = "Magenta";
                break;
            case 8:
                _Name = "Grey";
                break;
            default:
                break;
        }

        _Name_Name.text = _Name;
        _Name_Text.text = "Winner's name is:";
    }
}
