using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueReplayCaller : MonoBehaviour
{
    //Wrapper for ContinueReplay function so that it can be attached to onClick event for button
    public void ContinueReplay()
    {
        LevelEndManager.ContinueReplay();
    }
}
