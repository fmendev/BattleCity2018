using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	void Start ()
    {
        StartCoroutine(PlayEagleTooltipAnimation());
	}

    private IEnumerator PlayEagleTooltipAnimation()
    {
        //This wrapper function is just to add a delay before the first tooltip shows up
        yield return new WaitForSeconds(1.5f);
        TooltipController.PlayTooltipAnimation(Tooltip.Eagle);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
