using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BikeServiceCenter.Data
{
    public static class ApprovedItemService
    {
        // Save the list of approved items to a file
        private static void SaveAll(List<ApprovedItem> approvedItems)
        {
            // Get the path to the application data directory
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();

            // Get the path to the approved items file
            string approvedFilePath = Utils.GetApprovedFilePath();

            // Create the application data directory if it doesn't exist
            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            // Serialize the approved items to a JSON string
            var json = JsonSerializer.Serialize(approvedItems);

            // Write the JSON string to the approved items file
            File.WriteAllText(approvedFilePath, json);
        }

        // Get the list of approved items from the file
        public static List<ApprovedItem> GetAll()
        {
            // Get the path to the approved items file
            string appApprovedFilePath = Utils.GetApprovedFilePath();

            // If the file doesn't exist, return an empty list
            if (!File.Exists(appApprovedFilePath))
            {
                return new List<ApprovedItem>();
            }

            // Read the contents of the approved items file
            var json = File.ReadAllText(appApprovedFilePath);

            // Deserialize the JSON string to a list of approved items
            return JsonSerializer.Deserialize<List<ApprovedItem>>(json);
        }

        // Create a new approved item
        public static List<ApprovedItem> Create(Guid userId, string itemName, Guid itemid, int quantity, string takerName, Guid approverID, string approverName, bool isApproved)
        {
            // Get the current list of approved items
            List<ApprovedItem> approvedItems = GetAll();

            // Add the new approved item to the list
            approvedItems.Add(new ApprovedItem
            {
                Quantity = quantity,
                ItemId = itemid,
                TakenBy = userId,
                TakerName = takerName,
                IsApproved = isApproved,
                ItemName = itemName,
                ApprovedBy = approverID,
                ApproverName = approverName,
            });

            // Save the updated list of approved items
            SaveAll(approvedItems);
            return approvedItems;
        }
    }

}
