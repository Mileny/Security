using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using System;

namespace Mi.Common
{
    /// <summary>
    /// 添 加 人：LM
    /// 添加时间：20210629
    /// 内容说明：阿里短信帮助类
    /// 更新说明：
    /// </summary>
    public class SMSHelper
    {
        public static string regionId { get; set; }

        public static string accessKeyId { get; set; }

        public static string secret { get; set; }

        public static string signName { get; set; }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <returns></returns>
        public static string BuildRandomNumber()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        /// <summary>
        /// 阿里云申请短信 待定
        /// </summary>
        /// <param name="signName">短信签名名称</param>
        /// <param name="tempCode">短信模版CODE</param>
        /// <param name="phoneNumbers">接收短信的手机号码，支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔。</param>
        /// <param name="tempParam">短信模板变量对应的实际值，JSON格式。 {"code":"1111"}</param>
        /// <returns></returns>
        public static string SendSMS(string signName, string tempCode, string phoneNumbers, string tempParam)
        {
            IClientProfile profile = DefaultProfile.GetProfile(regionId, accessKeyId, secret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            request.AddQueryParameters("PhoneNumbers", phoneNumbers);
            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("TemplateCode", tempCode);
            request.AddQueryParameters("TemplateParam", tempParam);
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                return System.Text.Encoding.UTF8.GetString(response.HttpResponse.Content);
            }
            catch (ServerException se)
            {
                return "{\"RequestId\":\"0\", \"Code\":\"Error\", \"Message\":\"" + se.ErrorMessage + "\", \"SignName\":\"\"}";
            }
            catch (ClientException ce)
            {
                return "{\"RequestId\":\"0\", \"Code\":\"Error\", \"Message\":\"" + ce.ErrorMessage + "\", \"SignName\":\"\"}";
            }
        }
    }
}
