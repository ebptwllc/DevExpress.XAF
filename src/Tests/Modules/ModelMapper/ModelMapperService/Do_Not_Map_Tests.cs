﻿using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Fasterflect;
using Shouldly;
using Xpand.Source.Extensions.System.String;
using Xpand.XAF.Modules.ModelMapper;
using Xunit;

namespace Tests.Modules.ModelMapper.ModelMapperService{
    
    public partial class ModelMapperServiceTests{
        [Fact]
        public async Task Do_Not_Map_Already_Mapped_Properties(){
            InitializeMapperService(nameof(Do_Not_Map_Already_Mapped_Properties));
            var typeToMap = typeof(SelfReferenceTypeProperties);

            var modelType = await typeToMap.MapToModel().ModelInterfaces();

            var modelTypeProperties = modelType.Properties();

            modelTypeProperties.FirstOrDefault(info => info.Name==nameof(SelfReferenceTypeProperties.Self)).ShouldBeNull();
            var nestedType = modelType.Assembly.GetType($"IModel{typeof(NestedSelfReferenceTypeProperties).FullName.CleanCodeName()}");
            nestedType.ShouldNotBeNull();
            nestedType.Properties().Count.ShouldBe(0);
        }

        [Fact(Skip = NotImplemented)]
        public async Task Do_not_map_Objects_with_no_mapable_properties(){
            InitializeMapperService(nameof(Do_not_map_Objects_with_no_mapable_properties));
//            throw new NotImplementedException();
        }


        [Fact]
        public async Task Do_Not_Map_Reserved_properties(){
            InitializeMapperService(nameof(Do_Not_Map_Reserved_properties));
            var typeToMap = typeof(ResevredProperties);

            var modelType = await typeToMap.MapToModel().ModelInterfaces();

            var modelTypeProperties = ModelTypeProperties(modelType);
            
            modelTypeProperties.Length.ShouldBe(0);

        }

        [Fact]
        public async Task Do_Not_Map_Already_Mapped_Types(){
            var typeToMap1 = typeof(TestModelMapper);
            var typeToMap2 = typeof(TestModelMapper);
            InitializeMapperService(nameof(Do_Not_Map_Already_Mapped_Types));

            await typeToMap1.MapToModel().ModelInterfaces();
            await typeToMap2.MapToModel().ModelInterfaces();
            
            Xpand.XAF.Modules.ModelMapper.ModelMapperService.MappedTypes.ToEnumerable().Count().ShouldBe(1);
            
        }

        [Fact]
        public async Task Do_Not_Map_If_Type_Assembly_Version_Not_Changed(){
            InitializeMapperService(nameof(Do_Not_Map_If_Type_Assembly_Version_Not_Changed));
            var mappedType = typeof(TestModelMapper);

            var mapToModel = await mappedType.MapToModel().ModelInterfaces();

            var modelMapperAttribute = mapToModel.Assembly.GetCustomAttributes(typeof(ModelMapperServiceAttribute),false)
                .OfType<ModelMapperServiceAttribute>().FirstOrDefault(attribute => attribute.MappedType==mappedType.FullName&&attribute.MappedAssemmbly==mappedType.Assembly.GetName().Name);
            modelMapperAttribute.ShouldNotBeNull();

            var version = modelMapperAttribute.Version;

            mappedType.MapToModel();
            mapToModel = await Xpand.XAF.Modules.ModelMapper.ModelMapperService.MappedTypes;

            modelMapperAttribute = mapToModel.Assembly.GetCustomAttributes(typeof(ModelMapperServiceAttribute),false)
                .OfType<ModelMapperServiceAttribute>().First(attribute => attribute.MappedType==mappedType.FullName&&attribute.MappedAssemmbly==mappedType.Assembly.GetName().Name);
            modelMapperAttribute.Version.ShouldBe(version);
        }

    }
}
