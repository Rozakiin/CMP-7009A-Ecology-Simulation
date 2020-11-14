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
    private Simulation scene;
    private Rabbit rabbit;
    private Fox fox;
    private Grass grass;
    #endregion

    #region Initialisation
    void Awake()
    {
        scene = GameObject.FindWithTag("GameController").GetComponent<Simulation>(); // get reference to Simulation
        rabbit = gameObject.AddComponent<Rabbit>() as Rabbit;
        fox = gameObject.AddComponent<Fox>() as Fox;
        grass = gameObject.AddComponent<Grass>() as Grass;
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
        rabbit.SetGlobalBaseMaleScale(scale);
    }

    public void SetRabbitSizeFemale(float scale)
    {
        rabbit.SetGlobalBaseFemaleScale(scale);
    }

    public void SetRabbitSpeed(float speed)
    {
        rabbit.SetGlobalBaseMoveSpeed(speed);
    }

    public void SetRabbitMaxHunger(float hunger)
    {
        rabbit.SetGlobalMaxHunger(hunger);
    }

    public void SetRabbitMaxThirst(float thirst)
    {
        rabbit.SetGlobalMaxThirst(thirst);
    }

    public void SetRabbitMaxAge(float age)
    {
        rabbit.SetGlobalMaxThirst((int)age);
    }

    public void SetRabbitPregnancyLength(float length)
    {
        rabbit.SetGlobalBasePregnancyLength(length);
    }
}
