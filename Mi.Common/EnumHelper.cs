using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Mi.Common
{
    /// <summary>
    /// 得到enum字段描述列表 由枚举值得到枚举描述
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 得到enum值描述列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> EnumValueDescription(Type enumType)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            Type type = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                object[] arr = field.GetCustomAttributes(type, true);
                if (arr.Length > 0)
                {
                    dic.Add((int)Enum.Parse(enumType, field.Name), ((DescriptionAttribute)arr[0]).Description);
                }
            }

            return dic;
        }

        /// <summary>
        /// 得到enum字段描述列表
        /// </summary>
        /// <returns>返回字典</returns>
        public static Dictionary<string, string> EnumFieldDescription(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type type = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                object[] arr = field.GetCustomAttributes(type, true);
                if (arr.Length > 0)
                {
                    dic.Add(field.Name, ((DescriptionAttribute)arr[0]).Description);
                }
            }
            return dic;
        }

        /// <summary>
        /// 由枚举值得到枚举描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>g.
        /// <returns></returns>
        public static string GetEnumDescription<T>(string value)
        {
            Type type = typeof(T);
            Dictionary<string, string> dic = EnumFieldDescription(type);
            string description = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (dic.ContainsKey(Convert.ToString((T)Enum.Parse(type, value))))
                {
                    description = dic[Convert.ToString((T)Enum.Parse(type, value))];
                }
            }
            return description;
        }

        /// <summary>
        /// 由枚举描述得到枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static int GetEnumDescriptionvalue<T>(string description)
        {
            Type type = typeof(T);
            Dictionary<int, string> dic = EnumValueDescription(type);
            int value = 0;
            if (!string.IsNullOrWhiteSpace(description))
            {
                foreach (KeyValuePair<int, string> pair in dic)
                {
                    if (pair.Value == description)
                    {
                        value = pair.Key;
                        return value;
                    }
                }
            }
            return value;
        }
    }
}