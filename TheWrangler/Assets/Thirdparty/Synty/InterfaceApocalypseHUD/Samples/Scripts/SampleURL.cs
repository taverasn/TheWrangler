using UnityEngine;

namespace Synty.Interface.ApocalypseHUD.Samples
{
    /// <summary>
    ///     Opens a URL in the default browser.
    /// </summary>
    public class SampleURL : MonoBehaviour
    {
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
