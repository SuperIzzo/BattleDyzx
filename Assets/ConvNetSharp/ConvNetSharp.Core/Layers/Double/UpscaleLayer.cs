using System.Collections.Generic;

namespace ConvNetSharp.Core.Layers.Double
{
    public class UpscaleLayer : UpscaleLayer<double>
    {
        public UpscaleLayer(Dictionary<string, object> data) : base(data)
        {
        }

        public UpscaleLayer(int width, int height) : base(width, height)
        {
        }
    }
}