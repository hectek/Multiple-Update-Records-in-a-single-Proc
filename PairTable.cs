using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Sabio.Data;
using Microsoft.SqlServer.Server;

namespace Web.Classes.Data
{
    public class PairTable : IEnumerable<SqlDataRecord>
    {

        private IEnumerable<PairRequest> _pairs;

        public PairTable(IEnumerable<PairRequest> pairs)
        {

            _pairs = pairs;
        }

        private static SqlDataRecord GetPairRecord()
        {
            return new SqlDataRecord(
                    new SqlMetaData[] { new SqlMetaData("One", SqlDbType.Int)
                                      , new SqlMetaData("Two", SqlDbType.Int)}
               );
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            if (_pairs != null)
            {
            
                foreach (PairRequest item in _pairs)
                {

                    var rec = GetPairRecord();

                    rec.SetInt32(0, item.One);
                    rec.SetInt32(1, item.Two);

                    yield return rec;
                }

            }

            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }
    
    // For convenience rest of classes will be set below
    
      public class PairRequest
    {
        public int One { get; set; }
        public int Two { get; set; }
    }
    
        public class PairRequests
    {
        public List<PairRequest> Pairs {get; set;}
    }
    
    
}
