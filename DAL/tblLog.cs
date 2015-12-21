namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLog")]
    public partial class tblLog
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        [StringLength(50)]
        public string LogType { get; set; }

        public int? ProductID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public virtual tblUser tblUser { get; set; }
    }
}
