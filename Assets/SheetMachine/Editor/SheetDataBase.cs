using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenGames.SheetMachine
{
    public abstract class SheetDataBase :ScriptableObject
    {
        public abstract void Load();
    }
}
