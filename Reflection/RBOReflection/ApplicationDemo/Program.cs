using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using RBO.Util;

namespace ApplicationDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1000000;
            string str = Test_SetProperty(count);
            ShowResult(str);
        }

        static string Test_SetProperty(int count)
        {
            StringBuilder sb = new StringBuilder(System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());
            sb.AppendLine("Test_SetProperty");
            OrderInfo testObj = new OrderInfo();
            PropertyInfo propInfo = typeof(OrderInfo).GetProperty("OrderID");
            sb.AppendLine("直接访问花费时间：       ");
            Stopwatch watch1 = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
                testObj.OrderID = 123;

            watch1.Stop();
            sb.Append(watch1.Elapsed.ToString());


            SetValueDelegate setter2 = DynamicMethodFactory.CreatePropertySetter(propInfo);
            sb.AppendLine("EmitSet花费时间：        ");
            Stopwatch watch2 = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                setter2(testObj, 123);
            }

            watch2.Stop();
            sb.Append(watch2.Elapsed.ToString());


            sb.AppendLine("纯反射花费时间：　       ");
            Stopwatch watch3 = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
                propInfo.SetValue(testObj, 123, null);

            watch3.Stop();
            sb.Append(watch3.Elapsed.ToString());



            //sb.AppendLine("泛型委托花费时间：       ");
            //SetterWrapper<OrderInfo, int> setter3 = new SetterWrapper<OrderInfo, int>(propInfo);
            //Stopwatch watch4 = Stopwatch.StartNew();

            //for (int i = 0; i < count; i++)
            //    setter3.SetValue(testObj, 123);

            //watch4.Stop();
            //Console.WriteLine(watch4.Elapsed.ToString());


            //Console.Write("通用接口花费时间：       ");
            //ISetValue setter4 = GetterSetterFactory.CreatePropertySetterWrapper(propInfo);
            //Stopwatch watch5 = Stopwatch.StartNew();

            //for (int i = 0; i < count; i++)
            //    setter4.Set(testObj, 123);

            //watch5.Stop();
            //Console.WriteLine(watch5.Elapsed.ToString());



            //propInfo.FastSetValue(testObj, 123);
            //Console.Write("FastSet花费时间：　      ");
            //Stopwatch watch6 = Stopwatch.StartNew();

            //for (int i = 0; i < count; i++)
            //    propInfo.FastSetValue(testObj, 123);

            //watch6.Stop();
            //Console.WriteLine(watch6.Elapsed.ToString());



            //propInfo.FastSetValue2(testObj, 123);
            //Console.Write("FastSet2花费时间：　     ");
            //Stopwatch watch6b = Stopwatch.StartNew();

            //for (int i = 0; i < count; i++)
            //    propInfo.FastSetValue2(testObj, 123);

            //watch6b.Stop();
            //Console.WriteLine(watch6b.Elapsed.ToString());



            //Hashtable table = new Hashtable();
            //table[propInfo] = new object();
            //Console.Write("Hashtable花费时间：      ");
            //Stopwatch watch7 = Stopwatch.StartNew();

            //for (int i = 0; i < count; i++)
            //{
            //    object val = table[propInfo];
            //}
            //watch7.Stop();
            //Console.WriteLine(watch7.Elapsed.ToString());



            sb.AppendLine("-------------------");
            sb.AppendLine("纯反射/直接赋值：     ");
            sb.AppendFormat("{0} / {1} = {2}",
                watch3.Elapsed.ToString(),
                watch1.Elapsed.ToString(),
                watch3.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);

            sb.AppendLine("纯反射/EmitSet：     ");
            sb.AppendFormat("{0} / {1} = {2}",
                watch3.Elapsed.ToString(),
                watch2.Elapsed.ToString(),
                watch3.Elapsed.TotalMilliseconds / watch2.Elapsed.TotalMilliseconds);
            sb.AppendLine("EmitSet/直接赋值：     ");
            sb.AppendFormat("{0} / {1} = {2}",
                watch2.Elapsed.ToString(),
                watch1.Elapsed.ToString(),
                watch2.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);

            //Console.WriteLine("{0} / {1} = {2}",
            //    watch3.Elapsed.ToString(),
            //    watch5.Elapsed.ToString(),
            //    watch3.Elapsed.TotalMilliseconds / watch5.Elapsed.TotalMilliseconds);

            //Console.WriteLine("{0} / {1} = {2}",
            //    watch3.Elapsed.ToString(),
            //    watch6.Elapsed.ToString(),
            //    watch3.Elapsed.TotalMilliseconds / watch6.Elapsed.TotalMilliseconds);
            return sb.ToString();
        }


        static void ShowResult(string str)
        {
            Console.WriteLine(str);
            Console.ReadLine();
        }
    }
}
