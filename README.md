# 铁骑黑天使
## 快速启动项目
### 安装依赖
- Unity 2022.3.2.9740
- Github Desktop
- Visual Studio Code 并安装插件以下插件:
```
C#
C# Dev Kit
IntelliCode for C# Dev Kit
C# XML Documentation Comments
(安装完以上C#相关插件后，使用插件自带的功能安装.Net8和.Net Framework4.7.1，不要跑网上乱下)
Unity
Markdown Preview Enhanced
PlantUML
PDF Preview
Bookmarks
```
### 启动！
双击`open_unity.bat`


### 支持多重逻辑状态管理的LogicStateManager使用说明
假设你需要新增一个状态，名为PlayerMoving,当玩家正在位移时就会添加这个状态，玩家禁止时移除这个状态。
那么你需要找程序员在ELogicState中添加这个枚举，并且在LogicStatesSettings的列表LogicStateTemplates中添加这个状态的生成模板对象。
然后策划在Assets/Config目录中找到对应的配置，并且设置它。
![状态设置](./illustraction/状态设置.png)
- 其中，状态只会在满足LogicStateManager中有included列表中定义的状态，且没有exincluded列表中的状态时，才能存在，如果运行时不满足这一容斥条件了，这个状态就会被自动移除。
- 每个状态默认持续时间为无限时长，由程序员在逻辑中移除这个状态，如果状态存在的时间超过了这个设置，也会被自动移除。设置Duration时，输入inf并回车，会被自动补全为infinity，无限持续时长。
