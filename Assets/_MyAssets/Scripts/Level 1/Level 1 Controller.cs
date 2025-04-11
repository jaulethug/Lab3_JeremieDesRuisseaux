using UnityEngine;

public class Level1Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _listTorches;
    [SerializeField]
    private GameObject _collapsableWall;
    [SerializeField]
    private PlayerNew _playerNew;

    private bool[] _torchesStatus;
    private bool _allTorchActive = false;
    private bool _wallCollapsed = false;

    public static Level1Controller Instance;
    public PlayerNew PlayerNew { get { return _playerNew; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _torchesStatus = new bool[_listTorches.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (!_allTorchActive)
        {
            CheckAllTorchActive();
        }
        else if (!_wallCollapsed && _allTorchActive)
        {
            OpenCenterRoomDoor();
        }
    }

    private void CheckAllTorchActive()
    {
        var nbr = 0;
        foreach (var item in _torchesStatus)
        {
            if (item == true)
            {
                nbr++;
            }
        }
        if (nbr == _torchesStatus.Length)
        {
            _allTorchActive = true;
        }
        else
        {
            _allTorchActive = false;
        }
    }

    private void OpenCenterRoomDoor()
    {
        _collapsableWall.transform.position= new Vector3(_collapsableWall.transform.position.x, -10, _collapsableWall.transform.position.z);
        _wallCollapsed = true;
    }

    public void ActivateTorch(int torchId)
    {
        if (!_torchesStatus[(torchId - 1)])
        {
            _listTorches[(torchId - 1)].GetComponent<Animator>().SetBool("Active", true);
            _torchesStatus[(torchId - 1)] = true;
        }
        else
        {
            _listTorches[(torchId - 1)].GetComponent<Animator>().SetBool("Active", false);
            _torchesStatus[(torchId - 1)] = false;
        }
    }
}
