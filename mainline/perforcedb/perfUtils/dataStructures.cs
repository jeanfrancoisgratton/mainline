// perforceDB : perfUtils
// dataStructures.cs : data structures used in PerfDB
// 2013.03.11 04:40

namespace JFG.Ubisoft.Perforce
{
    public struct ResultatStruct
    {
        public string Alias;
        public int Allocated;
        public int Archive;
        public string DbstorageType;

        // other fields
        public string Description;
        public string Dudb;
        public string Estimated_finish;
        public int Estimated_size;
        public string Fpp;
        public int Hide;
        public int Journal_size;
        public string Location;
        public string Name;
        public string Note;
        public string Owner;
        public int Port;
        public string Restore_date;
        public string Server, Version;
    }

    public struct DBConnectionStruct
    {
        public int Port, ConnectionID;
        public string Server, Database, Username, Password;
    };

    public static partial class perfUtils
    {
        public static ResultatStruct ClearRS()
        {
            ResultatStruct rs = new ResultatStruct();

            rs.Name = rs.Alias = rs.Server = rs.Version = rs.Owner = rs.Estimated_finish = rs.Dudb = rs.Location = rs.Fpp =
                rs.Note = rs.Restore_date = rs.DbstorageType = "";
            rs.Port = rs.Hide = rs.Allocated = rs.Estimated_size = rs.Archive = rs.Journal_size = 0;

            return rs;
        }
    }
}