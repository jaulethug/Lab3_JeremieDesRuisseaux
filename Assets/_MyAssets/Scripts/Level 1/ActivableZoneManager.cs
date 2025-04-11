using UnityEngine;

public class ActivableZoneManager : MonoBehaviour
{
    [SerializeField] private int _zoneNbr;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Level1Controller.Instance.PlayerNew.SetInActiveZone(_zoneNbr);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Level1Controller.Instance.PlayerNew.SetOutActiveZone();
        }
    }
}
