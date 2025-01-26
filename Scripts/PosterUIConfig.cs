
using Sonic853.Udon.UrlLoader;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.PosterUI
{
    public class PosterUIConfig : UdonSharpBehaviour
    {
        public VRCUrl imageUrl;
        public VRCUrl datasUrl;
        public VRC_PortalMarker portalMarker;
        public PosterUI posterUI;
        public UrlImageLoader imageLoader;
        public UrlStringLoader dataLoader;
        void Start()
        {
            if (portalMarker != null) posterUI.portalMarker = portalMarker;
            posterUI.Init();
            if (!string.IsNullOrEmpty(imageUrl.ToString()) && imageLoader != null)
            {
                imageLoader.url = imageUrl;
                imageLoader.LoadUrl();
            }
            if (!string.IsNullOrEmpty(datasUrl.ToString()) && dataLoader != null)
            {
                dataLoader.url = datasUrl;
                dataLoader.LoadUrl();
            }
        }
    }
}
