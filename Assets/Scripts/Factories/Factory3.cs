using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory3 : MonoBehaviour
{
    [SerializeField]
    private Transform _spawn_point;
    [SerializeField]
    private Transform _end_spawn_point;
    [SerializeField]
    private Transform _start_conveyor1;
    [SerializeField]
    private Transform _start_conveyor2;
    [SerializeField]
    private Transform _end_conveyor1;
    [SerializeField]
    private Transform _end_conveyor2;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_spawn;
    private float _time_to_new_produce;
    private float _time_to_move_res1;
    private float _time_to_move_res2;

    private bool _res1_ready;
    private bool _res2_ready;

    [SerializeField]
    private GameObject _res_prefab;
    private GameObject _res_last_spawn;
    private GameObject _res_on_conveyor1;
    private GameObject _res_on_conveyor2;

    private Export _export_res;
    private Import _import_res1;
    private Import _import_res2;

    private void Start()
    {
        _export_res = transform.Find("Export").GetComponent<Export>();
        _import_res1 = transform.Find("Import1").GetComponent<Import>();
        _import_res2 = transform.Find("Import2").GetComponent<Import>();

        _time_to_move_res1 = _fixed_time_spawn;
        _time_to_move_res2 = _fixed_time_spawn;
    }

    private void Update()
    {
        if (_import_res1.res_on_conveyor != null)
        {
            if (_res_on_conveyor1 == null)
            {
                _res_on_conveyor1 = _import_res1.res_on_conveyor;
                _import_res1.res_on_conveyor = null;
            }
        }
        if (_import_res2.res_on_conveyor != null)
        {
            if (_res_on_conveyor2 == null)
            {
                _res_on_conveyor2 = _import_res2.res_on_conveyor;
                _import_res2.res_on_conveyor = null;
            }
        }

        if (_res_on_conveyor1 != null)
        {
            if (_time_to_move_res1 > 0)
            {
                _time_to_move_res1 -= Time.deltaTime;
                if (_res_on_conveyor1 != null)
                    _res_on_conveyor1.transform.position = Vector3.Lerp(_start_conveyor1.position, _end_conveyor1.position, 1 - _time_to_move_res1 * (1 / _fixed_time_spawn));
            }
            else
            {
                if (_export_res.Count_res < 25)
                {
                    _res1_ready = true;
                    Destroy(_res_on_conveyor1);
                    _res_on_conveyor1 = null;
                }
            }
        }
        if (_res_on_conveyor2 != null)
        {
            if (_time_to_move_res2 > 0)
            {
                _time_to_move_res2 -= Time.deltaTime;
                if (_res_on_conveyor2 != null)
                    _res_on_conveyor2.transform.position = Vector3.Lerp(_start_conveyor2.position, _end_conveyor2.position, 1 - _time_to_move_res2 * (1 / _fixed_time_spawn));
            }
            else
            {
                if (_export_res.Count_res < 25)
                {
                    _res2_ready = true;
                    Destroy(_res_on_conveyor2);
                    _res_on_conveyor2 = null;
                }
            }
        }

        if(_res1_ready && _res2_ready)
        {
            _res1_ready = false;
            _res2_ready = false;
            _time_to_move_res1 = _fixed_time_spawn;
            _time_to_move_res2 = _fixed_time_spawn;
            _import_res1.ConveyorEmpty();
            _import_res2.ConveyorEmpty();
            _res_last_spawn = Instantiate(_res_prefab, _spawn_point.position, transform.rotation);
            _time_to_new_produce = _fixed_time_spawn;
        }

        if (_res_last_spawn != null)
        {
            if (_time_to_new_produce > 0)
            {
                _time_to_new_produce -= Time.deltaTime;
                _res_last_spawn.transform.position = Vector3.Lerp(_spawn_point.position, _end_spawn_point.position, 1 - _time_to_new_produce * (1 / _fixed_time_spawn));
            }
            else
            {
                _export_res.MoveNewResStorage(_res_last_spawn);
                _res_last_spawn = null;
            }
        }
    }
}
