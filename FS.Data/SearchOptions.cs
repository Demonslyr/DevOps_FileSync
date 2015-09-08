using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Data
{
    using System;
    using System.Collections.Generic;

    public partial class SearchOptions
    {
//        public int Id { get; set; }
        public Nullable<int> StartDate { get; set; }
        public Nullable<int> StopDate { get; set; }
        public Nullable<int> LogTypeId { get; set; }
        public Nullable<int> InteractionTypeId { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string PCName { get; set; }
        public string FileName { get; set; }
        public Nullable<int> FileOperationId { get; set; }
//        public string ErrorMsg { get; set; }

    }
}
