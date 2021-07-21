using System.Collections.Generic;

namespace ConvNetSharp.Core.Layers.Single
{
    public class ReshapeLayer : ReshapeLayer<float>
    {
        public ReshapeLayer(Dictionary<string, object> data) : base(data)
        {
        }

        public ReshapeLayer(int width, int height, int depth) : base(width, height, depth)
        {
        }
    }
}