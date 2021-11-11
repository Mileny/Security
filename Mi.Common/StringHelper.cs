using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mi.Common
{
    /// <summary>
    /// 添 加 人：LM
    /// 添加时间：20210629
    /// 内容说明：字符串帮助类
    /// 更新说明：
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// MD5转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            using (var md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "");
            }
        }

        /// <summary>
        /// 验证是否是手机号码
        /// </summary>
        public static bool IsMobile(string str)
        {
            Regex reg = new Regex(@"^1[3-9]\d{9}$");
            if (reg.IsMatch(str))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证是否是邮箱地址
        /// </summary>
        public static bool IsEmail(string str)
        {
            Regex reg = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (reg.IsMatch(str))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 生产随机字符串
        /// </summary>
        /// <param name="num">多少位字符</param>
        /// <returns></returns>
        public static string RandomString(int num)
        {
            string vChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q,R,S,T,U,V,W,X,Y,Z";

            string[] vcArray = vChar.Split(new Char[] { ',' });//拆分成数组   
            string code = "";//产生的随机数  
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < num + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(61);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return RandomString(num);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += vcArray[t];//随机数的位数加一  
            }
            return code;
        }

        /// <summary>
        /// 是不是逗号开头和结尾
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCommaStartAndEnd(string str) {
            Regex reg = new Regex(@"^,\S+,$");
            return reg.IsMatch(str);
        }

        /// <summary>
        /// 生产特定随机字符串
        /// </summary>
        /// <param name="num"></param>
        /// <param name="type">0、数字 1、大写字母 2、小写字母 3、特殊字符 </param>
        /// <returns></returns>
        public static string RandomString(int num, int type){
            string vChar;
            switch (type) {
                case 1:
                    vChar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q,R,S,T,U,V,W,X,Y,Z";
                    break;
                case 2:
                    vChar = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z";
                    break;
                case 3:
                    vChar = "~,!,@,#,$,%,^,&,*,_,-,+,?,=";
                    break;
                default:
                    vChar = "0,1,2,3,4,5,6,7,8,9";
                    break;
            }
            string[] vcArray = vChar.Split(new Char[] { ',' });//拆分成数组   
            string code = "";//产生的随机数  
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < num + 1; i++) {
                if (temp != -1) {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(vcArray.Length);//获取随机数  
                if (temp != -1 && temp == t) {
                    return RandomString(num, type);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += vcArray[t];//随机数的位数加一  
            }
            return code;
        }

        #region 字符串加解密

        static string initKey = "7o8kdksESGSSjeGfFS6dee";
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strText">要加密的字符串。</param>
        /// <param name="strKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(string strText, string strKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                using (var md5 = MD5.Create())
                {

                    des.Key = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(strKey))).Replace("-", "").Substring(0, 8));
                    des.IV = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(strKey))).Replace("-", "").Substring(0, 8));
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Convert.ToBase64String(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strText">要加密的字符串。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(string strText)
        {
            return Encrypt(strText, initKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strText">要解密的以Base64</param>
        /// <param name="strKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string strText, string strKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(strText);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                using (var md5 = MD5.Create())
                {
                    des.Key = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(strKey))).Replace("-", "").Substring(0, 8));
                    des.IV = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(strKey))).Replace("-", "").Substring(0, 8));
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Encoding.UTF8.GetString(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strText">要解密的以Base64</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string strText)
        {
            return Decrypt(strText, initKey);
        }
        #endregion
    }
}
