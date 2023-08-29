using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnectionTools.Abstract
{
    /// <summary>
    /// DBConnection抽象クラス
    /// </summary>
    public abstract class AbstractDBConnection<U, V>  where U : DbConnection where V : DbTransaction , IDisposable
    {
        /// <summary>
        /// SQLコネクションプロパティ
        /// </summary>
        protected abstract U SqlConnection { get; }

        /// <summary>
        /// Dispose実装
        /// </summary>
        public void Dispose()
        {
            if (SqlConnection != null) this.SqlConnection.Close();
            if (SqlConnection != null) this.SqlConnection.Dispose();
        }

        /// <summary>
        /// オープントランザクション関数
        /// </summary>
        /// <returns></returns>
        public abstract V BeginTransaction();

        /// <summary>
        /// セレクト関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL文</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public abstract IEnumerable<T> Select<T>(string sql, IEnumerable<CommandParameter> parameters = null, V tran = null);

        /// <summary>
        /// SELECT文　戻り値：匿名型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">匿名型オブジェクト</param>
        /// <param name="sql">SQK文</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns></returns>
        public abstract IEnumerable<T> Select<T>(T a, string sql, IEnumerable<CommandParameter> parameters = null, V tran = null);

        /// <summary>
        /// インサート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="entity">INSERTの対象となるエンティティ</param>
        public abstract void Insert<T>(V transaction, T entity);

        /// <summary>
        /// バルクインサート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="entitties">INSERTの対象となるエンティティ</param>
        public abstract void BulkInsert<T>(V transaction, IEnumerable<T> entitties);

        /// <summary>
        /// アップデート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="builder">EntityModifyBuilder</param>
        /// <param name="phraseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">Where句のパラメータ</param>
        public abstract void Update<T>(V transaction, EntityModifyBuilder<T> builder, string phraseWhere, IEnumerable<CommandParameter> parameters = null);

        /// <summary>
        /// デリート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="pharaseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">Where句のパラメータ</param>
        public abstract void Delete<T>(V transaction, string pharaseWhere, IEnumerable<CommandParameter> parameters = null);
    }
}
