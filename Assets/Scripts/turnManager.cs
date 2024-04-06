using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class turnManager : MonoBehaviour
{
    [Header("Player 1")] public Transform player1;
    [Header("Player 2")] public Transform player2;

    private selectController _selectControllerPlayer1;
    private selectController _selectControllerPlayer2;
    private List<Transform> _entitiesPlayer1 = new List<Transform>();
    private List<Transform> _entitiesPlayer2 = new List<Transform>();

    private int _gameTurn;
    private int _player1Turn;
    private int _player2Turn;
    

    [Header("PlayerPanel")] public TextMeshProUGUI playerPanel;
    [Header("TurnPanel")] public TextMeshProUGUI turnPanel;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        setBorderColer(player1, Color.green);
        setBorderColer(player2, Color.red);
        foreach (Transform entity in player1)
        {
            Debug.Log(entity.name);
            _entitiesPlayer1.Add(entity);
        }
        foreach (Transform entity in player2)
        {
            _entitiesPlayer2.Add(entity);
        }
        

        _selectControllerPlayer1 = player1.GetComponent<selectController>();
        _selectControllerPlayer2 = player2.GetComponent<selectController>();

        _gameTurn = 1;
        _player1Turn = 0;
        _player2Turn = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        turnPanel.text = "Turn: " + _gameTurn;
        
        if (_player1Turn < _gameTurn)
        {
            _selectControllerPlayer1.enabled = true;
            playerPanel.text = "Current Player: Player 1";
        } else if (_player2Turn < _gameTurn)
        {
            _selectControllerPlayer2.enabled = true;
            playerPanel.text = "Current Player: Player 2";
        }
    }

    private void setBorderColer(Transform player, Color color)
    {
        foreach (Transform child in player)
        {
            child.Find("Square").GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void EndTurn()
    {
        if (_selectControllerPlayer1.enabled)
        {
            setBorderColer(player2, Color.green);
            setBorderColer(player1, Color.red);
            _player1Turn++;
            _selectControllerPlayer1.enabled = false;
        }

        if (_selectControllerPlayer2.enabled)
        {
            setBorderColer(player1, Color.green);
            setBorderColer(player2, Color.red);
            _player2Turn++;
            _selectControllerPlayer2.enabled = false;
            _gameTurn++;
            ResetMovePoints();
        }
    }

    private void ResetMovePoints()
    {
        foreach (Transform unit in _entitiesPlayer1)
        {
            unit.GetComponent<playerController>().resetMovePoints();
        }
        foreach (Transform unit in _entitiesPlayer2)
        {
            unit.GetComponent<playerController>().resetMovePoints();
        }
    }
}
