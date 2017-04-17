## Enum
```C#
string strVal = Enum.GetName(typeof(Season),  Season.Spring);
int intVal = (int)(Season.Autumn | Season.Winter);
Season enmVal = (Season)Enum.Parse(typeof(Season), "spring, summer", true);
Season enmVal = (Season)Enum.ToObject(typeof(Season), 2);
```
## 動的なEnum

```C#
using System.Reflection;
using System.Reflection.Emit;

static System.Type BuildEnum(string[] strings)
{
  AssemblyName asmName = new AssemblyName{ Name = "MyAssembly" };
  System.AppDomain domain = System.AppDomain.CurrentDomain;
  AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
  ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule("MyModule");
  EnumBuilder enumBuilder = moduleBuilder.DefineEnum("MyNamespace.MyEnum", TypeAttributes.Public, typeof(int));

  for (int i = 0; i < strings.Length; ++i)
    enumBuilder.DefineLiteral(strings[i], i + 1);
    return enumBuilder.CreateType();
}

public void Main()
{
  string[] animalTable = new string[] { "Dog", "Cat", "Horse", "Elephant" };
  System.Type enumType = BuildEnum(animalTable);

  System.Enum animal = (System.Enum)Activator.CreateInstance(enumType); //インスタンス生成
  animal = (System.Enum)enumType.GetField("Cat").GetValue(null);        //値の代入
  animal = (System.Enum)System.Enum.Parse(enumType, "Horse");           //文字列から値生成
  
  //intとの変換
  int val = (int)animal;
  animal = (Animal)val;
  int val = (int)animal;
  animal = (System.Enum)System.Convert.ChangeType(val, enumType);
}
```
