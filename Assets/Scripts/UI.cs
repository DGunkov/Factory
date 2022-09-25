using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] _res_on_backpack_text;
    [SerializeField]
    private TMP_Text _money_text;
    private int[] _res_on_backpack;
    private int _money;

    [SerializeField]
    private GameObject[] _buy_platform;
    [SerializeField]
    private GameObject[] _factory;


    internal int money
    {
        get
        {
            return _money;
        }
    }

    void Start()
    {
        _money_text.text = _money.ToString();
        _res_on_backpack = new int[3];
    }

    internal void Buy(int id)
    {
        _buy_platform[id - 1].SetActive(false);
        _factory[id - 1].SetActive(true);
        if(id != 3)
        {
            _buy_platform[id].SetActive(true);
        }
    }

    internal void NewRes(int id)
    {
        _res_on_backpack[id - 1]++;
        _res_on_backpack_text[id - 1].text = _res_on_backpack[id - 1].ToString();
    }
    internal void DeleteRes(int id)
    {
        _res_on_backpack[id - 1]--;
        _res_on_backpack_text[id - 1].text = _res_on_backpack[id - 1].ToString();
    }

    internal void NewMoney(int money)
    {
        _money += money;
        _money_text.text = _money.ToString();
    }
}
