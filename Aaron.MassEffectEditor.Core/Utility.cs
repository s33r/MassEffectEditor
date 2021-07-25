using System.Collections.Generic;

namespace Aaron.MassEffectEditor.Core
{
    public static class Utility
    {
        public static IEnumerable<T> CreateList<T>(int count) 
            where T: new()
        {
            List<T> result = new List<T>(count);

            PopulateList(count, result);

            return result;
        }       

        public static void PopulateList<T>(int count, List<T> list) 
            where T : new()
        {
            for (int j = 0; j < count; j++)
            {
                list.Add(new T());
            }
        }

    }
}
