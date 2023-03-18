// This class represents an item in the inventory
using System;
using System.ComponentModel.DataAnnotations;

namespace BikeServiceCenter.Data
{
    public class ItemsData
    {
        // The unique ID of the item
        public Guid Id { get; set; } = Guid.NewGuid();

        // The name of the item
        [Required(ErrorMessage = "Please provide the Item name.")]
        public string ItemName { get; set; }

        // The quantity of the item in the inventory
        public int QuantityinIntentory { get; set; }

        // The date when the item was taken out
        [Required(ErrorMessage = "Please provide the Takenout Date.")]
        public DateTime TakenOutDate { get; set; }
    }
}