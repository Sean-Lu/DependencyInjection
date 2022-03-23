## ���

> ����ע�루`Dependency Injection`��������ʵ��`IOC`��`Inversion of Control`�����Ʒ�ת��������ķ�ʽ֮һ�����Դﵽ�����Ŀ�ġ�
> 
> ������`IOC`��ܣ�

| IOC���                                                                | ˵��                |
| -------------------------------------------------------------------- | ----------------- |
| `ServiceCollection`                                                  | .NET Core���õ�IOC���� |
| [`Autofac`](https://github.com/autofac/Autofac)                      |                   |
| [`SimpleInjector`](https://github.com/simpleinjector/SimpleInjector) |                   |
| **Sean.Core.DependencyInjection**                                    | ��ǰ��Ŀ              |

## Packages

| Package                                                                                        | NuGet Stable                                                                                                                                                        | NuGet Pre-release                                                                                                                                                      | Downloads                                                                                                                                                            |
| ---------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Sean.Core.DependencyInjection](https://www.nuget.org/packages/Sean.Core.DependencyInjection/) | [![Sean.Core.DependencyInjection](https://img.shields.io/nuget/v/Sean.Core.DependencyInjection.svg)](https://www.nuget.org/packages/Sean.Core.DependencyInjection/) | [![Sean.Core.DependencyInjection](https://img.shields.io/nuget/vpre/Sean.Core.DependencyInjection.svg)](https://www.nuget.org/packages/Sean.Core.DependencyInjection/) | [![Sean.Core.DependencyInjection](https://img.shields.io/nuget/dt/Sean.Core.DependencyInjection.svg)](https://www.nuget.org/packages/Sean.Core.DependencyInjection/) |

## Nuget������

> **Id��`Sean.Core.DependencyInjection`**

- Package Manager

```
PM> Install-Package Sean.Core.DependencyInjection
```

## ˵��

> ʵ��ԭ������ + �ݹ飨���������� + ����

- ����Ŀ֧������ע��ķ�ʽ�����캯��
- ���`ImplementationInstance`��`ImplementationFactory`���Ծ�Ϊnull����ô��ͨ��`ImplementationType`����ѡ��һ���ʺϵĹ��캯�����������յķ���ʵ����

```
����֪��class����Զ����˶�����캯������ô��Թ��캯����ѡ�����������Ĳ����أ�

�������캯����ֱ�ӽ���

������캯����������ʱ��Ĭ������ѡ��˳��Ϊ��
1. ����[DependencyInjectionAttribute]�Ĺ��캯����
2. �޲ι��캯����
3. �����������������ȫ���вι��캯����ֱ���ҵ������ṩ���в����Ĺ��캯�������Ƚ��������������Ĺ��캯������
```

- ����ע�����ʱ��������޷����������ͣ����׳��쳣 `UnresolvedTypeException`
- ����ע�����ʱ���������ѭ�����������׳��쳣 `CircularDependencyException`

```mermaid
graph TB

A[AService]-->B[BService]
B-->C[CService]
C-->A
```

```
�ʣ�Ϊʲô��Ҫ�׳��쳣��û�н��������
����Ϊ������׳��쳣���ᵼ����������������ѭ����ֱ���ڴ�ľ���ϵͳ���������Ƿǳ������ġ�

ע�⣺����ѭ������������ϵ����⣬һ��Ҫ���⣡
```

## ʹ��ʾ��

> ��Ŀ��examples\Example.NetCore
