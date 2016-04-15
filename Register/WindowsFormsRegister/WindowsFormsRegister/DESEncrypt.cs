using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Management;

namespace WindowsFormsRegister
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public class DESEncrypt
    {

        #region ========加密========

        /// <summary>
        /// 加密xiugai
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "Rezin");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = Encoding.UTF8.GetBytes(sKey.Substring(0, 8));
            des.IV = Encoding.UTF8.GetBytes(sKey.Substring(0, 8)); 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "Rezin");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.UTF8.GetBytes(sKey.Substring(0, 8));
            des.IV = Encoding.UTF8.GetBytes(sKey.Substring(0, 8)); 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

    }

    class TimeClass
    {
        public static int InitRegedit()
        {
            /*检查注册表*/
            string SericalNumber = ReadSetting("", "SerialNumber", "-1");    // 读取注册表， 检查是否注册 -1为未注册
            if (SericalNumber == "-1")
            {               
                return 1;
            }

            /* 比较CPUid */
            string CpuId = GetSoftEndDateAllCpuId(1, SericalNumber);   //从注册表读取CPUid
            string CpuIdThis = GetCpuId();           //获取本机CPUId         
            if (CpuId != CpuIdThis)
            {
                return 2;
            }

            /* 比较时间 */
            string NowDate = TimeClass.GetNowDate();
            string EndDate = TimeClass.GetSoftEndDateAllCpuId(0, SericalNumber);
            if (Convert.ToInt32(EndDate) - Convert.ToInt32(NowDate) < 0)
            {
                return 3;
            }

            return 0;
        }


         /*CPUid*/
        public static string GetCpuId()
        {            
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();

            string strCpuID = null;
            foreach (ManagementObject mo in moc)
            {
                strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return strCpuID;
        }

        public static string GetNetworkAdpaterID()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac += mo["MacAddress"].ToString() + " ";
                        break;
                    }
                moc = null;
                mc = null;
                return mac.Trim();
            }
            catch (Exception e)
            {
                return "uMnIk";
            }
        }

        /*当前时间*/
        public static string GetNowDate()
        {
            string NowDate = DateTime.Now.ToString("yyyyMMdd");
            return NowDate;
        }

        /* 生成序列号 */
        public static string CreatSerialNumber()
        {
            string SerialNumber = GetCpuId() + "-" + "20110915";
            return SerialNumber; 
        }

        /* 
         * i=1 得到 CUP 的id 
         * i=0 得到上次或者 开始时间 
         */
        public static string GetSoftEndDateAllCpuId(int i, string SerialNumber)
        {
            if (i == 1)
            {
                string cupId = SerialNumber.Substring(0, SerialNumber.LastIndexOf("-")); // .LastIndexOf("-"));

                return cupId;
            }
            if (i == 0)
            {
                string dateTime = SerialNumber.Substring(SerialNumber.LastIndexOf("-") + 1);            
                return dateTime;
            }
            else
            {
                return string.Empty;
            }         
        }

        /*写入注册表*/
        public static void WriteSetting(string Section, string Key, string Setting)  // name = key  value=setting  Section= path
        {
            string text1 = Section;
            RegistryKey key1 = Registry.CurrentUser.CreateSubKey("Software\\MyTest_ChildPlat\\ChildPlat"); 
            if (key1 == null)
            {
                return;
            }
            try
            {
                key1.SetValue(Key, Setting);
            }
            catch (Exception exception1)
            {
                return;
            }
            finally
            {
                key1.Close();
            }

        }

        /*读取注册表*/
        public static string ReadSetting(string Section, string Key, string Default)
        {
            if (Default == null)
            {
                Default = "-1";
            }
            string text2 = Section;
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey("Software\\MyTest_ChildPlat\\ChildPlat");
            if (key1 != null)
            {
                object obj1 = key1.GetValue(Key, Default);
                key1.Close();
                if (obj1 != null)
                {
                    if (!(obj1 is string))
                    {
                        return "-1";
                    }
                    string obj2 = obj1.ToString();
                    obj2 = DESEncrypt.Decrypt(obj2);
                    return obj2;
                }
                return "-1";
            }
            return Default;
        }
    }

}
