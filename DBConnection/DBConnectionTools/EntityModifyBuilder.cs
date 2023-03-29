using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBConnectionTools
{
    /// <summary>
    /// エンティティモディファイクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityModifyBuilder<T>
    {
        /// <summary>
        /// プロパティの情報を格納するクラス
        /// </summary>
        public class PropertyValueMap
        {
            /// <summary>
            /// プロパティの情報
            /// </summary>
            public MemberInfo PropertyInfo { get; }

            /// <summary>
            /// プロパティの値
            /// </summary>
            public object Value { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="prop"></param>
            /// <param name="val"></param>
            public PropertyValueMap(MemberInfo propertyInfo, object value)
            {
                this.PropertyInfo = propertyInfo;
                this.Value = value;
            }
        }

        /// <summary>
        /// プロパティの情報と値を格納するリスト
        /// </summary>
        public IEnumerable<PropertyValueMap> PropertyValueMaps { get { return PrivatePropertyValueMaps; } }
        private List<PropertyValueMap> PrivatePropertyValueMaps { get; } = new();

        /// <summary>
        /// プロパティの情報と値を取得するメソッド
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="prop"></param>
        /// <param name="propertyValue"></param>
        public void Add<TValue>(Expression<Func<T, TValue>> prop, TValue propertyValue)
        {
            //MemberExpressionにキャスト
            //例外処理：キャストできなかった時
            if (!(prop.Body is MemberExpression))
            {
                throw new ArgumentException(prop.Body + "MemberExpression型にキャストできませんでした");
            }

            //MemberExpressionにキャスト
            var member = (MemberExpression)prop.Body;

            //リストに格納
            PrivatePropertyValueMaps.Add(new PropertyValueMap(member.Member, propertyValue));
        }
    }
}
