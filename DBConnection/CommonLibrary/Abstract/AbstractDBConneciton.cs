using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CommonLibrary.Abstract
{
    /// <summary>
    /// DBConnection抽象クラス
    /// </summary>
    public abstract class AbstractDBConnection : IDisposable
    {

        /// <summary>
        /// SQLコネクションプロパティ
        /// </summary>
        protected SqlConnection SqlConnection { get; }

        /// <summary>
        /// コンストラクタ
        /// 接続文字列を引数に
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public AbstractDBConnection(string connectionString)
        {

            // データベース接続の準備
            this.SqlConnection = new SqlConnection(connectionString);

            //接続開始
            this.SqlConnection.Open();
        }

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
        public abstract SqlTransaction BeginTransaction();


        /// <summary>
        /// セレクト関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL文</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public abstract IEnumerable<T> Select<T>(string sql, IEnumerable<CommandParameter> parameters = null, SqlTransaction tran = null);

        /// <summary>
        /// SELECT文　戻り値：匿名型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">匿名型オブジェクト</param>
        /// <param name="sql">SQK文</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns></returns>
        public abstract IEnumerable<T> Select<T>(T a, string sql, IEnumerable<CommandParameter> parameters = null);

        /// <summary>
        /// インサート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="entity">INSERTの対象となるエンティティ</param>
        public abstract void Insert<T>(SqlTransaction transaction, T entity);

        /// <summary>
        /// バルクインサート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="entitties">INSERTの対象となるエンティティ</param>
        public abstract void BulkInsert<T>(SqlTransaction transaction, IEnumerable<T> entitties);

        /// <summary>
        /// アップデート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="builder">EntityModifyBuilder</param>
        /// <param name="phraseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">Where句のパラメータ</param>
        public abstract void Update<T>(SqlTransaction transaction, EntityModifyBuilder<T> builder, string phraseWhere, IEnumerable<CommandParameter> parameters = null);

        /// <summary>
        /// デリート関数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="pharaseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">Where句のパラメータ</param>
        public abstract void Delete<T>(SqlTransaction transaction, string pharaseWhere, IEnumerable<CommandParameter> parameters = null);
    }
}
