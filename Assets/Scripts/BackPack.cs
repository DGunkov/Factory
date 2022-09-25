using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack : MonoBehaviour
{
    private GameObject[] _res_in_backpack;
    private int[] _res_id_in_backpack;
    private int _count_res;
    [SerializeField]
    private Transform _first_position_backpack;
    private Export _last_export;
    private Import _last_import;
    private UI UI;
    private bool _start_sell;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_to_sell;
    private float _time_to_sell;

    private GameObject _res_sell;
    private Vector3 _start_position;
    private Vector3 _target_position;
    private int _count_sell_money;

    internal Vector3 target_position
    {
        get
        {
            return new Vector3(0, 1.6f * _count_res, 0);
        }
    }
    internal Transform first_position_backpack
    {
        get
        {
            return _first_position_backpack;
        }
    }
    private void Start()
    {
        UI = Camera.main.GetComponent<UI>();
        _res_in_backpack = new GameObject[5];
        _res_id_in_backpack = new int[5];
    }
    private void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Sell")
        {
            if(_count_res > 0)
            _start_sell = true;
        }
        if (obj.gameObject.TryGetComponent(out Buy Buy))
        {
            if(Buy.prise <= UI.money)
            {
                UI.NewMoney(-Buy.prise);
                UI.Buy(Buy.id);
            }
        }
        if (obj.gameObject.TryGetComponent(out Export Export) && _count_res < 5)
        {
            Export.SwitchExportStay(true);
            _last_export = Export;
        }
        if (obj.gameObject.TryGetComponent(out Import Import) && _count_res > 0)
        {
            Import.SwitchImportStay(true);
            _last_import = Import;
        }
    }
    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Sell")
        {
            _start_sell = false;
        }
        if (obj.gameObject.TryGetComponent(out Export Export))
        {
            Export.SwitchExportStay(false);
        }
        if (obj.gameObject.TryGetComponent(out Import Import))
        {
            Import.SwitchImportStay(false);
        }
    }
    internal GameObject SearchRes(int id)
    {
        for(int i = _count_res - 1; i >= 0; i--)
        {
            if(_res_id_in_backpack[i] == id)
            {
                UI.DeleteRes(id);
                _count_res--;
                GameObject obj = _res_in_backpack[i];
                _res_in_backpack[i] = null;
                _res_id_in_backpack[i] = 0;

                for (int n = 0; n < _count_res; n++)
                {
                    if(_res_in_backpack[n] == null)
                    {
                        for (int nn = n + 1; nn < 5; nn++)
                        {
                            if (_res_in_backpack[nn] != null)
                            {
                                _res_in_backpack[n] = _res_in_backpack[nn];
                                _res_id_in_backpack[n] = _res_id_in_backpack[nn];
                                _res_in_backpack[n].transform.position = _first_position_backpack.position + new Vector3(0, 1.6f * n, 0);
                                _res_in_backpack[nn] = null;
                                _res_id_in_backpack[nn] = 0;
                                break;
                            }
                        }
                    }
                }
                return obj;
            }
        }
        return null;
    }
    internal void NewRes(GameObject res, int id)
    {
        _res_in_backpack[_count_res] = res;
        _res_id_in_backpack[_count_res] = id;
        res.transform.position = _first_position_backpack.position + new Vector3(0, 1.6f * _count_res, 0);
        res.transform.rotation = transform.rotation;
        _count_res++;
        UI.NewRes(id);
        if(_count_res >= 5)
        {
            _last_export.SwitchExportStay(false);
        }
    }
    private void Sell()
    {
        for (int n = _count_res - 1; n >= 0; n--)
        {
            if(_res_in_backpack[n] != null)
            {
                switch(_res_id_in_backpack[n])
                {
                    case 1:
                        _count_sell_money = 10;
                        break;
                    case 2:
                        _count_sell_money = 30;
                        break;
                    case 3:
                        _count_sell_money = 80;
                        break;
                }
                UI.DeleteRes(_res_id_in_backpack[n]);
                _count_res--;
                _res_sell = _res_in_backpack[n];
                _res_in_backpack[n] = null;
                _res_id_in_backpack[n] = 0;
                _time_to_sell = _fixed_time_to_sell;
                _start_position = _res_sell.transform.position;
                _target_position = _start_position + new Vector3(0, 20, 0);
                _res_sell.transform.parent = null;
                return;
            }
        }
    }
    private void Update()
    {
        if (_time_to_sell > 0)
        {
            _time_to_sell -= Time.deltaTime;
            if(_res_sell != null)
            {
                _res_sell.transform.position = Vector3.Lerp(_start_position, _target_position, 1 - _time_to_sell * (1 / _fixed_time_to_sell));
            }
        }
        else
        {
            if (_res_sell != null)
            {
                UI.NewMoney(_count_sell_money);
                Destroy(_res_sell);
                _res_sell = null;
            }
            if (_start_sell)
            {
                if(_count_res > 0)
                {
                    Sell();
                }
                else
                {
                    _start_sell = false;
                }
            }
        }
    }
}
