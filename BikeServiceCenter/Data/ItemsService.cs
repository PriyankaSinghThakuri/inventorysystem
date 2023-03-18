using System.Text.Json;

namespace BikeServiceCenter.Data;

/// A service class that provides CRUD operations for items.
public static class ItemsService
{
    /// Saves the list of items to a file.
    private static void SaveAll(Guid userId, List<ItemsData> items)
    {
        // Get the path of the file to save the items to
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemsFilePath = Utils.GetItemsFilePath();

        // Create the directory if it doesn't exist
        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        // Serialize the items to a JSON string
        var json = JsonSerializer.Serialize(items);

        // Save the JSON string to the file
        File.WriteAllText(itemsFilePath, json);
    }

    /// <summary>
    /// Gets all the items from the file.
    /// </summary>
    /// <returns>The list of items.</returns>
    public static List<ItemsData> GetAll()
    {
        // Get the path of the file containing the items
        string itemsFilePath = Utils.GetItemsFilePath();

        // Return an empty list if the file doesn't exist
        if (!File.Exists(itemsFilePath))
        {
            return new List<ItemsData>();
        }

        // Read the JSON string from the file
        var json = File.ReadAllText(itemsFilePath);

        // Deserialize the JSON string to a list of items
        return JsonSerializer.Deserialize<List<ItemsData>>(json);
    }

    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <param name="itemName">The name of the item.</param>
    /// <param name="quantity">The quantity of the item.</param>
    /// <param name="takenoutdate">The date when the item was taken out.</param>
    /// <returns>The list of items after the new item has been added.</returns>
    public static List<ItemsData> Create(Guid userId, Guid id, string itemName, int quantity, DateTime takenoutdate)
    {
        // Get the current list of items
        List<ItemsData> items = GetAll();

        // Validate the input
        if (quantity <= 0)
        {
            throw new Exception("Quantity cannot be zero or less than zero.");
        }
        if (itemName == null)
        {
            throw new Exception("ItemName cannot be empty.");
        }

        // Add the new item to the list
        items.Add(new ItemsData
        {
            ItemName = itemName,
            QuantityinIntentory = quantity,
            TakenOutDate = takenoutdate
        });

        // Save the updated list of items
        SaveAll(userId, items);

        // Return the updated list of items
        return items;
    }

    /// <summary>
    /// Gets all the items from the file. Delete the selected item.
    /// </summary>
    /// <returns>The list of taken items.</returns>
    public static List<ItemsData> Delete(Guid userId, Guid id)
    {
        // Get the list of taken items
        List<ItemsData> items = GetAll();
        //find the  item that needs to be deleted
        ItemsData item = items.FirstOrDefault(x => x.Id == id);

        // If the item is not found, throw an exception
        if (item == null)
        {
            throw new Exception("Item not found.");
        }

        //Detele the selected item
        items.Remove(item);
        // Save the updated list of items
        SaveAll(userId, items);
        // Return the updated list
        return items;
    }

    // This method deletes the file that stores the Item objects
    public static void DeleteByUserId(Guid userId)
    {
        // Get the file path of the file that stores the Item objects
        string itemsFilePath = Utils.GetItemsFilePath();

        // Check if the file exists
        if (File.Exists(itemsFilePath))
        {
            // If the file exists, delete it
            File.Delete(itemsFilePath);
        }
    }

    // This method updates an item in the list of items
    public static List<ItemsData> Update(Guid userId, Guid id, string itemName, int quantity, DateTime takenoutdate)
    {
        // Get the list of all items
        List<ItemsData> items = GetAll();

        // Find the item that needs to be updated
        ItemsData itemToUpdate = items.FirstOrDefault(x => x.Id == id);

        // If the item is not found, throw an exception
        if (itemToUpdate == null)
        {
            throw new Exception("Item not found.");
        }

        // Update the properties of the item
        itemToUpdate.ItemName = itemName;
        itemToUpdate.QuantityinIntentory = quantity;
        itemToUpdate.TakenOutDate = takenoutdate;

        // Save the updated list of items
        SaveAll(userId, items);

        // Return the updated list of items
        return items;
    }

    //this method allows the user to withdraw a certain quantity of an item from the inventory
    public static List<ItemsData> WithdrawItem(Guid userId, Guid id, string itemName, int quantity)
    {
        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Check if it is within working hours (9am to 4pm, Monday to Friday)
        if (currentTime.Hour >= 9 && currentTime.Hour < 16 && currentTime.DayOfWeek >= DayOfWeek.Monday && currentTime.DayOfWeek <= DayOfWeek.Friday)
        {
            // Get all items from the database
            List<ItemsData> items = GetAll();
            // Find the item with the specified ID
            ItemsData itemUpdate = items.FirstOrDefault(x => x.Id == id);

            // If item is not found, throw an exception
            if (itemUpdate == null)
            {
                throw new Exception("Item not found.");
            }

            // If the requested quantity is less than or equal to 0, throw an exception
            if (quantity <= 0)
            {
                throw new Exception("Quantity cannot be 0 or less");
            }
            else
            {
                // Update the item's name and reduce the quantity in inventory by the requested amount
                itemUpdate.ItemName = itemName;
                itemUpdate.QuantityinIntentory = itemUpdate.QuantityinIntentory - quantity;
                // Save the updated item list to the database
                SaveAll(userId, items);
                // Return the updated item list
                return items;
            }
        }
        else
        {
            // If it is not within working hours, throw an exception
            throw new Exception("Sorry, the withdrawal time is over");
        }
    }



    //This methos allows the user to reverse a previous withdrawal of an item from the inventory.
    public static List<ItemsData> RejectWithdrawItem(Guid userId, Guid id, string itemName, int quantity)
    {
        // Get the current list of items
        List<ItemsData> items = GetAll();

        // Find the item with the specified ID
        ItemsData itemUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemUpdate == null)
        {
            // If the item is not found, throw an exception
            throw new Exception("Item not found.");
        }
        else
        {
            // Update the item's name and quantity in the inventory
            itemUpdate.ItemName = itemName;
            itemUpdate.QuantityinIntentory = itemUpdate.QuantityinIntentory + quantity;

            // Save the updated list of items
            SaveAll(userId, items);
            return items;
        }
    }


    //This method allows the user to cancel a previous withdrawal of an item from the inventory.
    public static List<ItemsData> CancelWithdrawItem(Guid userId, Guid id, string itemName, int quantity)
    {
        // Get the current list of items
        List<ItemsData> items = GetAll();

        // Find the item with the specified ID
        ItemsData itemUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemUpdate == null)
        {
            // If the item is not found, throw an exception
            throw new Exception("Item not found.");
        }

        // Update the item's name and quantity in the inventory
        itemUpdate.ItemName = itemName;
        itemUpdate.QuantityinIntentory = itemUpdate.QuantityinIntentory + quantity;

        // Save the updated list of items
        SaveAll(userId, items);
        return items;
    }

}