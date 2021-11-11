using System.Text.RegularExpressions;
using System.Web;

namespace Mi.Common
{
    /// <summary>
    /// HTML处理帮助类
    /// </summary>
    public class HTMLHelper
    {
        /// <summary>
        /// 层级显示HTML
        /// </summary>
        public static string strLevel(int level) {
            string str = "";
            for (int i = 1; i <= level; i++)
            {
                str += "&nbsp;&nbsp;&nbsp;";
            }
            return str;
        }

        /// <summary>
        /// 清楚HTML标签
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string NoHtml(string text) {
            if (text == null) {
                text = "";
            }
            //删除脚本
            text = Regex.Replace(text, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            text = Regex.Replace(text, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"-->", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<!--.*", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            text.Replace("<", "");
            text.Replace(">", "");
            text.Replace("\r\n", "");
            text = HttpUtility.HtmlDecode(text).Trim();
            return text;
        }
    }
}
