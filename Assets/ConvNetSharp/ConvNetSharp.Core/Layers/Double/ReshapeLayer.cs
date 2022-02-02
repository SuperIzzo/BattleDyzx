using System.Collections.Generic;

namespace ConvNetSharp.Core.Layers.Double
{
    public class ReshapeLayer : ReshapeLayer<double>
    {
        public ReshapeLayer(Dictionary<string, object> data) : base(data)
        {
        }

        public ReshapeLayer(int width, int height, int depth) : base(width, height, depth)
        {
        }
    }
}