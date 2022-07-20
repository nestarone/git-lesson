namespace ToDoList.Domain
{
    public class TodoListItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        internal TodoListItem(User user,string title)
        {
            Title = title;
            Content = string.Empty;
            User = user?? throw new ArgumentNullException(nameof(user));
            UserId = user.Id;
        }

        private TodoListItem()
        {
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetContent(string content)
        {
            Content = content;
        }
    }
}
