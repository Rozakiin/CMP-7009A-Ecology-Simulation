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

    public void SetRabbitSizeMale(float scale)
    {
        new Rabbit().SetGlobalBaseMaleScale(scale);
    }

    public void SetRabbitSizeFemale(float scale)
    {
        new Rabbit().SetGlobalBaseFemaleScale(scale);
    }

    public void SetRabbitSpeed(float speed)
    {
        new Rabbit().SetGlobalBaseMoveSpeed(speed);
    }

    public void SetRabbitMaxHunger(float hunger)
    {
        new Rabbit().SetGlobalMaxHunger(hunger);
    }

    public void SetRabbitMaxThirst(float thirst)
    {
        new Rabbit().SetGlobalMaxThirst(thirst);
    }

    public void SetRabbitMaxAge(float age)
    {
        new Rabbit().SetGlobalMaxThirst((int)age);
    }

    public void SetRabbitPregnancyLength(float length)
    {
        new Rabbit().SetGlobalBasePregnancyLength(length);
    }
}
