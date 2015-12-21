using System;
namespace JewelleryStore.Models
{
    public class Log
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string LogType { get; set; }
        public string ProductName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}