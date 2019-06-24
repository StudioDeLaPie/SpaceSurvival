using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pression;

public class UIPlayer : MonoBehaviour
{
    public EnvironmentPlayerDetector envDetector;
    public PlayerHealth playerHealth;
    public PlayerRespiration playerRespiration;

    [Space]
    public TextMeshProUGUI envPressure;
    public TextMeshProUGUI envValO;
    public TextMeshProUGUI envValG;
    public TextMeshProUGUI envValV;
    public TextMeshProUGUI envRatO;
    public TextMeshProUGUI envRatG;
    public TextMeshProUGUI envRatV;
    [Space]
    public TextMeshProUGUI playerVie;
    public TextMeshProUGUI playerOxygene;
    [Space]
    public TextMeshProUGUI txtAimFeedback;
    public GameObject helpPanel;

    private Conteneur _environmentConteneur;
    private bool aimFeedbackDisplayed = false;

    public void FeedbackAim(string feedback, int indexLigne = 1)
    {
        if (indexLigne == 1)
            txtAimFeedback.text = feedback;
        if (indexLigne == 2)
            txtAimFeedback.text = txtAimFeedback.text + "\n" + feedback;

        aimFeedbackDisplayed = true;
    }

    private void Start()
    {
        envDetector.OnConteneurChange += ConteneurChange;
        _environmentConteneur = envDetector.EnvironmentConteneur;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }

        if (_environmentConteneur == null)
            _environmentConteneur = envDetector.EnvironmentConteneur;
        RefreshUI();
    }

    private void RefreshUI()
    {
        envPressure.text = PressionGestion.PascalToBar(_environmentConteneur.Pressure).ToString("F4");

        envValO.text = _environmentConteneur.nbParticulesOxygene.ToString("F0");
        envValG.text = _environmentConteneur.nbParticulesGaz.ToString("F0");
        envValV.text = _environmentConteneur.nbParticulesVapeur.ToString("F0");

        envRatO.text = (_environmentConteneur.Ratio(EGases.Oxygene) * 100).ToString("F0") + "%";
        envRatG.text = (_environmentConteneur.Ratio(EGases.Gaz) * 100).ToString("F0") + "%";
        envRatV.text = (_environmentConteneur.Ratio(EGases.Vapeur) * 100).ToString("F0") + "%";

        playerVie.text = playerHealth.Vie.ToString("F0");
        playerOxygene.text = playerRespiration.Oxygen.ToString("F1");

        if (aimFeedbackDisplayed) //S'il a été actualisé dans cette frame on fait rien
            aimFeedbackDisplayed = false;
        else                        //Sinon on l'efface
            txtAimFeedback.text = "";
    }

    private void ConteneurChange()
    {
        _environmentConteneur = envDetector.EnvironmentConteneur;
    }

}
