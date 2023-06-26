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
**实现内容：** 绿蓝各2个共4个cube一起旋转。体现对蓝和绿**分别处理**（即不同颜色的物体有不同的处理逻辑，因此没有用一个IJobEntity+withAny\<green/blueTag\>来处理）、对所有同色物体一起处理。

**实现方式：**
实现1：在独立component文件中定义RotateSpeed、GreeTag、BlueTag这三个component；写两个authoring文件分别bake上GreenTag+RotateSpeed和BlueTag+RotateSpeed，给蓝绿cube挂载上不同的authoring；在一个文件中写两个system（因为system不用挂载），里面用`foreach`+`SystemAPIQuery<RefRW<LocalTransform>, RefRO<RotateSpeed>>().WithAll<Green/BlueTag>()` 实现旋转逻辑。

**待改进点：**
1. 初始的绿蓝各2个共4个cube能否完全由代码生成
2. RotateSpeed和ColorTag最好在两个authoring文件中，分别bake，这样不会破坏开闭原则。*能否在一个物体上挂两个authoring？如果可以第二次bake的时候如何获取该entity？
**解答**：一个gameobject上可以挂多个monobehavior，因此也可以挂多个authoring

### lesson2: 
**实现内容：** 5个cube以不同的速度旋转。在工作线程中并行处理旋转逻辑，分别使用IJobEntity和IJobChunk来实现在工作线程中并行处理旋转逻辑。

**实现方式：**
**IJobEntity**：1.实现一个public partial struct且继承自IJobEntity的job，定义要用的变量(如deltaTime)，用public修饰，等待job实例化的时候传入，然后实现（不叫重写！）Execute方法，其参数就是要筛选检索的组件类型，用ref，in，out等修饰好（如果仅做筛选不做读写也得放在参数里！不像`SystemAPI.Query<RefRW<>,RefRO<>>().WithAll<>()`）。
2.然后在system的OnUpdate中new一个job对象，new的时候把job里public的参数都赋好值。
3.再`state.Dependency = job.ScheduleParallel(state.Dependency)`就可以了，在原本的依赖关系中插入该job。

**IJobChunk**：1.实现一个public partial struct且继承自IJobChunk的job，定义要用的变量+要检索的组件类型的类柄(typehandle)，用public修饰，等待job实例化的时候传入，实现Execute方法，注意参数是固定的。然后用`chunk.GetNativeArray(ref xxTypeHandle)`的方式获取chunk内的component数组，接着使用迭代器ChunkEntityEnumerator或自己用for循环实现对每个entity的操作。
2.在system内部设private的变量（system内不要定义public变量），用于OnCreate和OnUpdate的变量传递：EntityQuery和要检索的组件的类柄。
- 为什么要类柄：因为实例化job的时候需要传递类柄参数。
- 为什么要EntityQuery：因为job.ScheduleParallel的时候需要作为第一个参数（IJobChunk本质上根据EntityQuery检索chunk，检索到后再用IJobChunk来逐chunk操作其中的组件，因此既要EntityQuery，又要类柄）。
  
3.在system的OnCreate中通过`new EntityQueryBuilder(Allocator.Temp).WithAll<RotateSpeed, LocalTransform>()`的方式获取QueryBuilder，再`state.GetEntityQuery(queryBuilder)`得到EntityQuery。另外通过`state.GetComponentTypeHandle<xxx>()`来获取类柄。
4.在system的OnUpdate中，*首先要让类柄更新`TypeHandle.Update(ref state)`，防止structral change，（也可以不在OnCreate中保存变量，而是在OnUpdate中每帧获取，但这样开销更大，因为`TypeHandle.Update(ref state)`是增量更新）*。然后实例化job，把job要的参数和类柄传过去。然后通过`state.Dependency = job.ScheduleParallel(EntityQuery, state.Dependency)`开启job，注意要传入EntityQuery，还要更新Dependency。

**待改进点：**
1. 



### lesson7:
**实现内容：** 在左侧通过job一批一批地生成物体，物体会有随机终点，然后边旋转边移动到终点。在中间的区域物体不再旋转，而是上下弹跳+形状变大变小（弹跳+变性看能不能用aspect实现）

**实现方式：**
