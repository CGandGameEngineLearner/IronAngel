# LogicState文档
## 用途
用于角色的逻辑状态管理,支持维护多重状态、持续时间管理、Buff管理
## 架构图
```plantuml
@startuml
allowmixing


package 外部模块
package 逻辑状态管理器
{
    class LogicStateManager
    class LogicState
}

note top of 外部模块:通过调用LogicManager增删查改状态,根据LogicState广播的事件做出反应,可以Set和Get状态的持续时长
外部模块 ..> 逻辑状态管理器
外部模块 <.. 逻辑状态管理器
LogicStateManager --> LogicState:维护状态
@enduml
```
## 用法案例
[LogicState_Example](./LogicStateSubclass/LogicState_Example.cs)

## 原理讲解
LogicStateManager中使用了字典维护每个逻辑状态，键为状态枚举，值为状态实例。
所有的状态都继承自LogicState，其提供了以下主要函数，其调用顺序、时序关系请看[LogicState](./LogicState.cs)

最佳实践请阅读以下源码：

[LogicState_Example](./LogicStateSubclass/LogicState_Example.cs)

[LogicState_Damaged](./LogicStateSubclass/LogicState_Damaged.cs)
