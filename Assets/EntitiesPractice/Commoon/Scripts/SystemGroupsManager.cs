using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

namespace DOTS.DOD 
{ 
    public abstract partial class MatchSceneSystemGroup : ComponentSystemGroup //因为继承自SystemBase所以是托管类，用class表示
    {

        protected abstract string RequiredSceneName { get; } //抽象只读属性（没有set就是只读），子类必须实现该属性并且初始化，后续只读不可更改
        private bool initialized;

        protected override void OnCreate()
        {
            base.OnCreate();
            initialized = false;//该变量的目的是只查看一次，scene加载成功后那一次看scene name，如果对不上就永远不Update，中途scene也不管
        }
        protected override void OnUpdate()
        {
            if (!initialized)
            {
                // Debug.Log("this system group require scene name : " + RequiredSceneName);
                if (SceneManager.GetActiveScene().isLoaded)
                {
                    var subscene = Object.FindObjectOfType<SubScene>();
                    //Debug.Log("subscene == null : " + (subscene == null));
                    if (subscene != null) 
                    {
                        //Enabled属性是在父类ComponentSystemGroup中定义的，代表本systen的enable状态
                        Enabled = RequiredSceneName == subscene.gameObject.scene.name;
                        // Debug.Log("subSceneName : " + subscene.gameObject.scene.name);
                    }
                    else { Enabled = false; }//如果没有subscene直接disable，因为这说明没有entity？为啥不考虑还有convertToEntity的gameobject
                    initialized = true;//在isload的if里设置true，保证一直等待到scene loaded再进行检测
                }
            }
            base.OnUpdate();
        }
    }
}
