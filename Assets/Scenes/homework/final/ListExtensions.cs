using System;
using System.Collections;
using System.Collections.Generic;


public static class ListExtensions
{
    private static Random rng = new Random();  


    public static void Replace<T>(this List<T> list, Predicate<T> oldItemSelector , T newItem)
    {
        //check for different situations here and throw exception
        //if list contains multiple items that match the predicate
        //or check for nullability of list and etc ...
        var oldItemIndex = list.FindIndex(oldItemSelector);
        list[oldItemIndex] = newItem;
    }

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}