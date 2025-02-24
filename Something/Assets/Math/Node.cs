public class Node<T>
{
    public T Data;
    public int number;
    public Node<T> Next;
    public Node<T> Previous;

    public Node(T data, Node<T> next, Node<T> previous)
    {
        Data = data;
        Next = next;
        Previous = previous;
    }

    public void AddNode(T data, Node<T> head)
    {
        Node<T> searchNode = head;

        while (!searchNode.Next.Equals(head))
        {
            searchNode = searchNode.Next;
        }
        searchNode.Next = new(data, head, searchNode);
        searchNode.Next.Previous = searchNode;
        head.Previous = searchNode.Next;
    }

    public void RemoveNode(T data, Node<T> head)
    {
        Node<T> searchNode = head;

        while (!searchNode.Next.Equals(head) && !searchNode.Next.Data.Equals(data))
        {
            searchNode = searchNode.Next;
        }
        if (searchNode.Next != head)
        {
            searchNode.Next = searchNode.Next.Next;
            searchNode.Next.Previous = searchNode;
        }
    }
}