using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buy : MonoBehaviour
{
    [SerializeField, Range(0, 500)]
    private int _prise;

    [SerializeField, Range(1, 3)]
    private int _id;

    [SerializeField]
    private TMP_Text _prise_text;

    internal int prise
    {
        get
        {
            return _prise;
        }
    }
    internal int id
    {
        get
        {
            return _id;
        }
    }

    private void Start()
    {
        _prise_text.text = _prise.ToString();
    }
}
