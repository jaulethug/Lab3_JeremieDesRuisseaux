using System.Collections.Generic;
using UnityEngine;

public class GestionPieges : MonoBehaviour
{
    [SerializeField] private GameObject _boulderTrap;
    [SerializeField] private float _intensiteForce = 1000;

    [Header("Vecteur pour le déplacement de l'obstacle")]
    [SerializeField] private float _directionX = 0;
    [SerializeField] private float _directionY = -1;
    [SerializeField] private float _directionZ = 0;

    private Rigidbody _rbTrap;
    private Vector3 _direction;
    private bool _isTrigger = false;
    // Timer pour savoir quand réinitialiser les pièges
    private float _MaxTimeReset = 5;
    private float _timeSinceActivated = 0;
    private Vector3 _lastPosition;// le tier incrémenter

    private void Start()
    {
        _direction = new Vector3(_directionX, _directionY, _directionZ);
        _rbTrap = _boulderTrap.GetComponent<Rigidbody>();
        _lastPosition = _rbTrap.transform.position;
    }

    private void Update()
    {
        if (_isTrigger)
        {
            _timeSinceActivated += Time.deltaTime;

            if (_timeSinceActivated >= _MaxTimeReset)
            {
                _rbTrap.gameObject.SetActive(false);
                _rbTrap.useGravity = false;
                _isTrigger = false;
                _rbTrap.transform.position = _lastPosition;
                _timeSinceActivated = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !_isTrigger)
        {
            _rbTrap.gameObject.SetActive(true);
            _rbTrap.useGravity = true;
            _rbTrap.AddForce(_direction * _intensiteForce);
            _isTrigger = true;
        }
    }
}
