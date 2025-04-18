using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    private bool _isHit;
    private Material _originalMat;
    [SerializeField] private Material _materialHit = default(Material);
    [SerializeField] private float resetDelay = 4f; // Temps avant de restaurer le matériau original
    private float _timeSinceHit = 0f; // Temps écoulé depuis la collision

    private void Start()
    {
        _isHit = false;
        _originalMat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (_isHit)
        {
            _timeSinceHit += Time.deltaTime;

            // Vérifiez si le délai est atteint
            if (_timeSinceHit >= resetDelay)
            {
                _isHit = false;
                _timeSinceHit = 0f; // Réinitialiser le temps écoulé
                GetComponent<MeshRenderer>().material = _originalMat; // Restaurer le matériau original
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!_isHit && this.gameObject.tag != "End_Game")
            {
                GetComponent<MeshRenderer>().material = _materialHit;
                GameManager._instance.AddScore();
                _isHit = true;
                _timeSinceHit = 0f; // Réinitialiser le compteur de temps
            }
            else if (!_isHit && this.gameObject.tag == "End_Game")
            {
                int numScene = SceneManager.GetActiveScene().buildIndex;

                if (numScene >= SceneManager.sceneCountInBuildSettings - 1)
                {
                    Debug.Log("End of the game!");
                    GameManager._instance.setLevelTimeAndCollision(Time.time, numScene);
                    collision.gameObject.SetActive(false);
                  //  GameManager._instance.DisplayEndGameMessages();
                }
                else
                {
                    GameManager._instance.setLevelTimeAndCollision(Time.time, numScene);
                    SceneManager.LoadScene(numScene + 1);
                }
            }
        }
    }
}
