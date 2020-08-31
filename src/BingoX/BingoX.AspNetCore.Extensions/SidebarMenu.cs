namespace BingoX.AspNetCore
{
    public class SidebarMenu
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool Active { get; set; }
        public string Icon { get; set; }

        public SidebarMenu[] Childs { get; set; }
    }
}
