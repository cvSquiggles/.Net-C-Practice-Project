Console.WriteLine("Hello, Worldzzz!");

List<string> myGroceryList = ["apple", "milk"];


IEnumerable<string> ieString = myGroceryList;

//foreach(string x in ieString)
//{
    //Console.WriteLine(x);
//}

string[,,] multidimensionalArray = {
    
        {
            {"apple", "2 Dollars"}, 
            {"bananas", "3 Dollars"}
        },
        {
            {"milk", "Free with purchase of soda"}, 
            {"soda", "9 Dollars"}
        }
    };

//Console.WriteLine(multidimensionalArray[1,0,1]);

Dictionary<string, decimal> groceryPrices = new Dictionary<string, decimal>();

groceryPrices["Cheese"] = 5.29m;

Console.WriteLine(groceryPrices["Cheese"]);