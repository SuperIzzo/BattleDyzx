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
        private TrainerBase<float> trainer;

        public void Build()
        {
            BuildDiscriminator();
        }

        private void BuildDiscriminator()
        {
            discriminatorModel = new Net<float>();

            discriminatorModel.AddLayer(new InputLayer(256, 256, 4));
            discriminatorModel.AddLayer(new ConvLayer(5, 5, 32) { Stride = 1, Pad = 2 });
            discriminatorModel.AddLayer(new LeakyReluLayer(0.2f));
            discriminatorModel.AddLayer(new PoolLayer(2, 2) { Stride = 2 });
            discriminatorModel.AddLayer(new ConvLayer(5, 5, 32) { Stride = 2, Pad = 2 });
            discriminatorModel.AddLayer(new LeakyReluLayer(0.2f));
            discriminatorModel.AddLayer(new PoolLayer(3, 3) { Stride = 2 });
            discriminatorModel.AddLayer(new FullyConnLayer(64));
            discriminatorModel.AddLayer(new LeakyReluLayer(0.2f));
            discriminatorModel.AddLayer(new FullyConnLayer(1));
            discriminatorModel.AddLayer(new SigmoidLayer());
            discriminatorModel.AddLayer(new RegressionLayer());

            var adamTrainer = new AdamTrainer(discriminatorModel);

            trainer = adamTrainer;
        }

        private void BuildGenerator()
        {
            generatorModel = new Net<float>();

            generatorModel.AddLayer(new InputLayer(1, 1, 64));
            generatorModel.AddLayer(new FullyConnLayer(4*4*4));
            generatorModel.AddLayer(new ReshapeLayer(4,4,4));

            //discriminatorModel.AddLayer(generatorModel);
        }

        public void TrainReal(IImageData[] images)
        {
            int batchSize = images.Length;

            Shape sampleShape = new Shape(256, 256, 4, batchSize);
            Shape resultShape = new Shape(1, 1, 1, batchSize);

            
            var sampleVolume = BuilderInstance.Volume.SameAs(sampleShape);
            var resultVolume = BuilderInstance.Volume.SameAs(resultShape);

            for (int n=0; n< batchSize; n++)
            {
                sampleVolume.SetImageData(n, images[n]);
                resultVolume.Set(0, 0, 0, n, 1.0f);
            }

            trainer.Train(sampleVolume, resultVolume);
        }
    }
}