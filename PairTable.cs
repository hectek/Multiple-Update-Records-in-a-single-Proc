using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sabio.Web.Domain;
using System.Data;
using System.Data.SqlClient;
using Sabio.Data;
using Sabio.Web.Models.Requests;
using Microsoft.SqlServer.Server;

namespace Sabio.Web.Classes.Data
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
}