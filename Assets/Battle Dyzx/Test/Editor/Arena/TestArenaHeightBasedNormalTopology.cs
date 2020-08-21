using NUnit.Framework;

namespace BattleDyzx.Test
{
    public class TestGradientArenaHeightTopology
    {
        [Test]
        public void SampleElevation_WithBaseElevationZeroGradientAndScaledOutput_ShouldReturnBaseElevation()
        {
            // Given 
            var topology = new GradientArenaReliefTopology( 128, 128, 128, 0, 0, 30 );

            // When
            float sample1 = topology.SampleElevation( 10, 15, ArenaCoordType.Scaled );
            float sample2 = topology.SampleElevation( 1.0f, 0.8f, ArenaCoordType.ScaledOutput );
            float sample3 = topology.SampleElevation( 0, 128, ArenaCoordType.Scaled );

            // Then            
            Assert.AreEqual( 30.0f, sample1 );
            Assert.AreEqual( 30.0f, sample2 );
            Assert.AreEqual( 30.0f, sample3 );
        }

        [Test]
        public void SampleElevation_WithBaseElevationZeroGradientAndNormalizedOutput_ShouldReturnBaseElevationOverDepth()
        {
            // Given 
            var topology = new GradientArenaReliefTopology( 128, 128, 128, 0, 0, 32 );

            // When
            float sample1 = topology.SampleElevation( 10, 15, ArenaCoordType.ScaledInput );
            float sample2 = topology.SampleElevation( 1.0f, 0.8f, ArenaCoordType.Normalized );
            float sample3 = topology.SampleElevation( 0, 1.0f, ArenaCoordType.Normalized );

            // Then            
            Assert.AreEqual( 0.25f, sample1 );
            Assert.AreEqual( 0.25f, sample2 );
            Assert.AreEqual( 0.25f, sample3 );
        }

        [Test]
        public void Constructor_WithZeroWidth_ShouldThrowException()
        {
            // Given
            TestDelegate construction = () => new GradientArenaReliefTopology( 0, 128, 128, 0, 0, 0 );

            // When
            var exception = Assert.Throws<IllegalDimensionsException>( construction );

            // Then
            Assert.NotNull( exception );
        }

        [Test]
        public void Constructor_WithZeroHeight_ShouldThrowException()
        {
            // Given
            TestDelegate construction = () => new GradientArenaReliefTopology( 128, 0, 128, 0, 0, 0 );

            // When
            var exception = Assert.Throws<IllegalDimensionsException>( construction );

            // Then
            Assert.NotNull( exception );
        }

        [Test]
        public void Constructor_WithZeroDepth_ShouldThrowException()
        {
            // Given
            TestDelegate construction = () => new GradientArenaReliefTopology( 128, 128, 0, 0, 0, 0 );

            // When
            var exception = Assert.Throws<IllegalDimensionsException>( construction );

            // Then
            Assert.NotNull( exception );
        }

        [Test]
        public void Width_WhenSetToZero_ShouldThrowException()
        {
            // Given
            var topology = new GradientArenaReliefTopology( 128, 128, 128, 0, 0, 0 );
            TestDelegate setOperation = () => { topology.width = 0.0f; };

            // When
            var exception = Assert.Throws<IllegalDimensionsException>( setOperation );

            // Then
            Assert.NotNull( exception );
        }

        [Test]
        public void Height_WhenSetToZero_ShouldThrowException()
        {
            // Given
            var topology = new GradientArenaReliefTopology( 128, 128, 128, 0, 0, 0 );
            TestDelegate setOperation = () => { topology.height = 0.0f; };

            // When
            var exception = Assert.Throws<IllegalDimensionsException>( setOperation );

            // Then
            Assert.NotNull( exception );
        }
        
        [Test]
        public void Depth_WhenSetToZero_ShouldThrowException()
        {
            // Given
            var topology = new GradientArenaReliefTopology( 128, 128, 128, 0, 0, 0 );
            TestDelegate setOperation = () => { topology.depth = 0.0f; };

            // When
            var exception = Assert.Throws<IllegalDimensionsException>( setOperation );

            // Then
            Assert.NotNull( exception );
        }
    }
}