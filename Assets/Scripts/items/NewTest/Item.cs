using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class Item
{
    public abstract int dropWeight { get; set;} 
    public abstract string GiveName();
    public virtual void Update(PlayerController playerMov, PlayerHealth playerHealth1layerHealth){
    
    }
}

public class HealingItem:Item
{
    public override int dropWeight 
    { 
        get {return dropWeight;}
        set {dropWeight = 30;} 
    }

    public override string GiveName(){
        return "Healing item";
    }
}

public class TestItemOne:Item
{
    public override int dropWeight 
    { 
        get {return dropWeight;}
        set {dropWeight = 45;} 
    }

    public override string GiveName()
    {
        return "Test Passive Item one";
    }

}


public class TestActiveItem:Item
{
    public override int dropWeight 
    { 
        get {return dropWeight;}
        set {dropWeight = 25;}
    }

    public override string GiveName()
    {
        return "Test Active Item";
    }
}

