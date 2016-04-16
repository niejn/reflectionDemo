using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace RBO.Util
{
    class Wrapper
    {
    }


    // zhushi2
    public interface ISetValue
    {
        void Set(object target, object val);
    }

    public class SetterWrapper<TTarget, TValue>
    {
        private Action<TTarget, TValue> _setter;

        public SetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo m = propertyInfo.GetSetMethod(true);
            _setter = (Action<TTarget, TValue>)Delegate.CreateDelegate(typeof(Action<TTarget, TValue>), null, m);
        }

        public void SetValue(TTarget target, TValue val)
        {
            _setter(target, val);
        }
    }
}
