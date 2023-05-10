# 各个scene对应内容
## JobTutorials/Practice
### lesson0： 
**实现内容：** 80\*80个cube以正弦函数波动。
**实现方式：**
用monobehavior挂载，仅将全体transform更新部分交给job并行处理。

### lesson1： 
**实现内容：** 在左侧随机起点每一批生成一定数量cube，cube有随机终点，会自动旋转和平移到终点销毁。
**实现方式：**
旋转和平移部分交给job。还采用对象池，job对应的对象池得全部初始化，内存占用较非job实现方式高？

## EntitiesTutorials/Practice

### lesson0: 
**实现内容：** 一.单个cube旋转 二.根据scene name是否匹配来选择是否enable相应的system group
**实现方式：**
一.采用authoring、bake方式将cube转换为entity（同时引入LocalTransform组件），并设计system根据RotateSpeed这一component使(所有)cube旋转，这里是在主线程中查询并旋转的（用`foreach(var transform in SystemAPI.Query<RefRW<LocalTransform>>()`)这样查询），没有分发给job。

二.另外还实现了MatchSceneSystemGroup的抽象类（继承自ComponentSystemGroup（它继承自SystemBase），其中有Enabled属性），使得继承它的systemGroup以及在这些systemGroup下update的system都能根据sceneName来Enable或Disable。注意继承它的只是systemGroup，system只需要加上`[UpdateInGroup(typeof(xx))]`且继承Isystem或SystemBase，不继承MatchSceneSystemGroup！
### lesson1: 
**实现内容：** 绿蓝各2个共4个cube一起旋转。体现对蓝和绿分别处理、对所有同色物体一起处理。
**实现方式：**
实现1：在独立component文件中定义RotateSpeed、GreeTag、BlueTag这三个component；写两个authoring文件分别bake上GreenTag+RotateSpeed和BlueTag+RotateSpeed，给蓝绿cube挂载上不同的authoring；在一个文件中写两个system（因为system不用挂载），里面用`foreach`+`SystemAPIQuery<RefRW<LocalTransform>, RefRO<RotateSpeed>>().WithAll<Green/BlueTag>()` 实现旋转逻辑。

**待改进点：**
1. 初始的绿蓝各2个共4个cube能否完全由代码生成
2. RotateSpeed和ColorTag最好在两个authoring文件中，分别bake，这样不会破坏开闭原则。*能否在一个物体上挂两个authoring？如果可以第二次bake的时候如何获取该entity？*

### lesson2: 
**实现内容：** 5个cube以不同的速度旋转。在工作线程中并行处理旋转逻辑，使用IJobEntity和IJobChunk。
**实现方式：**
IJobEntity：

IJobChunk:
1. 

**待改进点：**