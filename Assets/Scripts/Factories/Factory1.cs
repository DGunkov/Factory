using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory1 : MonoBehaviour
{
    [SerializeField]
    private Transform _spawn_point;
    [SerializeField]
    private Transform _end_spawn_point;

    [SerializeField, Range(0.1f, 10f)]
    private float _fixed_time_spawn;
    private float _time_to_new_produce;

    [SerializeField]
    private GameObject _res_prefab;
    private GameObject _res_last_spawn;

    private Export _export_res;

    private void Start()
    {
        _export_res = transform.Find("Export").GetComponent<Export>();

        _time_to_new_produce = _fixed_time_spawn;
    }
    
    private void Update()
    {
        if (_time_to_new_produce > 0)
        {
            _time_to_new_produce -= Time.deltaTime;
            if (_res_last_spawn != null)
                _res_last_spawn.transform.position = Vector3.Lerp(_spawn_point.position, _end_spawn_point.position, 1 - _time_to_new_produce * (1 / _fixed_time_spawn));
        }
        else
        {
            if (_export_res.Count_res < 25)
            {
                if(_res_last_spawn != null)
                {
                    _export_res.MoveNewResStorage(_res_last_spawn);
                }
                _time_to_new_produce = _fixed_time_spawn;
                _res_last_spawn = Instantiate(_res_prefab, _spawn_point.position, transform.rotation);
            }            
        }
    }
}
