using AutoMapper.MultiTargetMapping.Tests.Mapper;
using AutoMapper.MultiTargetMapping.Tests.Models;
using System;
using System.Linq;
using Xunit;

namespace AutoMapper.MultiTargetMapping.Tests
{

    public class MultiTargetMapperTests
    {

        public AModel Source { get; }

        public MultiTargetMapperTests()
        {
            AutoMapper.Mapper.Initialize(config => config.AddProfile<MyProfile>());

            this.Source = new AModel()
            {
                Field1 = "a",
                Field2 = "b"
            };
        }

        private void AssertTransform<From, To>(From source, To actual)
        {
            var map = AutoMapper.Mapper.Map<From, To>(source);
            Assert.Equal(map.ToString(), actual.ToString());
        }

        [Fact]
        public void TupleMultiTargetMappingTest()
        {
            // Map to any type of Tuple, and then auto destructs.
            var (b1, b2) = MultiTargetMapper.Map<Tuple<BModel, BModel>>(this.Source);

            Assert.IsType<BModel>(b1);
            Assert.IsType<BModel>(b2);

            AssertTransform(this.Source, b1);
            AssertTransform(this.Source, b2);

            // Any number no more than 8 of items is ok.
            var (b3, b4, c1) = MultiTargetMapper.Map<Tuple<BModel, BModel, CModel>>(this.Source);

            Assert.IsType<BModel>(b3);
            Assert.IsType<BModel>(b4);
            Assert.IsType<CModel>(c1);

            AssertTransform(this.Source, b3);
            AssertTransform(this.Source, b4);
            AssertTransform(this.Source, c1);
        }

        // Someone may ask why we cannot use Mapper.Map.
        // It is just beacuse AutoMapper's mapping registration is static since compilation time.
        // So we should write our own dynamic mapping function to do so.
        // Another related problem is, what if we use a dynamically created object to do the destruction?
        // The question is partially answered in the examples below.
        // We will provide a way to make a dynamic object which contains what you want.
        // Sadly, since the dynamically created object's type is only kwnown at runtime,
        // and the destruction assignment syntax just works in compilation time,
        // we cannot use a strongly-typed way to do so.

        [Fact]
        public void DynamicMultiTargetMappingTest()
        {
            // If you want to map as many as possible destinations, do like this.
            // This way you can get more than 8 items while Tuple just holds no more than 8 items.
            var destination = MultiTargetMapper.MapDynamic(this.Source,
                typeof(BModel), typeof(BModel), typeof(CModel), typeof(CModel));

            Assert.IsType<BModel>(destination.Item1);
            Assert.IsType<BModel>(destination.Item2);
            Assert.IsType<CModel>(destination.Item3);
            Assert.IsType<CModel>(destination.Item4);

            AssertTransform<AModel, BModel>(this.Source, destination.Item1);
            AssertTransform<AModel, BModel>(this.Source, destination.Item2);
            AssertTransform<AModel, CModel>(this.Source, destination.Item3);
            AssertTransform<AModel, CModel>(this.Source, destination.Item4);
        }

        [Fact]
        public void GeneralMultiTargetMappingTest()
        {
            // A more effetive way is, why not just use Linq to get a destination collection?
            var targetTypes = new[] { typeof(BModel), typeof(BModel), typeof(CModel), typeof(CModel) };
            var destinations = MultiTargetMapper.Map(this.Source, targetTypes);

            Assert.NotEmpty(destinations);
            Assert.Equal(targetTypes.Length, destinations.Count());

            for (var i = 0; i < targetTypes.Length; i++)
            {
                Assert.IsType(targetTypes[i], destinations[i]);
                var expected = AutoMapper.Mapper.Map(this.Source, this.Source.GetType(), targetTypes[i]);
                Assert.Equal(expected.ToString(), destinations[i].ToString());
            }
        }

    }

}