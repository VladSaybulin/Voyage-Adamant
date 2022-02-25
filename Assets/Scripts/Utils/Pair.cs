namespace Utils
{
    public class Pair<T, K>
    {
        public T First;
        public K Second;

        public Pair(T first, K second)
        {
            First = first;
            Second = second;
        }
    }
}