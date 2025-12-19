namespace BehringerMonitor.Helpers
{
    internal static class CollectionExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(IEnumerable<T?> e)
            where T : class
        {
            return e.Where(e => e != null)!;
        }
    }
}
