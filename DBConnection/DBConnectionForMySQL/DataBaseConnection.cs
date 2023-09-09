﻿using Dapper;
using DBConnectionTools;
using DBConnectionTools.Abstract;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnectionForMySQL
{
    public class DataBaseConnection : AbstractDBConnection<MySqlConnection, MySqlTransaction>, IDisposable
    {
        /// <summary>
        /// DBConneciton
        /// </summary>
        protected override MySqlConnection SqlConnection { get; }

        /// <summary>
        /// コンストラクタ
        /// 接続文字列を引数に
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public DataBaseConnection(string connectionString)
        {
            // データベース接続の準備
            this.SqlConnection = new MySqlConnection(connectionString);

            //接続開始
            this.SqlConnection.Open();
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        /// <returns></returns>
        public override MySqlTransaction BeginTransaction()
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
        public override IEnumerable<T> Select<T>(string sql, IEnumerable<CommandParameter> parameters = null, MySqlTransaction tran = null)
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
        public override IEnumerable<T> Select<T>(T a, string sql, IEnumerable<CommandParameter> parameters = null, MySqlTransaction tran = null)
        {
            return this.Select<T>(sql, parameters, tran);
        }

        /// <summary>
        /// INSERT文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">INSERTの対象となるエンティティ</param>
        /// <param name="transaction">トランザクション</param>
        public override void Insert<T>(MySqlTransaction transaction, T entity, string tableName = "")
        {
            this.BulkInsert(transaction, new T[] { entity }, tableName);
            ////NpgsqlCommandに、コネクションとトランザクションを入れる
            //MySqlCommand command = new MySqlCommand();
            //command.Connection = SqlConnection;
            //command.Transaction = transaction;

            ////insert sql文作成  sql文に「INSERT INTO テーブル名 (」を入れる
            //StringBuilder sql = new(@"INSERT INTO " + typeof(T).Name + " (");

            ////エンティティからプロパティ名を取得する処理
            ////取得したプロパティ名をsql文に入れる
            //foreach (var prop in typeof(T).GetProperties())
            //{
            //    //初めのプロパティ名だけコンマをつけない処理
            //    //２つ目以降のプロパティ名だけにコンマをつける
            //    if (!ReferenceEquals(prop, typeof(T).GetProperties().First()))
            //    {
            //        sql.AppendLine(",");
            //    }
            //    sql.AppendLine(prop.Name);
            //}

            ////パラメータをセットするための準備
            //sql.AppendLine(") VALUES");

            ////パラメータをセットするリスト
            //var parameters = new List<CommandParameter>();

            ////INSERTのVALUES以降のSQL文の作成
            ////パラメータの作成とパラメータに値を入れる処理
            //sql.AppendLine("(");

            ////エンティティからプロパティ名を取得し、パラメータ作成 sql文に入れる
            //foreach (var prop in typeof(T).GetProperties())
            //{
            //    //初めのプロパティ名だけコンマをつけない処理
            //    //２つ目以降のプロパティ名だけにコンマをつける
            //    if (!ReferenceEquals(prop, typeof(T).GetProperties().First()))
            //    {
            //        sql.AppendLine(",");
            //    }
            //    //パラメータ名セット
            //    sql.AppendLine("@" + prop.Name);

            //    //パラメータに値をセットする
            //    parameters.Add(new CommandParameter("@" + prop.Name, prop.GetValue(entity)));
            //}

            ////最後の値は、コンマをつけない
            //sql.AppendLine(")");

            ////パラメータをコマンドにセットする
            //foreach (var parameter in parameters)
            //{
            //    //コマンドにパラメータ名、parameterオブジェクトのデータの型、パラメータに格納する値をセットする
            //    var param = new MySqlParameter(parameter.Name, parameter.Value);
            //    command.Parameters.Add(param);
            //}

            ////CommandにSqlを入れる
            //command.CommandText = sql.ToString();

            ////INSERT実行
            //command.ExecuteNonQuery();
        }

        /// <summary>
        /// BulkInsert文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="entitties"></param>
        public override void BulkInsert<T>(MySqlTransaction transaction, IEnumerable<T> entitties, string tableName = "")
        {
            //    SqlCommandに、コネクションとトランザクションを入れる
            MySqlCommand command = new MySqlCommand();
            command.Connection = SqlConnection;
            command.Transaction = transaction;

            StringBuilder sql;
            //insert sql文作成  sql文に「INSERT INTO テーブル名 (」を入れる
            if (tableName == "")
            {
                sql = new(@"INSERT INTO " + typeof(T).Name + " (");
            }
            else
            {
                sql = new(@"INSERT INTO " + tableName + " (");
            }

            //    エンティティからプロパティ名を取得する処理
            //    取得したプロパティ名をsql文に入れる
            foreach (var prop in typeof(T).GetProperties())
            {
                //初めのプロパティ名だけコンマをつけない処理
                //    ２つ目以降のプロパティ名だけにコンマをつける
                if (!ReferenceEquals(prop, typeof(T).GetProperties().First()))
                {
                    sql.AppendLine(",");
                }
                sql.AppendLine(prop.Name);
            }

            //    パラメータをセットするための準備
            sql.AppendLine(") VALUES");

            //    パラメータをセットするリスト
            var parameters = new List<CommandParameter>();

            //    INSERTのVALUES以降のSQL文の作成
            //    パラメータの作成とパラメータに値を入れる処理
            //    IEnumerable<T> entitties にインデックスをつけ、
            //    パラメータの末尾にインデックスを付与し、
            //    他のパラメータと区別させる
            foreach (var entity in entitties.Select((item, index) => new { item, index }))
            {
                sql.AppendLine("(");

                //        エンティティからプロパティ名を取得し、パラメータ作成 sql文に入れる
                foreach (var prop in typeof(T).GetProperties())
                {
                    //            初めのプロパティ名だけコンマをつけない処理
                    //            ２つ目以降のプロパティ名だけにコンマをつける
                    if (!ReferenceEquals(prop, typeof(T).GetProperties().First()))
                    {
                        sql.AppendLine(",");
                    }
                    //            インデックスを付与しパラメータを区別する
                    sql.AppendLine("@" + prop.Name + entity.index);

                    //            パラメータに値をセットする
                    parameters.Add(new CommandParameter("@" + prop.Name + entity.index, prop.GetValue(entity.item)));
                }

                //最後の値は、コンマをつけない
                sql.AppendLine(")");

                if (entity.index != entitties.Count() - 1)
                {
                    sql.AppendLine(",");
                }
            }

            //    パラメータをコマンドにセットする
            foreach (var parameter in parameters)
            {
                //        コマンドにパラメータ名、parameterオブジェクトのデータの型、パラメータに格納する値をセットする
                var param = new MySqlParameter(parameter.Name, parameter.Value);
                command.Parameters.Add(param);
            }

            //    CommandにSqlを入れる
            command.CommandText = sql.ToString();

            //    INSERT実行
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// UPDATE文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">トランザクション</param>
        /// <param name="builder">EntityModifyBuilder</param>
        /// <param name="phraseWhere">絞り込み　Where句：Whereから書いて</param>
        /// <param name="parameters">Where句のパラメータ</param>
        public override void Update<T>(MySqlTransaction transaction, EntityModifyBuilder<T> builder, string phraseWhere = "", IEnumerable<CommandParameter> parameters = null, string tableName = "")
        {
            //SqlCommandに、コネクションとトランザクションを入れる
            MySqlCommand command = new MySqlCommand();
            command.Connection = SqlConnection;
            command.Transaction = transaction;

            //update sql文作成  sql文に「UPDATE テーブル名 SET」を入れる
            StringBuilder sql;
            if (tableName == "")
            {
                sql = new(@"UPDATE " + typeof(T).Name + " SET ");
            }
            else
            {
                sql = new(@"UPDATE " + tableName + " SET ");
            }

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
                var param = new MySqlParameter(map.PropertyInfo.Name, map.Value);
                command.Parameters.Add(param);
            }

            //Where句
            if (phraseWhere != "" && phraseWhere != null)
            {
                sql.AppendLine(" " + phraseWhere);
            }

            //sqlをコマンドにセットする
            command.CommandText = sql.ToString();

            //パラメータを使わない場合null
            if (parameters != null)
            {
                //where句のパラメータをコマンドにセットする
                foreach (var parameter in parameters)
                {
                    //コマンドにパラメータ名、パラメータに格納する値をセットする
                    var param = new MySqlParameter(parameter.Name, parameter.Value);
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
        public override void Delete<T>(MySqlTransaction transaction, string pharaseWhere="", IEnumerable<CommandParameter> parameters = null, string tableName = "")
        {
            //SqlCommandに、コネクションとトランザクションをセットする
            MySqlCommand command = new MySqlCommand();
            command.Connection = this.SqlConnection;
            command.Transaction = transaction;

            //sql文作成
            //テーブル名
            StringBuilder sql = new(@"DELETE FROM ");
            if (tableName == "")
            {
                sql.AppendLine(typeof(T).Name);
            }
            else
            {
                sql.AppendLine(tableName);
            }


            //Where句
            if (pharaseWhere != "" && pharaseWhere != null)
            {
                sql.AppendLine(" " + pharaseWhere);
            }

            //sqlをコマンドにセットする
            command.CommandText = sql.ToString();


            //パラメータを使わない場合null
            if (parameters != null)
            {
                //where句のパラメータをコマンドにセットする
                foreach (var parameter in parameters)
                {
                    //コマンドにパラメータ名、パラメータに格納する値をセットする
                    var param = new MySqlParameter(parameter.Name, parameter.Value);
                    command.Parameters.Add(param);
                }
            }

            //DELETE実行
            command.ExecuteNonQuery();
        }
    }
}
