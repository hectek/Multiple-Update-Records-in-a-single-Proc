// Javascript function to send 'records[]' to be updated
// records[i] have properties id and column2
// *Not inclued is an Ajax function to send data records to be updated*

        // UPDATE PAGES WITH NEW PAGE ORDER
        function _updateRecords(records) {
            if(records){
                var updateList = [];
                for (var i = 0; i < records.length; i++) {
                    var curRecord = records[i];
                    var pair = {
                        One: null
                        , Two: null
                    };
                    pair.One = curRecord.id;
                    pair.Two = curRecord.column2;
                    updateList.pairs.push(pair);
                }
                // Use an AJAX function to send List of updates
                'AJAXFUNCTION'(JSON.stringify(updateList));
            }
        }
       
