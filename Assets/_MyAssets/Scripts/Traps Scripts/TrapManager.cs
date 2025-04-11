using UnityEngine;
using System.Collections.Generic;
using System;

public class TrapManager : MonoBehaviour
{
    [Header("Extra")]
    [SerializeField] private List<GameObject> _listTrap = new List<GameObject>();
    [SerializeField] private float _intensityForce = 1000;


    [Header("Vecteur déplacement obstacles")]
    [SerializeField] private float _directionX = 0;
    [SerializeField] private float _directionY = -1;
    [SerializeField] private float _directionZ = 0;

    [Header("Trap Rotational speed")]
    [SerializeField] private float _rotationspeed = 100;
    private float _numberAngleRotation = 180;
    private float _currentRotationPosition = 0f;

    [Header("Axe trap rotation sens")]
    [SerializeField] private bool _axeTrapRotateRight = false;

    private List<Rigidbody> _listRb = new List<Rigidbody>();
    private bool _OnCooldown = false;
    private bool _isRotating = false;
    private Vector3 _direction;

    private float _initialPositionsX = 0;
    private float _initialPositionsY = 0;
    private float _initialPositionsZ = 0;

    // Timer pour savoir quand réinitialiser les pièges
    private float _MaxTimeReset = 4;
    private float _timeSinceActivated = 0; // le tier incrémenter


    private void Start()
    {
        _direction = new Vector3(_directionX, _directionY, _directionZ);

        // Récupérer les Rigidbody et définir une direction aléatoire pour chaque piège
        foreach (var trap in _listTrap)
        {
            _listRb.Add(trap.GetComponent<Rigidbody>());
            if (trap.CompareTag("Canon_Trap"))
            {
                trap.gameObject.SetActive(false);
                _initialPositionsX = trap.transform.position.x;
                _initialPositionsY = trap.transform.position.y;
                _initialPositionsZ = trap.transform.position.z;
            }

        }
    }

    // Por les traps qui rotates constamment
    private void Update()
    {
        if (_isRotating)
        {
            Trunk_Trap_Rotation();
        }

        if (gameObject.CompareTag("Axe_Trap"))
        {
            Axe_Trap_Rotation();
        }

        if (gameObject.CompareTag("Spider_Trap"))
        {
            // Spider trap code

        }


        if (_OnCooldown)
        {
            _timeSinceActivated += Time.deltaTime;

            if (_timeSinceActivated >= _MaxTimeReset)
            {
                TrapsResets();
                _timeSinceActivated = 0;
            }

        }
    }

    private void OnTriggerEnter(Collider triggerTrap)
    {
        if (triggerTrap.CompareTag("Player") && !_OnCooldown)
        {

            foreach(var rbTrap in _listRb)
            {
                rbTrap.gameObject.SetActive(true);

                if (rbTrap.CompareTag("Trunk_Trap"))
                {
                    _isRotating= true;
                }
                if (rbTrap.CompareTag("Boulder_Trap"))
                {
                    rbTrap.useGravity = true;
                }
                if (rbTrap.CompareTag("Canon_Trap"))
                {
                    rbTrap.gameObject.SetActive(true);
                    rbTrap.AddForce(_direction * _intensityForce);
                    rbTrap.useGravity = true;
                }
                else
                {
                    rbTrap.AddForce(_direction * _intensityForce);
                    rbTrap.useGravity = true;
                }
                _OnCooldown = true;
            }
        }
    }

    private void TrapsResets()
    {
        foreach (var rbTrap in _listRb)
        {
            if (rbTrap.CompareTag("Trunk_Trap"))
            {
                rbTrap.transform.Rotate(0, 0, -_numberAngleRotation);
                
            }
            if(rbTrap.CompareTag("Boulder_Trap"))
            {
                rbTrap.useGravity = false;
                rbTrap.transform.position = new Vector3(rbTrap.transform.position.x, 6, rbTrap.transform.position.z);
                rbTrap.transform.rotation = Quaternion.Euler(0,0,0);
            }
            if (rbTrap.CompareTag("Canon_Trap"))
            {
                rbTrap.gameObject.SetActive(false);
                rbTrap.useGravity = false;

                rbTrap.transform.position = new Vector3(_initialPositionsX, _initialPositionsY, _initialPositionsZ);
                rbTrap.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }
        _isRotating = false;
        _currentRotationPosition = 0;
        _OnCooldown = false;
    }

    private void Trunk_Trap_Rotation()
    {
        foreach (var rbTrap in _listRb)
        {
            if (rbTrap.CompareTag("Trunk_Trap"))
            {

                if (_currentRotationPosition < _numberAngleRotation)
                {
                    var rotationDone = _rotationspeed * Time.deltaTime;
                    // dans le négatif, car sinon la trap bouge vers le haut
                    rbTrap.transform.Rotate(0, 0, -rotationDone);
                    _currentRotationPosition += rotationDone;


                    if (rotationDone >= _numberAngleRotation)
                    {
                        _currentRotationPosition = _numberAngleRotation;
                        rbTrap.transform.rotation = Quaternion.Euler(0, 0, 90);
                        _isRotating = false;
                    }

                }
            }
        }
    }

    private void Axe_Trap_Rotation()
    {
        foreach (var rbTrap in _listRb)
        {
            if (rbTrap.CompareTag("Axe_Trap"))
            {
                if (_axeTrapRotateRight)
                {
                    rbTrap.transform.Rotate(0, 0, _rotationspeed);
                }
                else
                {
                    rbTrap.transform.Rotate(0, 0, -_rotationspeed);
                }
                    
                //rbTrap.transform.rotation = Quaternion.Euler(0, 0, 180); // Rotation entre 0 et 180 degrés
            }
        }
    }
}
