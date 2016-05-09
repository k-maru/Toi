using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    [Serializable]
    public class SqlTemplateyException : Exception
    {
        public SqlTemplateyException() : base(Resource.GeneralErrorMessage)
        {
        }

        public SqlTemplateyException(string message) : base(message)
        {
        }

        protected SqlTemplateyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SqlTemplateyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
