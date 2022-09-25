using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Import : MonoBehaviour
{
    [SerializeField]
    private Transform _first_storage_point;

    [SerializeField, Range(1, 3)]
    private int _default_res;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_take_res;
    private float _time_take_res;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_factory_res;
    private float _time_factory_res;

    private BackPack _backpack;    

    private GameObject _last_take_res;
    private GameObject _res_on_conveyor;
    private GameObject[,] _res_on_platform;
    private GameObject _res_move_factory;

    private int _count_res;

    private bool _start_import;
    private bool _conveyor_empty = true;

    private Vector3 _start_move_position_storage;
    private Vector3 _start_move_position_factory;
    private Vector3 _target_position_storage;

    [SerializeField]
    private Transform _conveyor;

    internal GameObject res_on_conveyor
    {
        get
        {
            return _res_on_conveyor;
        }
        set
        {
            _res_on_conveyor = value;
        }
    }

    void Start()
    {
        _backpack = GameObject.FindGameObjectWithTag("Player").transform.Find("Player").GetComponent<BackPack>();
        _res_on_platform = new GameObject[5, 5];
        _time_factory_res = _fixed_time_factory_res;
    }

    private void TakeNewRes()
    {
        _last_take_res = _backpack.SearchRes(_default_res);
        if(_last_take_res != null)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (_res_on_platform[y, x] == null)
                    {
                        _count_res++;
                        _res_on_platform[y, x] = _last_take_res;
                        _last_take_res.transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                        _last_take_res.transform.SetParent(transform);
                        _target_position_storage = _first_storage_point.position + new Vector3(4 * x, 0, -4 * y);
                        _start_move_position_storage = _last_take_res.transform.position;
                        return;
                    }
                }
            }
        }
        else
        {
            _start_import = false;
        }
    }
    private void MoveResFactory()
    {
        for (int y = 4; y >= 0; y--)
        {
            for (int x = 4; x >= 0; x--)
            {
                if (_res_on_platform[y, x] != null)
                {
                    _res_move_factory = _res_on_platform[y, x];
                    _res_on_platform[y, x] = null;
                    _last_take_res = null;
                    _start_move_position_factory = _res_move_factory.transform.position;
                    if (_count_res >= 25)
                    {
                        _start_import = false;
                    }
                    return;
                }
            }
        }
    }
    internal void SwitchImportStay(bool stay)
    {
        _start_import = stay;           
    }
    internal void ConveyorEmpty()
    {
        _conveyor_empty = true;
        _time_factory_res = _fixed_time_factory_res;
    }
    void Update()
    {
        if(_conveyor_empty && _count_res > 0)
        {
            if(_res_move_factory == null)
            {
                MoveResFactory();
            }
            if(_time_factory_res > 0)
            {
                _time_factory_res -= Time.deltaTime;
                if (_res_move_factory != null)
                _res_move_factory.transform.position = Vector3.Lerp(_start_move_position_factory, _conveyor.position, 1 - _time_factory_res * (1 / _fixed_time_factory_res));
            }
            else
            {
                _count_res--;
                _res_on_conveyor = _res_move_factory;
                _res_move_factory = null;
                _conveyor_empty = false;
            }
        }
        if (_time_take_res > 0)
        {
            _time_take_res -= Time.deltaTime;
            if (_last_take_res != null)
                _last_take_res.transform.position = Vector3.Lerp(_start_move_position_storage, _target_position_storage, 1 - _time_take_res * (1 / _fixed_time_take_res));
        }
        else
        {
            _last_take_res = null;
            if (_start_import && _count_res < 25)
            {
                _time_take_res = _fixed_time_take_res;
                TakeNewRes();
            }
        }        
    }
}
