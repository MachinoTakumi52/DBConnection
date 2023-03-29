using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnectionTools
{
    public class CommandParameter
    {
        /// <summary>
        /// パラメータ名
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// パラメータに格納してある値
        /// </summary>
        public object Value { get; }
        /// <summary>
        /// parameterオブジェクトのデータの型
        /// </summary>
        public DbType? DBType { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="paraName">パラメータ名</param>
        /// <param name="value">パラメータに入れる値</param>
        /// <param name="dbType">Parameter オブジェクトのデータ型(入れなくても可)</param>
        public CommandParameter(string paraName, object value, DbType? type = null)
        {
            this.Name = paraName;
            this.Value = value;
            this.DBType = type;
        }
    }
}
