using SQLite;
using TableAttribute = SQLite.TableAttribute;

namespace MauiTotpApp.Shared
{
    [Table("key")]
    class TotpKeys
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
