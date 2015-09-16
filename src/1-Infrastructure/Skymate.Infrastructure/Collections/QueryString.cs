// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryString.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   定义QueryString类型.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Collections
{
    using System;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web;

    /// <summary>
    /// 查询字符串.
    /// </summary>
    public class QueryString : NameValueCollection
    {
        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryString"/> class.
        /// </summary>
        public QueryString()
        {
        }

        /// <summary>
        /// 创建 <see cref="QueryString"/> 实例.
        /// </summary>
        /// <param name="queryString">
        /// 查询字符串.
        /// </param>
        public QueryString(string queryString)
        {
            this.FillFromString(queryString);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取当前.
        /// </summary>
        public static QueryString Current
        {
            get
            {
                return (new QueryString()).FromCurrent();
            }
        }

        #endregion

        #region 索引

        /// <summary>
        /// 覆盖默认的的索引
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>相关解码值指定的名称</returns>
        public new string this[string name]
        {
            get
            {
                return HttpUtility.UrlDecode(base[name]);
            }
        }

        /// <summary>
        /// 覆盖默认的的索引
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns>相关解码值指定的名称</returns>
        public new string this[int index]
        {
            get
            {
                return HttpUtility.UrlDecode(base[index]);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 提取完整的URL的查询字符串
        /// </summary>
        /// <param name="s">字符串提取的变量的名称</param>
        /// <returns>只代表一个字符串参数</returns>
        public static string ExtractQuerystring(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.Contains("?") ? s.Substring(s.IndexOf("?", StringComparison.Ordinal) + 1) : s;
        }

        /// <summary>
        /// 基于字符串返回一个变量的对象
        /// </summary>
        /// <param name="s">字符串解析</param>
        /// <returns>查询字符串对象 </returns>
        public QueryString FillFromString(string s)
        {
            this.Clear();
            if (string.IsNullOrEmpty(s))
            {
                return this;
            }

            foreach (var keyValuePair in ExtractQuerystring(s).Split('&'))
            {
                if (string.IsNullOrEmpty(keyValuePair))
                {
                    continue;
                }

                var split = keyValuePair.Split('=');
                base.Add(split[0], split.Length == 2 ? split[1] : string.Empty);
            }

            return this;
        }

        /// <summary>
        /// 返回一个变量的对象基于当前请求的变量的名称
        /// </summary>
        /// <returns>the QueryString object </returns>
        public QueryString FromCurrent()
        {
            if (HttpContext.Current != null)
            {
                return this.FillFromString(HttpContext.Current.Request.QueryString.ToString());
            }

            this.Clear();
            return this;
        }

        /// <summary>
        /// 添加一个名称值对集合
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">关联的值的名称</param>
        /// <returns>QueryString对象 </returns>
        public new QueryString Add(string name, string value)
        {
            return this.Add(name, value, false);
        }

        /// <summary>
        /// 添加一个名称值对集合
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">关联的值的名称</param>
        /// <param name="isUnique">
        /// 如果在变量的名称是独一无二的。这使我们能够覆盖现有值
        /// </param>
        /// <returns>QueryString对象 </returns>
        public QueryString Add(string name, string value, bool isUnique)
        {
            var existingValue = base[name];
            if (string.IsNullOrEmpty(existingValue))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                base.Add(name, value: HttpUtility.UrlEncode(value));
            }
            else if (isUnique)
            {
                base[name] = HttpUtility.UrlEncode(value);
            }
            else
            {
                base[name] += "," + HttpUtility.UrlEncode(value);
            }

            return this;
        }

        /// <summary>
        /// 从查询字符串中删除一个名称值对集合
        /// </summary>
        /// <param name="name">名称属性值删除</param>
        /// <returns>QueryString对象 </returns>
        public new QueryString Remove(string name)
        {
            string existingValue = base[name];
            if (!string.IsNullOrEmpty(existingValue))
            {
                base.Remove(name);
            }

            return this;
        }

        /// <summary>
        /// 清除集合
        /// </summary>
        /// <returns>QueryString对象 </returns>
        public QueryString Reset()
        {
            this.Clear();
            return this;
        }

        /// <summary>
        /// 检查是否在查询字符串集合名称已经存在
        /// </summary>
        /// <param name="name">检查名称</param>
        /// <returns>一个布尔如果名字存在</returns>
        public bool Contains(string name)
        {
            string existingValue = base[name];
            return !string.IsNullOrEmpty(existingValue);
        }

        /// <summary>
        /// 输出一个字符串变量的对象
        /// </summary>
        /// <returns>
        /// 编码的变量的名称,因为它会出现在浏览器中
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < this.Keys.Count; i++)
            {
                if (string.IsNullOrEmpty(this.Keys[i]))
                {
                    continue;
                }

                foreach (var val in base[this.Keys[i]].Split(','))
                {
                    builder.Append((builder.Length == 0) ? "?" : "&")
                        .Append(HttpUtility.UrlEncode(this.Keys[i]))
                        .Append("=")
                        .Append(val);
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}
