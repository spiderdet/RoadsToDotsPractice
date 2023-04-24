using DOTS.DOD;
using Unity.Entities;

namespace DOTS.DOD
{
    public partial class CubeRotateSystemGroup : MatchSceneSystemGroup //父类在EntitiesPractice/common/Scripts/SystemGroupsManager中
                                                              //定义，主要实现了group内SceneName与当前scene的name不同就disable的功能
    {
        protected override string RequiredSceneName => "RotateCubeAuthoring";  //用表达式体来初始化该值，该值为只读类型，初始化后不能改
    }    

    public partial class TestSystemGroup : MatchSceneSystemGroup
    {
        protected override string RequiredSceneName => "Test";
    }
}

   

