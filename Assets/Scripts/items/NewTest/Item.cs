using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
//classe abstraite pour enfant et atteindre partout.
public abstract class Item
{
    //poids de loot
    public abstract int dropWeight { get; set;} 
    public abstract string GiveName();

    public bool Upgraded = false;

    //Methode update si  on veux modifier des trucs tt le temps
    public virtual void Update(PlayerController playerMov, PlayerHealth playerHealth1layerHealth, PlayerSouls soulScript, ParticleSystem ItemUpgrade){
    
    }

    //Si on veux ajouter une méthode dans un item il FAUT l'ajouter ici d'abord (ou  l'appeller depuis une méthode ici) pour pas avoir a faire 50k if avec les  enfants
}

//item 1  s'appelle  comme ça parceque oui
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

//item 2
public class TestItemOne : Item
{
    public override int dropWeight
    {
        get { return dropWeight; }
        set { dropWeight = 45; }
    }

    public override string GiveName()
    {
        return "Test Passive Item one";
    }

    public override void Update(PlayerController playerMov, PlayerHealth playerHealth1layerHealth, PlayerSouls soulScript, ParticleSystem ItemUpgrade)
    {
        int currentSouls = soulScript.souls;
        Debug.Log(currentSouls + " in item");
        if (currentSouls >= 5)
        {
            playerMov.dashDuration = 0.5f;
            playerMov.dashDistance = 8f;
            playerMov.dashCooldown = 0.25f;
            if(Upgraded != true)
            {
                Debug.Log("play Effect");
                ItemUpgrade.Play();
                Upgraded = true;
            }
        }
        else
        {
            playerMov.dashDuration = 0.5f;
            playerMov.dashDistance = 8f;
        }

    }
}

    //item 3 est sensé être actif
    public class TestActiveItem : Item
    {
        public override int dropWeight
        {
            get { return dropWeight; }
            set { dropWeight = 25; }
        }

        public override string GiveName()
        {
            return "Test Active Item";
        }
    }

