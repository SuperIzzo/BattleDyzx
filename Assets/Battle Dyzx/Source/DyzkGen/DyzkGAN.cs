using System.Collections;
using System.Collections.Generic;
using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers.Single;
using ConvNetSharp.Core.Training;
using ConvNetSharp.Core.Training.Single;
using ConvNetSharp.Volume;
using ConvNetSharp.Volume.GPU.Single;

namespace BattleDyzx
{
    public class DyzkGAN
    {
        private Net<float> discriminatorModel;
        private Net<float> generatorModel;
        private TrainerBase<float> discriminatorTrainer;


        public void GenerateImage(IImageData imageData)
        {
            Shape latentShape = new Shape(64);
            var latentVectors = BuilderInstance.Volume.Random(latentShape);

            var generatedVolume = generatorModel.Forward(latentVectors);
            generatedVolume.ToImageData(imageData);
        }

        public void Build()
        {
            BuildDiscriminator();
            BuildGenerator();
        }

        private void BuildDiscriminator()
        {
            discriminatorModel = new Net<float>();

            discriminatorModel.AddLayer(new InputLayer(256, 256, 4));
            discriminatorModel.AddLayer(new ConvLayer(5, 5, 8) { Stride = 1, Pad = 2 });
            discriminatorModel.AddLayer(new LeakyReluLayer(0.2f));
            discriminatorModel.AddLayer(new PoolLayer(2, 2) { Stride = 2 });
            discriminatorModel.AddLayer(new ConvLayer(5, 5, 8) { Stride = 2, Pad = 2 });
            discriminatorModel.AddLayer(new LeakyReluLayer(0.2f));
            discriminatorModel.AddLayer(new PoolLayer(3, 3) { Stride = 2 });
            discriminatorModel.AddLayer(new FullyConnLayer(64));
            discriminatorModel.AddLayer(new LeakyReluLayer(0.2f));
            discriminatorModel.AddLayer(new FullyConnLayer(1));
            discriminatorModel.AddLayer(new SigmoidLayer());
            discriminatorModel.AddLayer(new RegressionLayer());

            var adamTrainer = new AdamTrainer(discriminatorModel);

            discriminatorTrainer = adamTrainer;
        }

        private void BuildGenerator()
        {
            generatorModel = new Net<float>();

            generatorModel.AddLayer(new InputLayer(1, 1, 64));
            generatorModel.AddLayer(new FullyConnLayer(4 * 4 * 4));
            generatorModel.AddLayer(new ReshapeLayer(4, 4, 4));

            generatorModel.AddLayer(new UpscaleLayer(2, 2)); // 8x8
            generatorModel.AddLayer(new ConvLayer(3, 3, 8) { Stride = 1, Pad = 1 });
            generatorModel.AddLayer(new LeakyReluLayer(0.2f));

            generatorModel.AddLayer(new UpscaleLayer(2, 2)); // 16x16
            generatorModel.AddLayer(new ConvLayer(3, 3, 8) { Stride = 1, Pad = 1 });
            generatorModel.AddLayer(new LeakyReluLayer(0.2f));

            generatorModel.AddLayer(new UpscaleLayer(2, 2)); // 32x32
            generatorModel.AddLayer(new ConvLayer(3, 3, 8) { Stride = 1, Pad = 1 });
            generatorModel.AddLayer(new LeakyReluLayer(0.2f));

            generatorModel.AddLayer(new UpscaleLayer(2, 2)); // 64x64
            generatorModel.AddLayer(new ConvLayer(3, 3, 8) { Stride = 1, Pad = 1 });
            generatorModel.AddLayer(new LeakyReluLayer(0.2f));

            generatorModel.AddLayer(new UpscaleLayer(2, 2)); // 128x128
            generatorModel.AddLayer(new ConvLayer(3, 3, 8) { Stride = 1, Pad = 1 });
            generatorModel.AddLayer(new LeakyReluLayer(0.2f));

            generatorModel.AddLayer(new UpscaleLayer(2, 2)); // 256x256
            generatorModel.AddLayer(new ConvLayer(3, 3, 8) { Stride = 1, Pad = 1 });
            generatorModel.AddLayer(new ConvLayer(3, 3, 4) { Stride = 1, Pad = 1 });
            generatorModel.AddLayer(new LeakyReluLayer(0.2f));
        }

        public void TrainReal(IImageData[] images)
        {
            int batchSize = images.Length;

            Shape sampleShape = new Shape(256, 256, 4, batchSize);
            Shape resultShape = new Shape(1, 1, 1, batchSize);


            var sampleVolume = BuilderInstance.Volume.SameAs(sampleShape);
            var resultVolume = BuilderInstance.Volume.SameAs(resultShape);

            for (int n = 0; n < batchSize; n++)
            {
                sampleVolume.SetImageData(n, images[n]);
                resultVolume.Set(0, 0, 0, n, 1.0f);
            }

            discriminatorTrainer.Train(sampleVolume, resultVolume);
        }

        public void TrainFake(int batchSize)
        {
            Shape latentShape = new Shape(1, 1, 64, batchSize);
            var latentVectors = BuilderInstance.Volume.Random(latentShape);

            var fakeImages = generatorModel.Forward(latentVectors);

            Shape resultShape = new Shape(1, 1, 1, batchSize);
            var resultVolume = BuilderInstance.Volume.SameAs(resultShape);
            for (int n = 0; n < batchSize; n++)
            {
                resultVolume.Set(0, 0, 0, n, 1.0f);
            }

            discriminatorTrainer.Train(fakeImages, resultVolume);
        }
    }
}