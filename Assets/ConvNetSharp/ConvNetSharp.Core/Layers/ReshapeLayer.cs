using System;
using System.Collections.Generic;
using ConvNetSharp.Volume;

namespace ConvNetSharp.Core.Layers
{
    public class ReshapeLayer<T> : LayerBase<T> where T : struct, IEquatable<T>, IFormattable
    {
        public ReshapeLayer(Dictionary<string, object> data) : base(data)
        {
        }

        public ReshapeLayer(int width, int height, int depth)
        {
            OutputWidth = width;
            OutputHeight = height;
            OutputDepth = depth;
        }

        protected override Volume<T> Forward(Volume<T> input, bool isTraining = false)
        {
            this.OutputActivation.Storage.CopyFrom(input.Storage);
            return this.OutputActivation;
        }

        public override void Backward(Volume<T> outputGradient)
        {
            this.OutputActivationGradients = outputGradient;
            this.InputActivationGradients.Storage.CopyFrom(outputGradient.Storage);
        }
    }
}