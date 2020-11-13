using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Specialized;

public class UIController : MonoBehaviour
{
    #region Properties
    [Header("Properties")]
    [SerializeField] private Simulation scene;
    #endregion

    #region Initialisation
    void Awake()
    {
        scene = GameObject.FindWithTag("GameController").GetComponent<Simulation>(); // get reference to Simulation
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }

    // gets all rabbits and sets their scale to scaleMultiplier (called by slider)
    public void SetRabbitSize(float scaleMultiplier)
    {
        Rabbit[] rabbitList = GameObject.FindObjectsOfType<Rabbit>();
        foreach (Rabbit rabbit in rabbitList)
        {
            rabbit.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
        }
    }

    public void SetRabbitSpeed(float speed)
    {
        new Rabbit().SetGlobalBaseMoveSpeed(speed);
    }
}
