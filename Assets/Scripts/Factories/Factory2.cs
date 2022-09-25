using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory2 : MonoBehaviour
{
    [SerializeField]
    private Transform _spawn_point;
    [SerializeField]
    private Transform _end_spawn_point;
    [SerializeField]
    private Transform _start_conveyor;
    [SerializeField]
    private Transform _end_conveyor;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_spawn;
    private float _time_to_new_produce;
    private float _time_to_move_res;

    [SerializeField]
    private GameObject _res_prefab;
    private GameObject _res_last_spawn;
    private GameObject _res_on_conveyor;

    private Export _export_res;
    private Import _import_res;

    private void Start()
    {
        _export_res = transform.Find("Export").GetComponent<Export>();

        _import_res = transform.Find("Import").GetComponent<Import>();

        _time_to_move_res = _fixed_time_spawn;
    }

    private void Update()
    {
        if(_import_res.res_on_conveyor != null)
        {
            if(_res_on_conveyor == null)
            {
                _res_on_conveyor = _import_res.res_on_conveyor;
                _import_res.res_on_conveyor = null;
            }
        }
        if (_res_on_conveyor != null)
        {
            if (_time_to_move_res > 0)
            {
                _time_to_move_res -= Time.deltaTime;
                if (_res_on_conveyor != null)
                    _res_on_conveyor.transform.position = Vector3.Lerp(_start_conveyor.position, _end_conveyor.position, 1 - _time_to_move_res * (1 / _fixed_time_spawn));
            }
            else
            {
                if (_export_res.Count_res < 25)
                {
                    _time_to_move_res = _fixed_time_spawn;
                    _import_res.ConveyorEmpty();
                    _time_to_new_produce = _fixed_time_spawn;
                    _res_last_spawn = Instantiate(_res_prefab, _spawn_point.position, transform.rotation);
                    Destroy(_res_on_conveyor);
                    _res_on_conveyor = null;
                }
            }
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
