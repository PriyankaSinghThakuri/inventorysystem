using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BikeServiceCenter.Data
{
    public static class WithdrawlService
    {
        // Save the list of withdrawl items to a file
        private static void SaveAll(Guid userId, List<WithdrawlItem> withdrawl)
        {
            // Get the path to the application data directory
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();

            // Get the path to the withdrawl items file
            string withdrawlFilePath = Utils.GetWithdrawlFilePath();

            // Create the application data directory if it doesn't exist
            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            // Serialize the withdrawl items to a JSON string
            var json = JsonSerializer.Serialize(withdrawl);

            // Write the JSON string to the withdrawl items file
            File.WriteAllText(withdrawlFilePath, json);
        }



        //// Get the list of withdrawl items from the file
        public static List<WithdrawlItem> GetAll()
        {
            // Get the path to the withdrawl items file
            string appInventoryFilePath = Utils.GetWithdrawlFilePath();

            // If the file doesn't exist, return an empty list
            if (!File.Exists(appInventoryFilePath))
            {
                return new List<WithdrawlItem>();
            }

            // Read the contents of the withdrawl items file
            var json = File.ReadAllText(appInventoryFilePath);

            // Deserialize the JSON string to a list of withdrawl items
            return JsonSerializer.Deserialize<List<WithdrawlItem>>(json);
        }




        //This method creates a new WithdrawlItem object and adds it to a list of withdrawl items.
        public static List<WithdrawlItem> Create(Guid userId, Guid itemId, string itemName, int quantity, string takerName)
        {
            // Validate the quantity of the withdrawl item
            if (quantity <= 0)
            {
                // If the quantity is less than or equal to 0, throw an exception
                throw new Exception("Withdraw quantity must be greater than 0.");
            }
            else
            {
                // Get the current list of withdrawl items
                List<WithdrawlItem> withdrawlItems = GetAll();

                // Add the new withdrawl item to the list
                withdrawlItems.Add(new WithdrawlItem
                {
                    Quantity = quantity,
                    TakenBy = userId,
                    TakerName = takerName,
                    ItemId = itemId,
                    IsApproved = false,
                    ItemName = itemName,
                });

                // Save the updated list of withdrawl items
                SaveAll(userId, withdrawlItems);
                return withdrawlItems;
            }
        }



        //This method deletes a withdrawl item from a list of withdrawl items

        public static List<WithdrawlItem> Delete(Guid userId, Guid id)
        {
            // Get the current list of withdrawl items
            List<WithdrawlItem> withdrawlItems = GetAll();

            // Find the withdrawl item with the specified ID
            WithdrawlItem itemDelete = withdrawlItems.FirstOrDefault(x => x.Id == id);

            // If the item is not found, throw an exception
            if (itemDelete == null)
            {
                throw new Exception("Item not found.");
            }

            // Remove the item from the list
            withdrawlItems.Remove(itemDelete);

            // Save the updated list of withdrawl items
            SaveAll(userId, withdrawlItems);
            return withdrawlItems;
        }


    }
}
