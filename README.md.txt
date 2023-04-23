# 各个scene对应内容

## JobTutorials

1. lesson0： 80*80个cube以正弦函数波动，用monobehavior挂载，仅将全体transform更新部分交给job并行处理。
1. lesson1： 在左侧随机起点每一批生成一定数量cube，cube有随机终点，会自动旋转和平移到终点销毁。旋转和平移部分交给job。还采用对象池，job对应的对象池得全部初始化，内存占用较非job实现方式高？

## EntitiesTutorials

1. lesson0: 单个cube旋转。采用authoring、bake方式将cube转换为entity（同时引入LocalTransform组件），并设计system根据RotateSpeed这一component使(所有)cube旋转。
1. 