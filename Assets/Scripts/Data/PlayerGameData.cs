using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public class PlayerGameData
    {
        public string Name;
        public string UserID;   
        public int ViewID;
        public int Coins;
        public int Frags;
        public int Deaths;

        public override string ToString()
        {
            return Name + "_" + UserID + "\n" + ViewID + " " + Coins + " " + Deaths;
        }
    }
}
