using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace YourNamespace
{
    public class UpdateService : BaseService
    {
        // MULTIPLE UPDATE OF Column2
        // Reference to PairTable, pairs and PairRequest ... For convenience in the same file Pair Classes
        
        public void PairTables(PairRequests pairs)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.MultipleUpdateRecords"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   SqlParameter o = new SqlParameter("@Ordering", System.Data.SqlDbType.Structured);

                   if (pairs != null  && pairs.Pairs.Any())
                   {
                       o.Value = new PairTable(pairs.Pairs);
                   }

                   paramCollection.Add(o);

               }
               );

        }
    }
}
