using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Export : MonoBehaviour
{
    [SerializeField, Range(1, 3)]
    private int _default_res;

    private GameObject _res_last_spawn;
    private GameObject _res_move_backpack;
    private GameObject[,] _res_on_platform;

    [SerializeField]
    private Transform _first_storage_point;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_move_storage;
    private float _time_move_storage;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_move;
    private float _time_to_move_backpack;

    private Vector3 _start_move_position_storage;
    private Vector3 _start_move_position_backpack;
    private Vector3 _target_position_storage;
    private Vector3 _target_position_backpack;

    private Transform first_position_backpack;

    private int _count_res;

    private bool _start_export;

    private BackPack _player_backpack;

    internal int Count_res
    {
        get
        {
            return _count_res;
        }
    }   

    private void Start()
    {
        _time_move_storage = _fixed_time_move_storage;
        _res_on_platform = new GameObject[5, 5];

        _player_backpack = GameObject.FindGameObjectWithTag("Player").transform.Find("Player").GetComponent<BackPack>();

        first_position_backpack = _player_backpack.first_position_backpack;
    }

    internal void MoveNewResStorage(GameObject obj)
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (_res_on_platform[y, x] == null)
                {
                    _count_res++;
                    _res_on_platform[y, x] = obj;
                    _res_last_spawn = obj;
                    obj.transform.SetParent(transform);
                    _target_position_storage = _first_storage_point.position + new Vector3(4 * x, 0, -4 * y);
                    _start_move_position_storage = _res_last_spawn.transform.position;
                    _time_move_storage = _fixed_time_move_storage;
                    return;
                }
            }
        }
    }
    private void MoveResBackPack()
    {
        for (int y = 4; y >= 0; y--)
        {
            for (int x = 4; x >= 0; x--)
            {
                if (_res_on_platform[y, x] != null)
                {
                    _count_res--;
                    _res_move_backpack = _res_on_platform[y, x];
                    _res_on_platform[y, x] = null;
                    _res_last_spawn = null;
                    _start_move_position_backpack = _res_move_backpack.transform.position;
                    _target_position_backpack = _player_backpack.target_position;
                    return;
                }
            }
        }
    }

    internal void SwitchExportStay(bool stay)
    {
        _start_export = stay;
    }

    private void Update()
    {        
        if (_time_move_storage > 0)
        {
            _time_move_storage -= Time.deltaTime;
            if (_res_last_spawn != null)
                _res_last_spawn.transform.position = Vector3.Lerp(_start_move_position_storage, _target_position_storage, 1 - _time_move_storage * (1 / _fixed_time_move_storage));
        }
        else
        {
            _res_last_spawn = null;
        }
        if (_time_to_move_backpack > 0 && _res_move_backpack != null)
        {
            _time_to_move_backpack -= Time.deltaTime;
            _res_move_backpack.transform.position = Vector3.Lerp(_start_move_position_backpack, first_position_backpack.position + _target_position_backpack, 1 - _time_to_move_backpack * (1 / _fixed_time_move));
        }
        else
        {
            if (_res_move_backpack != null)
            {
                _res_move_backpack.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
                _player_backpack.NewRes(_res_move_backpack, _default_res);
                _res_move_backpack = null;
            }
            if (_start_export)
            {
                _time_to_move_backpack = _fixed_time_move;
                MoveResBackPack();
            }
        }
    }
}
