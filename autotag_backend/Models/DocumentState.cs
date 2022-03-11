using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class DocumentState
    {
        public DocumentState()
        {
            Documents = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; }
    }
}
