using Dapper;
using DBConnectionTools;
using DBConnectionTools.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace DBConnectionForSQLServer
{
    /// <summary>
    /// データベースコネクションクラス
    /// </summary>
    public class DataBaseConnection : AbstractDBConnection
    {

        /// <summary>
        /// コンストラクタ
        /// 接続文字列を引数に
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public DataBaseConnection(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        /// <param name="disposing"></param>
        public override SqlTransaction BeginTransaction()
        {
            return SqlConnection.BeginTransaction();

        }

        /// <summary>
        /// SELECT文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">生SQL文</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="tran">トランザクション</param>
        /// <returns></returns>
        public override IEnumerable<T> Select<T>(string sql, IEnumerable<CommandParameter> parameters = null, SqlTransaction tran = null)
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
        /// SELECT文　匿名型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">匿名型のオブジェクトを渡す</param>
        /// <param name="sql">SQL文</param>
        /// <param name="parameters">パラメータ's</param>
        /// <returns></returns>
        public override IEnumerable<T> Select<T>(T a, string sql, IEnumerable<CommandParameter> parameters = null, SqlTransaction tran = null)
        {
            return this.Select<T>(sql, parameters, tran);
        }


        /// <summary>
        /// INSERT文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">INSERTの対象となるエンティティ</param>
        /// <param name="transaction">トランザクション</param>
        public override void Insert<T>(SqlTransaction transaction, T entity)
        {
            //Insert文実行
            //BulkInsert呼び出し
            //エンティティが一つの場合でもインサート可
            this.BulkInsert(transaction, new T[] { entity });
        }

        ///// <summary>
        ///// bulkInsert文
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="transaction"></param>
        ///// <param name="entitties"></param>
        public override void BulkInsert<T>(SqlTransaction transaction, IEnumerable<T> entitties)
        {
            //エンティティのプロパティ情報取得
            var properties = typeof(T).GetProperties();

            //データテーブルインスタンス生成
            var dataTable = new DataTable();

            //データテーブル作成
            foreach (var property in properties)
            {

                if (property.PropertyType.IsGenericType
            && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) //null許容型の時
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

            try
            {
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
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// BulkInsert文 旧バージョン
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitties">INSERTの対象となるエンティティ</param>
        /// <param name="transaction">トランザクション</param>
        //public override void BulkInsert<T>(SqlTransaction transaction, IEnumerable<T> entitties)
        //{
        //    //SqlCommandに、コネクションとトランザクションを入れる
        //    SqlCommand command = new SqlCommand();
        //    command.Connection = SqlConnection;
        //    command.Transaction = transaction;

        //    //insert sql文作成  sql文に「INSERT INTO テーブル名 (」を入れる
        //    StringBuilder sql = new(@"INSERT INTO " + typeof(T).Name + " (");

        //    //エンティティからプロパティ名を取得する処理
        //    //取得したプロパティ名をsql文に入れる
        //    foreach (var prop in typeof(T).GetProperties())
        //    {
        //        //初めのプロパティ名だけコンマをつけない処理
        //        //２つ目以降のプロパティ名だけにコンマをつける
        //        if (!ReferenceEquals(prop, typeof(T).GetProperties().First()))
        //        {
        //            sql.AppendLine(",");
        //        }
        //        sql.AppendLine(prop.Name);
        //    }

        //    //パラメータをセットするための準備
        //    sql.AppendLine(") VALUES");

        //    //パラメータをセットするリスト
        //    var parameters = new List<CommandParameter>();

        //    //INSERTのVALUES以降のSQL文の作成
        //    //パラメータの作成とパラメータに値を入れる処理
        //    //IEnumerable<T> entitties にインデックスをつけ、
        //    //パラメータの末尾にインデックスを付与し、
        //    //他のパラメータと区別させる
        //    foreach (var entity in entitties.Select((item, index) => new { item, index }))
        //    {
        //        sql.AppendLine("(");

        //        //エンティティからプロパティ名を取得し、パラメータ作成 sql文に入れる
        //        foreach (var prop in typeof(T).GetProperties())
        //        {
        //            //初めのプロパティ名だけコンマをつけない処理
        //            //２つ目以降のプロパティ名だけにコンマをつける
        //            if (!ReferenceEquals(prop, typeof(T).GetProperties().First()))
        //            {
        //                sql.AppendLine(",");
        //            }
        //            //インデックスを付与しパラメータを区別する
        //            sql.AppendLine("@" + prop.Name + entity.index);

        //            //パラメータに値をセットする
        //            parameters.Add(new CommandParameter("@" + prop.Name + entity.index, prop.GetValue(entity.item)));
        //        }

        //        //最後の値は、コンマをつけない
        //        sql.AppendLine(")");

        //        if (entity.index != entitties.Count() - 1)
        //        {
        //            sql.AppendLine(",");
        //        }
        //    }

        //    //パラメータをコマンドにセットする
        //    foreach (var parameter in parameters)
        //    {
        //        //コマンドにパラメータ名、parameterオブジェクトのデータの型、パラメータに格納する値をセットする
        //        var param = new SqlParameter(parameter.Name, parameter.Value);
        //        command.Parameters.Add(param);
        //    }

        //    //CommandにSqlを入れる
        //    command.CommandText = sql.ToString();

        //    //INSERT実行
        //    command.ExecuteNonQuery();
        //}




        /// <summary>
        /// UPDATE文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="builder">EntityModifyBuilder</param>
        /// <param name="phraseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">Where句のパラメータ</param>
        public override void Update<T>(SqlTransaction transaction, EntityModifyBuilder<T> builder, string phraseWhere, IEnumerable<CommandParameter> parameters = null)
        {
            //SqlCommandに、コネクションとトランザクションを入れる
            SqlCommand command = new SqlCommand();
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
            //発行SQL
            Debug.WriteLine(sql);
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
        public override void Delete<T>(SqlTransaction transaction, string pharaseWhere, IEnumerable<CommandParameter> parameters = null)
        {
            //SqlCommandに、コネクションとトランザクションをセットする
            SqlCommand command = new SqlCommand();
            command.Connection = SqlConnection;
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
