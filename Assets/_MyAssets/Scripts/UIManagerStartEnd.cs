using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System.Threading;

public class UIManagerStartEnd : UI
{
    [Header("Scène Début")]
    [SerializeField] GameObject _panelPrincipal = default;
    [SerializeField] GameObject _panelInstrction = default;
    [SerializeField] GameObject _boutonDemarrer = default;
    [SerializeField] GameObject _boutonRetourInstructions = default;

    [Header("Scène Fin")]
    [SerializeField] TMP_Text _txtTemps = default(TMP_Text);
    [SerializeField] TMP_Text _txtCollisions = default(TMP_Text);
    [SerializeField] TMP_Text _txtPointage = default(TMP_Text);
    [SerializeField] GameObject _boutonRetourFin= default;

    private bool _intructionsOn = false;

    private void Start()
    {
        if (GameManager._instance != null && SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            AffichageResultats();

        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_boutonDemarrer);
        }
        DestructionGameManager();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            if (gameManager != null)
            {
                Destroy(gameManager);
            }
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_boutonDemarrer);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            
            _txtTemps.text = "Temps : " + GameManager._instance.TempsFin.ToString("f2");
            _txtCollisions.text = "Collisions : " + GameManager._instance.Score;
            float total = GameManager._instance.TempsFin + GameManager._instance.Score;
            _txtPointage.text = "Pointage : " + total.ToString("f2");

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_boutonRetourFin);
        }
    }

    private static void DestructionGameManager()
    {
        if (GameManager._instance != null && SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>(); //FindObjectOfType<GameManager>();
            Destroy(gameManager);
        }
    }

    private void AffichageResultats()
    {
        _txtTemps.text = "Temps : " + (/*GameManager._instance.TempFinal*/Time.time/* - GameManager._instance.TempsDepart*/).ToString("f2") + " sec.";
      //  _txtCollisions.text = "Collisions : " + GameManager._instance.Score;
        float total = (GameManager._instance.TempFinal - GameManager._instance.TimeStart) + GameManager._instance.Score;
        _txtPointage.text = "Pointage final : " + total.ToString("f2") + " sec.";
    }

    public void ToggleInstructions()
    {
        bool toggle = _panelPrincipal.activeSelf;
        _panelPrincipal.SetActive(!toggle);
        _panelInstrction.SetActive(toggle);

        if (!_intructionsOn)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_boutonRetourInstructions);
            _intructionsOn = true;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_boutonDemarrer);
            _intructionsOn = false;
        }
    }
}
