using Nelibur.ObjectMapper;
using System.Collections.Generic;
using System.Linq;

namespace Mi.Common
{
    /// <summary>
    /// 添 加 人：LM
    /// 添加时间：20210629
    /// 内容说明：实体映射帮助类
    /// 更新说明：
    /// </summary>
    public class TinyHelper<TSource, TTarget>
    {
        /// <summary>
        /// 实体映射
        /// </summary>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static TTarget AutoMapper(TSource source)
        {
            TinyMapper.Bind<TSource, TTarget>();
            return TinyMapper.Map<TTarget>(source);
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns></returns>
        public static TTarget AutoMapper(TSource source, TTarget target)
        {
            TinyMapper.Bind<TSource, TTarget>();
            return TinyMapper.Map(source, target);
        }

        ///// <summary>
        ///// 结果列表实体映射
        ///// </summary>
        ///// <param name="source">源结果列表数据</param>
        ///// <returns>目标结果列表数据</returns>
        //public static ResultData<List<TTarget>> AutoMapper(ResultData<List<TSource>> source)
        //{
        //    TinyMapper.Bind<ResultData<List<TSource>>, ResultData<List<TTarget>>>();
        //    return TinyMapper.Map<ResultData<List<TTarget>>>(source);
        //}

        ///// <summary>
        ///// 结果实体映射
        ///// </summary>
        ///// <param name="source">源结果数据</param>
        ///// <returns></returns>
        //public static ResultData<TTarget> AutoMapper(ResultData<TSource> source)
        //{
        //    TinyMapper.Bind<ResultData<TSource>, ResultData<TTarget>>();
        //    return TinyMapper.Map<ResultData<TTarget>>(source);
        //}

        /// <summary>
        /// 列表实体映射
        /// </summary>
        /// <param name="source">源列表数据</param>
        /// <returns>目标列表数据</returns>
        public static List<TTarget> AutoMapper(List<TSource> source) {
            TinyMapper.Bind<TSource, TTarget>();
            List<TTarget> _list = new List<TTarget>();
            foreach (var item in source)
                _list.Add(TinyMapper.Map<TTarget>(item));
            return _list;
        }

        /// <summary>
        /// 实体映射(非NULL属性)
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns></returns>
        public static TTarget NoNullMapper(TSource source, TTarget target)
        {
            foreach (var item in typeof(TTarget).GetProperties().Where(x => x.PropertyType.IsPublic && x.CanWrite))
            {
                var sourceItem = typeof(TSource).GetProperty(item.Name);

                //判断实体的读写权限
                if (sourceItem == null || !sourceItem.CanRead || sourceItem.PropertyType.IsNotPublic)
                    continue;

                if (sourceItem.GetValue(source) == null)
                    continue;

                item.SetValue(target, sourceItem.GetValue(source));

            }
            return target;
        }

        /// <summary>
        /// 实体映射(非NULL属性)
        /// </summary>
        /// <param name="source">源数据</param>
        public static TTarget NoNullMapper(TSource source)
        {
            TTarget target = System.Activator.CreateInstance<TTarget>();
            foreach (var item in typeof(TTarget).GetProperties().Where(x => x.PropertyType.IsPublic && x.CanWrite))
            {
                var sourceItem = typeof(TSource).GetProperty(item.Name);

                //判断实体的读写权限
                if (sourceItem == null || !sourceItem.CanRead || sourceItem.PropertyType.IsNotPublic)
                    continue;

                if (sourceItem.GetValue(source) == null)
                    continue;

                item.SetValue(target, sourceItem.GetValue(source));
            }
            return target;
        }
    }

    /// <summary>
    /// 添 加 人：LM
    /// 添加时间：20210629
    /// 内容说明：实体映射帮助类
    /// 更新说明：
    /// </summary>
    public class TinyHelper<TTarget>
    {
        /// <summary>
        /// 用类型实体映射(非NULL属性)
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns></returns>
        public static TTarget NoNullMapper(TTarget source, TTarget target)
        {
            foreach (var item in typeof(TTarget).GetProperties().Where(x => x.PropertyType.IsPublic && x.CanWrite))
            {
                //判断实体的读写权限
                if (item == null || !item.CanRead || item.PropertyType.IsNotPublic)
                    continue;

                if (item.GetValue(source) == null)
                    continue;

                item.SetValue(target, item.GetValue(source));
            }
            return target;
        }
    }
}
