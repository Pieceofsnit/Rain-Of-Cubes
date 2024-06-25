using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private MeshRenderer _renedererColor;
    private Coroutine _coroutine;
    private Rigidbody _rigidbody;
    private float _minLifeTime = 2f;
    private float _maxLifeTime = 5f;
    private bool _isTouched = false;

    public event UnityAction<Cube> OnReleased;

    private void Awake()
    {
        _renedererColor = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ResetParameters();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched == false)
        {
            if (collision.collider.TryGetComponent(out Platform platform))
            {
                _renedererColor.material.color = Random.ColorHSV();
                _coroutine = StartCoroutine(WaitForDeactivation());
                _isTouched = true;
            }
        }
    }

    private IEnumerator WaitForDeactivation()
    {
        float currentLifeTime = Random.Range(_minLifeTime, _maxLifeTime);
        yield return new WaitForSeconds(currentLifeTime);
        Deactivate();
    }

    private void ResetParameters()
    {
        _rigidbody.velocity = Vector3.zero;
        _renedererColor.material.color = Color.white;
        _isTouched = false;
    }

    private void Deactivate()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        OnReleased?.Invoke(this);
    }
}
