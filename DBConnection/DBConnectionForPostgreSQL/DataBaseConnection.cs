using Dapper;
using DBConnectionTools;
using DBConnectionTools.Abstract;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DBConnectionForPostgreSQL
{
    /// <summary>
    /// ポスグレ用DBConnection
    /// </summary>
    public class DataBaseConnection : AbstractDBConnection<NpgsqlConnection, NpgsqlTransaction>, IDisposable
    {
        /// <summary>
        /// DBConneciton
        /// </summary>
        protected override NpgsqlConnection SqlConnection { get; }

        /// <summary>
        /// コンストラクタ
        /// 接続文字列を引数に
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public DataBaseConnection(string connectionString)
        {
            // データベース接続の準備
            this.SqlConnection = new NpgsqlConnection(connectionString);

            //接続開始
            this.SqlConnection.Open();
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        /// <returns></returns>
        public override NpgsqlTransaction BeginTransaction()
        {
            return SqlConnection.BeginTransaction();
        }

        /// <summary>
        /// SELECT文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">生SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public override IEnumerable<T> Select<T>(string sql, IEnumerable<CommandParameter> parameters = null, NpgsqlTransaction tran = null)
        {
            //パラメータインスタンス生成
            var dynamicParameters = new DynamicParameters();

            //パラメータの中身がある時だけ実行
            //パラメータを使わない場合null
            if (parameters != null)
            {
                //受けったパラメータの数だけ回す
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Name, parameter.Value, parameter.DBType);
                }
            }
            return SqlConnection.Query<T>(sql, dynamicParameters, tran);
        }

        /// <summary>
        /// SELECT文　匿名型用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">匿名型のオブジェクトを渡す</param>
        /// <param name="sql">SQL文</param>
        /// <param name="parameters">パラメータ</param>
        /// /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public override IEnumerable<T> Select<T>(T a, string sql, IEnumerable<CommandParameter> parameters = null, NpgsqlTransaction tran = null)
        {
            return this.Select<T>(sql, parameters, tran);
        }

        /// <summary>
        /// INSERT文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">INSERTの対象となるエンティティ</param>
        /// <param name="transaction">トランザクション</param>
        public override void Insert<T>(NpgsqlTransaction transaction, T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// BulkInsert文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="entitties"></param>
        public override void BulkInsert<T>(NpgsqlTransaction transaction, IEnumerable<T> entitties)
        {
            //エンティティのプロパティ情報取得
            var properties = typeof(T).GetProperties();

            //データテーブルインスタンス生成
            var dataTable = new DataTable();

            //データテーブル作成
            foreach (var property in properties)
            {

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) //null許容型の時
                {
                    dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType));
                }
                else //null非許容型の時
                {
                    dataTable.Columns.Add(property.Name, property.PropertyType);
                }
            }

            //登録するデータをデータテーブルに追加
            foreach (var entitiy in entitties)
            {
                //一行ずつ作成
                var row = dataTable.NewRow();

                //プロパティの一つ一つに値を入れる
                //TODO:値が入っているかいないかで分岐させる
                //DataRowって基本的にString型にしていれる必要あり
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(entitiy) ?? DBNull.Value;
                }

                //登録データ行追加
                dataTable.Rows.Add(row);
            }

            //バルクコピーインスタンス
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.SqlConnection, SqlBulkCopyOptions.Default, transaction))
            {
                //タイムアウト指定
                bulkCopy.BulkCopyTimeout = 60;

                //テーブル名指定
                bulkCopy.DestinationTableName = typeof(T).Name;

                //一括Insert実行
                bulkCopy.WriteToServer(dataTable);
            }
        }

        /// <summary>
        /// UPDATE文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="builder">EntityModifyBuilder</param>
        /// <param name="phraseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">Where句のパラメータ</param>
        public override void Update<T>(NpgsqlTransaction transaction, EntityModifyBuilder<T> builder, string phraseWhere, IEnumerable<CommandParameter> parameters = null)
        {
            //SqlCommandに、コネクションとトランザクションを入れる
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = SqlConnection;
            command.Transaction = transaction;

            //update sql文作成  sql文に「UPDATE テーブル名 SET」を入れる
            StringBuilder sql = new(@"UPDATE " + typeof(T).Name + " SET ");

            //アップデートするカラム名と値をEntityModifyBuilderから取り出す
            //リストの中のPropertyInfoからNameを取り出し、値と共にsql文を作成
            foreach (var map in builder.PropertyValueMaps)
            {
                //初めのプロパティ名だけコンマをつけない処理
                //２つ目以降のプロパティ名だけにコンマをつける
                if (!ReferenceEquals(map, builder.PropertyValueMaps.First()))
                {
                    sql.AppendLine(",");
                }

                //sql作成
                sql.AppendLine(map.PropertyInfo.Name + "=" + "@" + map.PropertyInfo.Name);

                //コマンドにパラメータ名、パラメータに格納する値をセットする
                var param = new SqlParameter(map.PropertyInfo.Name, map.Value);
                command.Parameters.Add(param);
            }

            // where文と共にsqlに入れる
            sql.AppendLine(phraseWhere);

            //sqlをコマンドにセットする
            command.CommandText = sql.ToString();

            //パラメータを使わない場合null
            if (parameters != null)
            {
                //where句のパラメータをコマンドにセットする
                foreach (var parameter in parameters)
                {
                    //コマンドにパラメータ名、パラメータに格納する値をセットする
                    var param = new SqlParameter(parameter.Name, parameter.Value);
                    command.Parameters.Add(param);
                }
            }

            //UPDATE実行
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// DELETE文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pharaseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="transaction">トランザクション</param>
        public override void Delete<T>(NpgsqlTransaction transaction, string pharaseWhere, IEnumerable<CommandParameter> parameters = null)
        {
            //SqlCommandに、コネクションとトランザクションをセットする
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.SqlConnection;
            command.Transaction = transaction;

            //delete sql文作成
            StringBuilder sql = new(@"DELETE FROM " + typeof(T).Name + " " + pharaseWhere);

            //sqlをコマンドにセットする
            command.CommandText = sql.ToString();


            //パラメータを使わない場合null
            if (parameters != null)
            {
                //where句のパラメータをコマンドにセットする
                foreach (var parameter in parameters)
                {
                    //コマンドにパラメータ名、パラメータに格納する値をセットする
                    var param = new SqlParameter(parameter.Name, parameter.Value);
                    command.Parameters.Add(param);
                }
            }

            //DELETE実行
            command.ExecuteNonQuery();
        }
    }
}
