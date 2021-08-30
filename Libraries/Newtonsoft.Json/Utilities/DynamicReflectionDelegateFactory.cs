// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.DynamicReflectionDelegateFactory
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Newtonsoft.Json.Utilities
{
  internal class DynamicReflectionDelegateFactory : ReflectionDelegateFactory
  {
    public static DynamicReflectionDelegateFactory Instance = new DynamicReflectionDelegateFactory();

    private static DynamicMethod CreateDynamicMethod(
      string name,
      Type returnType,
      Type[] parameterTypes,
      Type owner)
    {
      return !owner.IsInterface() ? new DynamicMethod(name, returnType, parameterTypes, owner, true) : new DynamicMethod(name, returnType, parameterTypes, owner.Module, true);
    }

    public override ObjectConstructor<object> CreateParametrizedConstructor(
      MethodBase method)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(method.ToString(), typeof (object), new Type[1]
      {
        typeof (object[])
      }, method.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateMethodCallIL(method, ilGenerator, 0);
      return (ObjectConstructor<object>) dynamicMethod.CreateDelegate(typeof (ObjectConstructor<object>));
    }

    public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(method.ToString(), typeof (object), new Type[2]
      {
        typeof (object),
        typeof (object[])
      }, method.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateMethodCallIL(method, ilGenerator, 1);
      return (MethodCall<T, object>) dynamicMethod.CreateDelegate(typeof (MethodCall<T, object>));
    }

    private void GenerateCreateMethodCallIL(
      MethodBase method,
      ILGenerator generator,
      int argsIndex)
    {
      ParameterInfo[] parameters = method.GetParameters();
      Label label1 = generator.DefineLabel();
      generator.Emit(OpCodes.Ldarg, argsIndex);
      generator.Emit(OpCodes.Ldlen);
      generator.Emit(OpCodes.Ldc_I4, parameters.Length);
      generator.Emit(OpCodes.Beq, label1);
      generator.Emit(OpCodes.Newobj, typeof (TargetParameterCountException).GetConstructor(ReflectionUtils.EmptyTypes));
      generator.Emit(OpCodes.Throw);
      generator.MarkLabel(label1);
      if (!method.IsConstructor && !method.IsStatic)
        generator.PushInstance(method.DeclaringType);
      int num = 0;
      for (int arrayIndex = 0; arrayIndex < parameters.Length; ++arrayIndex)
      {
        ParameterInfo parameterInfo = parameters[arrayIndex];
        Type parameterType = parameterInfo.ParameterType;
        if (parameterType.IsByRef)
        {
          Type elementType = parameterType.GetElementType();
          LocalBuilder local = generator.DeclareLocal(elementType);
          if (!parameterInfo.IsOut)
          {
            generator.PushArrayInstance(argsIndex, arrayIndex);
            if (elementType.IsValueType())
            {
              Label label2 = generator.DefineLabel();
              Label label3 = generator.DefineLabel();
              generator.Emit(OpCodes.Brtrue_S, label2);
              generator.Emit(OpCodes.Ldloca_S, local);
              generator.Emit(OpCodes.Initobj, elementType);
              generator.Emit(OpCodes.Br_S, label3);
              generator.MarkLabel(label2);
              generator.PushArrayInstance(argsIndex, arrayIndex);
              generator.UnboxIfNeeded(elementType);
              generator.Emit(OpCodes.Stloc, num);
              generator.MarkLabel(label3);
            }
            else
            {
              generator.UnboxIfNeeded(elementType);
              generator.Emit(OpCodes.Stloc, num);
            }
          }
          generator.Emit(OpCodes.Ldloca_S, local);
          ++num;
        }
        else if (parameterType.IsValueType())
        {
          generator.PushArrayInstance(argsIndex, arrayIndex);
          Label label4 = generator.DefineLabel();
          Label label5 = generator.DefineLabel();
          generator.Emit(OpCodes.Brtrue_S, label4);
          LocalBuilder local = generator.DeclareLocal(parameterType);
          generator.Emit(OpCodes.Ldloca_S, local);
          generator.Emit(OpCodes.Initobj, parameterType);
          generator.Emit(OpCodes.Ldloc, num);
          generator.Emit(OpCodes.Br_S, label5);
          generator.MarkLabel(label4);
          generator.PushArrayInstance(argsIndex, arrayIndex);
          generator.UnboxIfNeeded(parameterType);
          generator.MarkLabel(label5);
          ++num;
        }
        else
        {
          generator.PushArrayInstance(argsIndex, arrayIndex);
          generator.UnboxIfNeeded(parameterType);
        }
      }
      if (method.IsConstructor)
        generator.Emit(OpCodes.Newobj, (ConstructorInfo) method);
      else
        generator.CallMethod((MethodInfo) method);
      Type type = method.IsConstructor ? method.DeclaringType : ((MethodInfo) method).ReturnType;
      if (type != typeof (void))
        generator.BoxIfNeeded(type);
      else
        generator.Emit(OpCodes.Ldnull);
      generator.Return();
    }

    public override Func<T> CreateDefaultConstructor<T>(Type type)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Create" + type.FullName, typeof (T), ReflectionUtils.EmptyTypes, type);
      dynamicMethod.InitLocals = true;
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateDefaultConstructorIL(type, ilGenerator);
      return (Func<T>) dynamicMethod.CreateDelegate(typeof (Func<T>));
    }

    private void GenerateCreateDefaultConstructorIL(Type type, ILGenerator generator)
    {
      if (type.IsValueType())
      {
        generator.DeclareLocal(type);
        generator.Emit(OpCodes.Ldloc_0);
        generator.Emit(OpCodes.Box, type);
      }
      else
      {
        ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, ReflectionUtils.EmptyTypes, (ParameterModifier[]) null);
        if (constructor == (ConstructorInfo) null)
          throw new ArgumentException("Could not get constructor for {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
        generator.Emit(OpCodes.Newobj, constructor);
      }
      generator.Return();
    }

    public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Get" + propertyInfo.Name, typeof (T), new Type[1]
      {
        typeof (object)
      }, propertyInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateGetPropertyIL(propertyInfo, ilGenerator);
      return (Func<T, object>) dynamicMethod.CreateDelegate(typeof (Func<T, object>));
    }

    private void GenerateCreateGetPropertyIL(PropertyInfo propertyInfo, ILGenerator generator)
    {
      MethodInfo getMethod = propertyInfo.GetGetMethod(true);
      if (getMethod == (MethodInfo) null)
        throw new ArgumentException("Property '{0}' does not have a getter.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyInfo.Name));
      if (!getMethod.IsStatic)
        generator.PushInstance(propertyInfo.DeclaringType);
      generator.CallMethod(getMethod);
      generator.BoxIfNeeded(propertyInfo.PropertyType);
      generator.Return();
    }

    public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
    {
      if (fieldInfo.IsLiteral)
      {
        object constantValue = fieldInfo.GetValue((object) null);
        return (Func<T, object>) (o => constantValue);
      }
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Get" + fieldInfo.Name, typeof (T), new Type[1]
      {
        typeof (object)
      }, fieldInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      this.GenerateCreateGetFieldIL(fieldInfo, ilGenerator);
      return (Func<T, object>) dynamicMethod.CreateDelegate(typeof (Func<T, object>));
    }

    private void GenerateCreateGetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
    {
      if (!fieldInfo.IsStatic)
      {
        generator.PushInstance(fieldInfo.DeclaringType);
        generator.Emit(OpCodes.Ldfld, fieldInfo);
      }
      else
        generator.Emit(OpCodes.Ldsfld, fieldInfo);
      generator.BoxIfNeeded(fieldInfo.FieldType);
      generator.Return();
    }

    public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Set" + fieldInfo.Name, (Type) null, new Type[2]
      {
        typeof (T),
        typeof (object)
      }, fieldInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      DynamicReflectionDelegateFactory.GenerateCreateSetFieldIL(fieldInfo, ilGenerator);
      return (Action<T, object>) dynamicMethod.CreateDelegate(typeof (Action<T, object>));
    }

    internal static void GenerateCreateSetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
    {
      if (!fieldInfo.IsStatic)
        generator.PushInstance(fieldInfo.DeclaringType);
      generator.Emit(OpCodes.Ldarg_1);
      generator.UnboxIfNeeded(fieldInfo.FieldType);
      if (!fieldInfo.IsStatic)
        generator.Emit(OpCodes.Stfld, fieldInfo);
      else
        generator.Emit(OpCodes.Stsfld, fieldInfo);
      generator.Return();
    }

    public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
    {
      DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod("Set" + propertyInfo.Name, (Type) null, new Type[2]
      {
        typeof (T),
        typeof (object)
      }, propertyInfo.DeclaringType);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      DynamicReflectionDelegateFactory.GenerateCreateSetPropertyIL(propertyInfo, ilGenerator);
      return (Action<T, object>) dynamicMethod.CreateDelegate(typeof (Action<T, object>));
    }

    internal static void GenerateCreateSetPropertyIL(
      PropertyInfo propertyInfo,
      ILGenerator generator)
    {
      MethodInfo setMethod = propertyInfo.GetSetMethod(true);
      if (!setMethod.IsStatic)
        generator.PushInstance(propertyInfo.DeclaringType);
      generator.Emit(OpCodes.Ldarg_1);
      generator.UnboxIfNeeded(propertyInfo.PropertyType);
      generator.CallMethod(setMethod);
      generator.Return();
    }
  }
}
