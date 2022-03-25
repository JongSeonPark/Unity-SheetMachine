using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CreateAssetMenu(fileName = "GoogleMachine ", menuName = "SheetMachine/GoogleMachine", order = 0)]
    public class GoogleMachine : BaseMachine
    {
        private void Awake()
        {
            templatePath = GoogleDataSettings.Instance.templatePath;
            editorClassPath = GoogleDataSettings.Instance.editorClassPath;
            runtimeClassPath = GoogleDataSettings.Instance.runtimeClassPath;
        }

    }
}
