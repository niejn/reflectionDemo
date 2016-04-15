using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace RBO.Util
{
    public class DynamicMethodEmit
    {
    }

    public delegate void SetValueDelegate(object target, object arg);

    public delegate object GetValueDelegate(object target);

    public static class DynamicMethodFactory
    {
        public static SetValueDelegate CreatePropertySetter(PropertyInfo property)
        {            
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanWrite)
                return null;

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                new Type[] { typeof(object), typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, setMethod, null);

            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }


        public static SetValueDelegate CreateFieldSetter(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            DynamicMethod dm = new DynamicMethod("FieldSetter", null,
                new Type[] { typeof(object), typeof(object) },
                field.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!field.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, field.FieldType);

            if (!field.IsStatic)
                il.Emit(OpCodes.Stfld, field);
            else
                il.Emit(OpCodes.Stsfld, field);
            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));

        }

        public static GetValueDelegate CreatePropertyGetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanRead)
                return null;

            MethodInfo getMethod = property.GetGetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(object),
                new Type[] { typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!getMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, getMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, getMethod, null);

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);

            il.Emit(OpCodes.Ret);

            return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);
            else
                il.Emit(OpCodes.Castclass, type);
        }
    }
}
