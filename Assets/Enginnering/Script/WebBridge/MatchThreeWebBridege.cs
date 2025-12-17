using UnityEngine;

public class MatchThreeWebBridege : WebGLBridge
{
    public override void PauseGame()
    {
        base.PauseGame();
        Time.timeScale = 0;
    }
    
    public override void ResumeGame()
    {
        base.ResumeGame();
        Time.timeScale = 1;
    }
}
