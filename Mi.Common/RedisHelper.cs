using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mi.Common {

    /// <summary>
    /// Redis帮助类
    /// </summary>
    public static class RedisHelper {

        private static string _redisConnectionString;
        private static readonly object Locker = new object();
        private static string _prefixKey;
        private static ConnectionMultiplexer _instance;
        public static int DbNum = 10;

        //连接字符串
        public static string RedisConnectionString {
            set {
                _redisConnectionString = value;
            }
            get {
                return _redisConnectionString;
            }
        }

        //Key 前缀
        public static string PrefixKey {
            set {
                _prefixKey = value;
            }
            get {
                return _prefixKey;
            }
        }

        /// <summary>
        /// 单例获取
        /// </summary>
        public static ConnectionMultiplexer Instance {
            get {
                if (_instance == null || !_instance.IsConnected) {
                    lock (Locker) {
                        _instance = GetManager();
                    }
                }
                return _instance;
            }
        }

        private static ConnectionMultiplexer GetManager(string connectionString = null) {
            connectionString = connectionString ?? _redisConnectionString;
            try {
                return ConnectionMultiplexer.Connect(connectionString);
            } catch {
                return default(ConnectionMultiplexer);
            }
        }

        #region String
        #region 同步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?)) {
            key = AddSysCustomKey(key);
            return Do(db => db.StringSet(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues) {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return Do(db => db.StringSet(newkeyValues.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool StringSet<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?)) {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool StringSet<T>(string key, T obj, DateTime expiry) {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry - DateTime.Now));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static string StringGet(string key) {
            key = AddSysCustomKey(key);
            return Do(db => db.StringGet(key));
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public static RedisValue[] StringGet(List<string> listKey) {
            List<string> newKeys = listKey.Select(AddSysCustomKey).ToList();
            return Do(db => db.StringGet(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGet<T>(string key) {
            key = AddSysCustomKey(key);
            return Do(db => ConvertObj<T>(db.StringGet(key)));
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static double StringIncrement(string key, double val = 1) {
            key = AddSysCustomKey(key);
            return Do(db => db.StringIncrement(key, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static double StringDecrement(string key, double val = 1) {
            key = AddSysCustomKey(key);
            return Do(db => db.StringDecrement(key, val));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?)) {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringSetAsync(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues) {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return await Do(db => db.StringSetAsync(newkeyValues.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?)) {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return await Do(db => db.StringSetAsync(key, json, expiry));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static async Task<string> StringGetAsync(string key) {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringGetAsync(key));
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public static async Task<RedisValue[]> StringGetAsync(List<string> listKey) {
            List<string> newKeys = listKey.Select(AddSysCustomKey).ToList();
            return await Do(db => db.StringGetAsync(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> StringGetAsync<T>(string key) {
            key = AddSysCustomKey(key);
            string result = await Do(db => db.StringGetAsync(key));
            return ConvertObj<T>(result);

        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> StringIncrementAsync(string key, double val = 1) {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringIncrementAsync(key, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static async Task<double> StringDecrementAsync(string key, double val = 1) {
            key = AddSysCustomKey(key);
            return await Do(db => db.StringDecrementAsync(key, val));
        }

        #endregion 异步方法
        #endregion String

        #region Hash

        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashExists(string key, string dataKey) {
            key = AddSysCustomKey(key);
            return Do(db => db.HashExists(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool HashSet<T>(string key, string dataKey, T t) {
            key = AddSysCustomKey(key);
            return Do(db => {
                string json = ConvertJson(t);
                return db.HashSet(key, dataKey, json);
            });
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashDelete(string key, string dataKey) {
            key = AddSysCustomKey(key);
            return Do(db => db.HashDelete(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public static long HashDelete(string key, List<RedisValue> dataKeys) {
            key = AddSysCustomKey(key);
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return Do(db => db.HashDelete(key, dataKeys.ToArray()));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T HashGet<T>(string key, string dataKey) {
            key = AddSysCustomKey(key);
            return Do(db => {
                string value = db.HashGet(key, dataKey);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static double HashIncrement(string key, string dataKey, double val = 1) {
            key = AddSysCustomKey(key);
            return Do(db => db.HashIncrement(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static double HashDecrement(string key, string dataKey, double val = 1) {
            key = AddSysCustomKey(key);
            return Do(db => db.HashDecrement(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> HashKeys<T>(string key) {
            key = AddSysCustomKey(key);
            return Do(db => {
                RedisValue[] values = db.HashKeys(key);
                return ConvetList<T>(values);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> HashValues<T>(string key) {
            var result = new List<T>();
            key = AddSysCustomKey(key);
            return Do(db => {
                HashEntry[] arr = db.HashGetAll(key);
                foreach (var item in arr) {
                    if (!item.Value.IsNullOrEmpty) {
                        result.Add(JsonConvert.DeserializeObject<T>(item.Value));
                    }
                }
                return result;
            });
        }

        public static HashEntry[] HashValueAll(string key) {
            key = AddSysCustomKey(key);
            return Do(db => {
                HashEntry[] arr = db.HashGetAll(key);
                return arr;
            });
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<bool> HashExistsAsync(string key, string dataKey) {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashExistsAsync(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static async Task<bool> HashSetAsync<T>(string key, string dataKey, T t) {
            key = AddSysCustomKey(key);
            return await Do(db => {
                string json = ConvertJson(t);
                return db.HashSetAsync(key, dataKey, json);
            });
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<bool> HashDeleteAsync(string key, string dataKey) {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public static async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys) {
            key = AddSysCustomKey(key);
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return await Do(db => db.HashDeleteAsync(key, dataKeys.ToArray()));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<T> HashGeAsync<T>(string key, string dataKey) {
            key = AddSysCustomKey(key);
            string value = await Do(db => db.HashGetAsync(key, dataKey));
            return ConvertObj<T>(value);

        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1) {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1) {
            key = AddSysCustomKey(key);
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> HashKeysAsync<T>(string key) {
            key = AddSysCustomKey(key);
            RedisValue[] values = await Do(db => db.HashKeysAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> HashValuesAsync<T>(string key) {
            var result = new List<T>();
            key = AddSysCustomKey(key);
            HashEntry[] arr = await Do(db => db.HashGetAllAsync(key));
            foreach (var item in arr) {
                string values = item.Name;
                if (!item.Value.IsNullOrEmpty) {
                    result.Add(JsonConvert.DeserializeObject<T>(item.Value));
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<HashEntry[]> HashValueAllAsync(string key) {
            key = AddSysCustomKey(key);
            HashEntry[] arr = await Do(db => db.HashGetAllAsync(key));
            return arr;
        }
        #endregion 异步方法

        #endregion Hash

        #region List

        #region 同步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ListRemove<T>(string key, T value) {
            key = AddSysCustomKey(key);
            Do(db => db.ListRemove(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> ListRange<T>(string key) {
            key = AddSysCustomKey(key);
            return Do(redis => {
                var values = redis.ListRange(key);
                return ConvetList<T>(values);
            });
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ListRightPush<T>(string key, T value) {
            key = AddSysCustomKey(key);
            Do(db => db.ListRightPush(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(string key) {
            key = AddSysCustomKey(key);
            return Do(db => {
                var value = db.ListRightPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ListLeftPush<T>(string key, T value) {
            key = AddSysCustomKey(key);
            Do(db => db.ListLeftPush(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(string key) {
            key = AddSysCustomKey(key);
            return Do(db => {
                var value = db.ListLeftPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long ListLength(string key) {
            key = AddSysCustomKey(key);
            return Do(redis => redis.ListLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<long> ListRemoveAsync<T>(string key, T value) {
            key = AddSysCustomKey(key);
            return await Do(db => db.ListRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> ListRangeAsync<T>(string key) {
            key = AddSysCustomKey(key);
            var values = await Do(redis => redis.ListRangeAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<long> ListRightPushAsync<T>(string key, T value) {
            key = AddSysCustomKey(key);
            return await Do(db => db.ListRightPushAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListRightPopAsync<T>(string key) {
            key = AddSysCustomKey(key);
            var value = await Do(db => db.ListRightPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<long> ListLeftPushAsync<T>(string key, T value) {
            key = AddSysCustomKey(key);
            return await Do(db => db.ListLeftPushAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListLeftPopAsync<T>(string key) {
            key = AddSysCustomKey(key);
            var value = await Do(db => db.ListLeftPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> ListLengthAsync(string key) {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.ListLengthAsync(key));
        }

        #endregion 异步方法

        #endregion List

        #region SortedSet 有序集合

        #region 同步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public static bool SortedSetAdd<T>(string key, T value, double score) {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetAdd(key, ConvertJson<T>(value), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool SortedSetRemove<T>(string key, T value) {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetRemove(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> SortedSetRangeByRank<T>(string key) {
            key = AddSysCustomKey(key);
            return Do(redis => {
                var values = redis.SortedSetRangeByRank(key);
                return ConvetList<T>(values);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SortedSetLength(string key) {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public static async Task<bool> SortedSetAddAsync<T>(string key, T value, double score) {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.SortedSetAddAsync(key, ConvertJson<T>(value), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<bool> SortedSetRemoveAsync<T>(string key, T value) {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.SortedSetRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> SortedSetRangeByRankAsync<T>(string key) {
            key = AddSysCustomKey(key);
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> SortedSetLengthAsync(string key) {
            key = AddSysCustomKey(key);
            return await Do(redis => redis.SortedSetLengthAsync(key));
        }

        #endregion 异步方法

        #endregion SortedSet 有序集合

        #region key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public static bool KeyDelete(string key) {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyDelete(key));
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public static long KeyDelete(List<string> keys) {
            List<string> newKeys = keys.Select(AddSysCustomKey).ToList();
            return Do(db => db.KeyDelete(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static bool KeyExists(string key) {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExists(key));
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public static bool KeyRename(string key, string newKey) {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyRename(key, newKey));
        }

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?)) {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExpire(key, expiry));
        }

        /// <summary>
        /// 获取所有KEY
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllCacheKeys() {
            try {
                var result = new List<string>();
                var endpoints = Instance.GetEndPoints(true);
                foreach (var endpoint in endpoints) {
                    var server = Instance.GetServer(endpoint);
                    result.AddRange(server.Keys(DbNum).Select(x => x.ToString()));
                }
                return result.Distinct().ToList();
            } catch {
                return null;
            }
        }

        /// <summary>
        /// 获取所有KEY-Value
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, object> GetAll() {
            try {
                var result = new Dictionary<string, object>();
                IDatabase idb = Instance.GetDatabase(DbNum);
                var endpoints = Instance.GetEndPoints(true);
                foreach (var endpoint in endpoints) {
                    var server = Instance.GetServer(endpoint);
                    foreach (var item in server.Keys(DbNum)) {
                        result.Add(item.ToString(), idb.StringGet(item.ToString()));
                    }
                }
                return result;
            } catch {
                return null;
            }
        }


        #endregion key

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public static void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null) {
            ISubscriber sub = Instance.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) => {
                if (handler == null) {
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                } else {
                    handler(channel, message);
                }
            });
        }

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static long Publish<T>(string channel, T msg) {
            ISubscriber sub = Instance.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public static void Unsubscribe(string channel) {
            ISubscriber sub = Instance.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public static void UnsubscribeAll() {
            ISubscriber sub = Instance.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅

        #region 其他

        public static ITransaction CreateTransaction() {
            return GetDatabase().CreateTransaction();
        }

        public static IDatabase GetDatabase() {
            return Instance.GetDatabase(DbNum);
        }

        public static IServer GetServer(string hostAndPort) {
            return Instance.GetServer(hostAndPort);
        }
        #endregion 其他

        #region 辅助方法

        private static string AddSysCustomKey(string oldKey) {
            return _prefixKey + oldKey;
        }

        private static T Do<T>(Func<IDatabase, T> func) {
            try {
                var database = Instance.GetDatabase(DbNum);
                return func(database);
            } catch (Exception ex) {
                Console.WriteLine($"reids.do:{ex.Message}");
                return default(T);
            }
        }

        private static string ConvertJson<T>(T value) {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value, settings);
            return result;
        }

        private static T ConvertObj<T>(RedisValue value) {
            if (typeof(T).Name.Equals(typeof(string).Name)) {
                return JsonConvert.DeserializeObject<T>($"'{value}'");
            } else {
                T t = JsonConvert.DeserializeObject<T>(value);
                if (t == null) {
                    Console.WriteLine($"reids.ConvertObj.t:{null}");
                }
                return t;
            }
        }


        private static List<T> ConvetList<T>(RedisValue[] values) {
            List<T> result = new List<T>();
            foreach (var item in values) {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        private static RedisKey[] ConvertRedisKeys(List<string> redisKeys) {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }

        #endregion 辅助方法
    }
}
