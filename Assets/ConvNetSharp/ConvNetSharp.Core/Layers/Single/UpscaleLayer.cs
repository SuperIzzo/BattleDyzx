using System.Collections.Generic;

namespace ConvNetSharp.Core.Layers.Single
{
    public class UpscaleLayer : UpscaleLayer<float>
    {
        public UpscaleLayer(Dictionary<string, object> data) : base(data)
        {
        }

        public UpscaleLayer(int width, int height) : base(width, height)
        {
        }
    }
}