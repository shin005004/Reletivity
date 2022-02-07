using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI speedOfLight;
    public TextMeshProUGUI speedOfPlayer;
    public TextMeshProUGUI gammaText;
    public GameState gameState;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        speedOfLight.text = string.Format("���� �ӵ�: {0:0.00}", gameState.SpeedOfLight);
        speedOfPlayer.text = string.Format("���� �ӵ�: {0:0.00C}", gameState.PlayerVelocityVector.magnitude / gameState.SpeedOfLight);
    }
}
