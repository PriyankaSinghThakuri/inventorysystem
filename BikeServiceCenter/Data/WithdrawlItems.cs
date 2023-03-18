using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeServiceCenter.Data
{
    public class WithdrawlItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Please provide the task name.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Please provide the Required Quantity.")]
        public int Quantity { get; set; }
        public Guid ItemId { get; set; }
        public bool IsApproved { get; set; }
        public Guid TakenBy { get; set; }
        public string TakerName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
