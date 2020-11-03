using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Edible : MonoBehaviour
{
protected bool canBeEaten { get; set; }
    public int baseNutritionalValue { get; set; }
    public int nutritionalValue;
    public Vector3 position;
    public Simulation scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public abstract int GetNutritionalValue();

    public void LowerNutritionalValue(int value)
    {
        nutritionalValue -= value;
    }

    public abstract void SetNutritionalValue();

    public void SetPosition()
    {
        position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    
    public void Die()
    {
        //scene.RemoveFromList(this, scene.GetGrassList());
        //scene.RemoveFromList(this);
        Destroy(gameObject);
    }
}
