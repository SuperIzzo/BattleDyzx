using System;
using System.Collections.Generic;
using ConvNetSharp.Volume;

namespace ConvNetSharp.Core.Layers
{
    public class UpscaleLayer<T> : LayerBase<T> where T : struct, IEquatable<T>, IFormattable
    {
        public UpscaleLayer(Dictionary<string, object> data) : base(data)
        {
            this.Width = Convert.ToInt32(data["Width"]);
            this.Height = Convert.ToInt32(data["Height"]);
            this.IsInitialized = true;
        }

        public UpscaleLayer(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public override void Backward(Volume<T> outputGradient)
        {
            // For back propagation we just get the average gradient
            for (int x = 0; x < InputActivationGradients.Shape.Dimensions[0]; x++)
            {
                for (int y = 0; y < InputActivationGradients.Shape.Dimensions[1]; y++)
                {
                    for (int c = 0; c < InputActivationGradients.Shape.Dimensions[2]; c++)
                    {
                        for (int n = 0; n < InputActivationGradients.Shape.Dimensions[3]; n++)
                        {
                            T meanGradient = Ops<T>.Zero;

                            for (int xx = 0; xx < Width; xx++)
                            {
                                for (int yy = 0; yy < Height; yy++)
                                {
                                    meanGradient = Ops<T>.Add(meanGradient, outputGradient.Get((x * Width) + xx, (y * Height) + yy, c, n));                                    
                                }
                            }

                            meanGradient = Ops<T>.Divide(meanGradient, Ops<T>.Cast(Width * Height));
                            InputActivationGradients.Set(x, y, c, n, meanGradient);
                        }
                    }
                }
            }
        }

        protected override Volume<T> Forward(Volume<T> input, bool isTraining = false)
        {
            for (int x = 0; x < input.Shape.Dimensions[0]; x++)
            {
                for (int y = 0; y < input.Shape.Dimensions[1]; y++)
                {
                    for (int xx=0; xx<Width; xx++)
                    {
                        for (int yy=0; yy<Height; yy++)
                        {
                            for (int c = 0; c < input.Shape.Dimensions[2]; c++)
                            {
                                for (int n = 0; n < input.Shape.Dimensions[3]; n++)
                                {
                                    OutputActivation.Set(x * Width + xx, y * Height + yy, c, n, input.Get(x, y, c, n));
                                }
                            }
                        }
                    }
                }
            }

            return OutputActivation;
        }

        public override void Init(int inputWidth, int inputHeight, int inputDepth)
        {
            base.Init(inputWidth, inputHeight, inputDepth);
            this.UpdateOutputSize();
        }

        private void UpdateOutputSize()
        {
            // computed
            this.OutputDepth = this.InputDepth;
            this.OutputWidth = this.InputWidth * this.Width;
            this.OutputHeight = this.InputHeight * this.Height;
        }
    }
}
